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

    // PUBLIC
    public static void Reset()
    {
        if (gameState == GameState.INIT) return;

        gameState = GameState.INIT;

        onReset?.Invoke();
    }

    public static void StarGame()
    {
        if (gameState == GameState.PLAYING) return;

        gameState = GameState.PLAYING;

        onGameStarted?.Invoke();
    }

    public static void CompleteLevel(CompleteState completeState)
    {
        if (gameState == GameState.COMPLETE) return;

        gameState = GameState.COMPLETE;

        onGameFinished?.Invoke(completeState);
    }
}
