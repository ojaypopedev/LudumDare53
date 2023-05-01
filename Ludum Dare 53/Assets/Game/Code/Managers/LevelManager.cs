using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using HotDogCannon.FoodPrep;

public class LevelManager : MonoBehaviour
{
    [System.Serializable]
    public class Level
    {
        public string levelName;

        public Stadium stadium;

        public bool hasLives;

        public List<Recipie> recipies = new List<Recipie>();

        public Vector2 minMaxOrderTime;

        [Tooltip("overall time in seconds")]
        public float overallTimeLimit;

        public Vector2 minMaxTimeBetweenOrders;

        public CharacterCustomizations characterCustomizations;

        public UnityEvent onLevelStart;
    }

    [System.Serializable]
    public class FoodGrabbers
    {
        public string ingredientNamel;
        public Ingredient linkedIngredient;
        public FoodObject obj;
        public FoodSpawner spawner;

        public void SetOn(bool on)
        {
            if (obj) obj.gameObject.SetActive(on);
            if (spawner)
            {
                spawner.gameObject.SetActive(on);
                if(on) spawner.BeginSpawn();
            }
        }
    }

    public List<Level> levels = new List<Level>();

    public List<Level> storyLevels => levels.FindAll(l => l.hasLives);

    public Level endlessLevel => levels.Find(l => !l.hasLives);

    public List<FoodGrabbers> foodSpawners = new List<FoodGrabbers>();

    public Level currentLevel
    {
        get
        {
            if (GameManager.gameMode == GameManager.GameMode.STORY)
            {

                if (_currentLevelIndex >= storyLevels.Count)
                {
                    _currentLevelIndex = 0;
                }

                return storyLevels[_currentLevelIndex];
            }

            else
            {
                return endlessLevel;
            }
        }
    }

    public int currentLevelIndex => _currentLevelIndex;

    public int currentLives = 0;

    int _currentLevelIndex
    {
        get
        {
            return PlayerPrefs.GetInt("HotDogCannon.levelNum", 0);
        }

        set
        {
            PlayerPrefs.SetInt("HotDogCannon.levelNum", value);
        }
    }

    public static string getTimeLeftString
    {
        get
        {
            var mins = Mathf.Floor(currentTime / 60f);
            var seconds = currentTime - (mins * 60);

            return (mins + ":" + (seconds == 0 ? "00" : seconds));
        }
    }

    public static System.Action onGameTimerChanged;

    static float currentTime;

    public static LevelManager instance;

    private void Awake()
    {
        GameManager.onReset += OnReset;
        GameManager.onGameFinished += OnLevelComplete;
        GameManager.onGameStarted += OnGameStarted;
        CustomerManager.onCompletedFoodOrder += OnOrderComplete;
        instance = this;
    }

    void OnGameStarted()
    {
        currentLevel.onLevelStart?.Invoke();
        StartCoroutine(DoGameTimer());
        SetSpawners();
    }

    IEnumerator DoGameTimer()
    {
        while(currentTime > 0)
        {
            yield return new WaitForSeconds(1);
            if (GameManager.gameState == GameManager.GameState.PLAYING)
            {
                currentTime--;
                onGameTimerChanged?.Invoke();
            }
        }

        if (GameManager.gameMode == GameManager.GameMode.STORY)
            GameManager.CompleteLevel(GameManager.CompleteState.WIN);
        else
            GameManager.CompleteLevel(GameManager.CompleteState.FAIL);
    }

    void OnReset()
    {
        StopAllCoroutines();
        LoadLevel();
        currentTime = currentLevel.overallTimeLimit;
        onGameTimerChanged?.Invoke();
    }

    void OnLevelComplete(GameManager.CompleteState completeState)
    {
        if (completeState == GameManager.CompleteState.WIN && _currentLevelIndex < storyLevels.Count - 1)
            _currentLevelIndex++;

    }

    void OnOrderComplete(bool success, FoodOrder foodOrder)
    {
        switch (GameManager.gameMode)
        {
            case GameManager.GameMode.STORY:
                if (!success)
                {
                    currentLives--;
                    if (currentLives <= 0)
                    {
                        GameManager.CompleteLevel(GameManager.CompleteState.FAIL);
                    } 
                }
                break;
            case GameManager.GameMode.ENDLESS:
                if (!success)
                {
                    currentTime -= 1f;
                }
                else
                {
                    currentTime += 5f;
                }
                break;
        }
    }

    void LoadLevel()
    {

        switch (GameManager.gameMode)
        {
            case GameManager.GameMode.STORY:
                LoadStoryLevel();
                break;
            case GameManager.GameMode.ENDLESS:
                LoadEndlessLevel();
                break;
            default:
                break;
        }

    }

    void LoadStoryLevel()
    {
        levels.ForEach(l =>
        {
            l.stadium.gameObject.SetActive(false);
        });

        currentLives = 5;

        storyLevels[_currentLevelIndex].stadium.gameObject.SetActive(true);

        var customerManager = CustomerManager.instance;

        customerManager.Init(currentLevel);

        SetSpawners();
    }

    void LoadEndlessLevel()
    {
        levels.ForEach(l =>
        {
            l.stadium.gameObject.SetActive(false);
        });

        endlessLevel.stadium.gameObject.SetActive(true);

        var customerManager = CustomerManager.instance;

        customerManager.Init(currentLevel);

        SetSpawners();
    }

    void SetSpawners()
    {
        foodSpawners.ForEach(f => f.SetOn(false));

        List<Ingredient> requiredIngredients = new List<Ingredient>();

        currentLevel.recipies.ForEach(r =>
        {
            r.ingredients.ForEach(i =>
            {
                if (requiredIngredients.Contains(i) == false)
                    requiredIngredients.Add(i);
            });
        });

        requiredIngredients.ForEach(r =>
        {
            foodSpawners.Find(f => f.linkedIngredient.foodName == r.foodName).SetOn(true);
        });
    }

    private void OnDestroy()
    {
        GameManager.onReset -= OnReset;
        GameManager.onGameFinished -= OnLevelComplete;
        GameManager.onGameStarted -= OnGameStarted;
    }
}
