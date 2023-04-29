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

    private void Awake()
    {
        GameManager.onReset += OnReset;
        GameManager.onGameFinished += OnLevelComplete;
        GameManager.onGameStarted += OnGameStarted;
    }

    void OnGameStarted()
    {
        
    }

    void OnReset()
    {
        LoadLevel(_currentLevelIndex);
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
        customerManager.Customers = currentLevel.stadium.customers;
        customerManager.RecipiesInLevel = currentLevel.recipies;
        customerManager.OrderTimeMin = currentLevel.minMaxOrderTime.x;
        customerManager.OrderTimeMax = currentLevel.minMaxOrderTime.y;
        customerManager.Init();

    }

    private void OnDestroy()
    {
        GameManager.onReset -= OnReset;
        GameManager.onGameFinished -= OnLevelComplete;
        GameManager.onGameStarted -= OnGameStarted;
    }
}
