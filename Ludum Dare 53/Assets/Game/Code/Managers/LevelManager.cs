using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    }

    public List<Level> levels = new List<Level>();

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

    private void Awake()
    {
        GameManager.onReset += OnReset;
        GameManager.onGameFinished += OnLevelComplete;
        GameManager.onGameStarted += OnGameStarted;
    }

    void OnGameStarted()
    {
        StartCoroutine(DoGameTimer());
    }

    IEnumerator DoGameTimer()
    {
        while(currentTime > 0 && GameManager.gameState == GameManager.GameState.PLAYING)
        {
            yield return new WaitForSeconds(1);
            currentTime--;
            onGameTimerChanged?.Invoke();
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
        if (completeState == GameManager.CompleteState.WIN)
            _currentLevelIndex++;
    }

    void LoadLevel(int levelIndex)
    {

        levels.ForEach(l =>
        {
            l.stadium.gameObject.SetActive(false);
        });

        currentLevel.stadium.gameObject.SetActive(true);

        var customerManager = CustomerManager.instance;
      
        customerManager.Init(currentLevel);

    }

    private void OnDestroy()
    {
        GameManager.onReset -= OnReset;
        GameManager.onGameFinished -= OnLevelComplete;
        GameManager.onGameStarted -= OnGameStarted;
    }
}
