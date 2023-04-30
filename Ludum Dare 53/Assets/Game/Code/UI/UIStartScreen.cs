using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIStartScreen : BaseScreen
{
    public TextMeshProUGUI levelText;
    public Button playButton;

    public override void Awake()
    {
        GameManager.onGameStarted += Hide;
        GameManager.onReset += OnReset;
        base.Awake();
        playButton.onClick.RemoveAllListeners();
        playButton.onClick.AddListener(OnClickStart);
    }

    public void OnReset()
    {
        Show();
        levelText.text = "Level " + (LevelManager.instance.currentLevelIndex + 1).ToString();
    }

    public void OnClickStart()
    {
        GameManager.StartGame();
    }

}
