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

        //Replace the strings here for the actual information.
        tutorialButton.Refresh(onboardingManager.isOnboardingComplete ? "Complete" : "Incomplete", LoadTutorial);
        storyButton.Refresh("Level 1/3", LoadStoryLevel, !onboardingManager.isOnboardingComplete, "Complete Tutorial");
        endlessButton.Refresh("Best 2:35", LoadEndlessMode, !onboardingManager.isOnboardingComplete, "Complete Tutorial");

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

    }

    public void LoadStoryLevel()
    {

    }

    public void LoadEndlessMode()
    {

    }
    //public void OnClickStart()
    //{
    //    GameManager.StartGame();
    //}

}
