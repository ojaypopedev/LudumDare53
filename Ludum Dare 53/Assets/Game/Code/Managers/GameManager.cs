using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    // Actions
    public static System.Action onReset;
    public static System.Action onGameStarted;
    public static System.Action<CompleteState> onGameFinished;
    

    // Enums
    public enum CompleteState
    {
        WIN,
        FAIL
    }
    public enum GameState
    {
        INIT,
        MENU,
        PAUSED,
        PLAYING,
        COMPLETE,
    }

    public enum GameMode
    {
        TUTORIAL,
        STORY,
        ENDLESS
    }

    public static GameState gameState;
    public static GameMode gameMode
    {
        get
        {
            return _gameMode;
        }

        set
        {
            _gameMode = value;
            onGameModeChanged?.Invoke();
        }
    }

    static GameMode _gameMode;

    public static System.Action onGameModeChanged;

    // UNITY
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Reset();
    }


    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.S))
        {
            StartGame();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Reset();
        }
#endif


    }


    // PUBLIC
    public static void Reset()
    {
        if (gameState == GameState.MENU) return;

        gameState = GameState.MENU;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        onReset?.Invoke();
    }

    public static void StartGame()
    {
        if (gameState == GameState.PLAYING) return;

        gameState = GameState.PLAYING;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        onGameStarted?.Invoke();
    }

    public void PauseGame()
    {
        PauseGame(true);
    }

    public void UnPauseGame()
    {
        PauseGame(false);
    }

    static GameState lastState;
    public static void PauseGame(bool paused, bool showMenu = false)
    {
        lastState = gameState;

        if (gameState == GameState.PAUSED && paused) return;

        if (paused)
        {
            gameState = GameState.PAUSED;
        }
        else
        {
            gameState = lastState;
        }
    }

    public static void SetGameMode(GameMode mode)
    {
        gameMode = mode;
    }

    public void ForceCompleteLevel()
    {
        CompleteLevel(CompleteState.WIN);
    }

    public void forceMouseVisible()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void forceMouseHidden()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public static void CompleteLevel(CompleteState completeState)
    {
        if (gameState == GameState.COMPLETE) return;

        gameState = GameState.COMPLETE;

        onGameFinished?.Invoke(completeState);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
