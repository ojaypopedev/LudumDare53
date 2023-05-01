using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIStartScreen : BaseScreen
{
    //public TextMeshProUGUI levelText;
    //public Button playButton;

    public UILevelLoadButton tutorialButton;
    public UILevelLoadButton storyButton;
    public UILevelLoadButton endlessButton;

    public Button QuitButton;

    public override void Awake()
    {
        GameManager.onGameStarted += Hide;
        GameManager.onReset += OnReset;
        base.Awake();
        //playButton.onClick.RemoveAllListeners();
        //playButton.onClick.AddListener(OnClickStart);
    }

    public void OnReset()
    {
        Show();
        //levelText.text = "Level " + (LevelManager.instance.currentLevelIndex + 1).ToString() + " : " + LevelManager.instance.currentLevel.levelName;
        var levelNum = (LevelManager.instance.currentLevelIndex + 1).ToString();
        //Replace the strings here for the actual information.
        tutorialButton.Refresh(onboardingManager.isOnboardingComplete ? "Complete" : "Incomplete", LoadTutorial);
        storyButton.Refresh( "Level " + levelNum + "/" + (LevelManager.instance.storyLevels.Count).ToString(), LoadStoryLevel, !onboardingManager.isOnboardingComplete, "Complete Tutorial");
        endlessButton.Refresh("Best: " + LevelManager.endlessHighScoreString, LoadEndlessMode, !onboardingManager.isOnboardingComplete, "Complete Tutorial");

        if(Application.platform == RuntimePlatform.WebGLPlayer)
        {
            QuitButton.gameObject.SetActive(false);
        }
        else
        {
            QuitButton.onClick.RemoveAllListeners();
            QuitButton.onClick.AddListener(Application.Quit);
        }
        //se

    }

    public void LoadTutorial()
    {
        GameManager.gameMode = GameManager.GameMode.TUTORIAL;
        GameManager.StartGame();
    }

    public void LoadStoryLevel()
    {
        GameManager.gameMode = GameManager.GameMode.STORY;
        GameManager.StartGame();
    }

    public void LoadEndlessMode()
    {
        GameManager.gameMode = GameManager.GameMode.ENDLESS;
        GameManager.StartGame();
    }
    //public void OnClickStart()
    //{
    //    GameManager.StartGame();
    //}

}
