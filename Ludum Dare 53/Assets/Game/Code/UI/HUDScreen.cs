using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDScreen : BaseScreen
{

    public GameObject timer;

    public override void Awake()
    {
        GameManager.onReset += OnReset;
        GameManager.onGameStarted += Show;
        GameManager.onGameFinished += OnGameFinished;
        base.Awake();
    }

    void OnReset()
    {
        timer.SetActive(true);
    }

    void OnGameFinished(GameManager.CompleteState completeState)
    {
        Hide();
    }

}
