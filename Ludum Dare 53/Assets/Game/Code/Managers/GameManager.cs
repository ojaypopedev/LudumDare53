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
        PLAYING,
        COMPLETE,
    }

    public static GameState gameState;

    // UNITY
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Reset();
    }

//#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            StartGame();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Reset();
        }
    }
//# endif

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

    public static void CompleteLevel(CompleteState completeState)
    {
        if (gameState == GameState.COMPLETE) return;

        gameState = GameState.COMPLETE;

        onGameFinished?.Invoke(completeState);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
