using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDScreen : BaseScreen
{

    public override void Awake()
    {
        GameManager.onGameStarted += Show;
        GameManager.onGameFinished += OnGameFinished;
        base.Awake();
    }

    void OnGameFinished(GameManager.CompleteState completeState)
    {
        Hide();
    }

}
