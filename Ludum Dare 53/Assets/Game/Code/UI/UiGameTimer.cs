using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UiGameTimer : MonoBehaviour
{
    public TextMeshProUGUI timerText;

    private void Awake()
    {
        LevelManager.onGameTimerChanged += Refresh;
        GameManager.onReset += OnReset;
    }

    void OnReset()
    {
        timerText.text = LevelManager.getTimeLeftString;
    }

    void Refresh()
    {
        timerText.text = LevelManager.getTimeLeftString;
    }
}
