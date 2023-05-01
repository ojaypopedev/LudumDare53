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

        public int numOrders;

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

    public List<FoodGrabbers> foodSpawners = new List<FoodGrabbers>();

    public Level currentLevel
    {
        get
        {
            if(_currentLevelIndex >= levels.Count)
            {
                _currentLevelIndex = 0;
            }

            return levels[_currentLevelIndex];
        }
    }

    public int currentLevelIndex => _currentLevelIndex;

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

        GameManager.CompleteLevel(GameManager.CompleteState.FAIL);
    }

    void OnReset()
    {
        StopAllCoroutines();
        LoadLevel(_currentLevelIndex);
        currentTime = currentLevel.overallTimeLimit;
        onGameTimerChanged?.Invoke();
    }

    void OnLevelComplete(GameManager.CompleteState completeState)
    {
        if (completeState == GameManager.CompleteState.WIN && _currentLevelIndex < levels.Count - 1)
            _currentLevelIndex++;
    }

    void LoadLevel(int levelIndex)
    {

        levels.ForEach(l =>
        {
            l.stadium.gameObject.SetActive(false);
        });

        levels[_currentLevelIndex].stadium.gameObject.SetActive(true);

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
