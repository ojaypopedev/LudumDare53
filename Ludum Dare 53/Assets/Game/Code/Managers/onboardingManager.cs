using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class onboardingManager : MonoBehaviour
{
    public static onboardingManager instance;

    public List<BaseOnboardingEvent> onboardingEvents = new List<BaseOnboardingEvent>();

    public bool completedOnboarding
    {
        get { return PlayerPrefs.GetInt("onBoarding.completed", 0) == 1; }
        set { PlayerPrefs.SetInt("onBoarding.completed", value ? 1 : 0); }
    }

    public static bool isOnboardingComplete
    {
        get { return PlayerPrefs.GetInt("onBoarding.completed", 0) == 1; }
    }

    BaseOnboardingEvent lastOnboarding;

    private void Awake()
    {
        instance = this;
        GameManager.onGameStarted += OnGameStarted;
        BaseOnboardingEvent.onEventCompleted += OnOnboardingComplete;
        if(completedOnboarding == false)
        {
            onboardingEvents.ForEach(o => o.isCompleted = false);
        }
    }

    
    public void OnGameStarted()
    {
        if(GameManager.gameMode == GameManager.GameMode.TUTORIAL)
            onboardingEvents.ForEach(o => o.isCompleted = false);
        StartOnboarding();
    }

    public void StartOnboarding()
    {
        if (GameManager.gameMode == GameManager.GameMode.TUTORIAL)
        {
            onboardingEvents[0].StartOnboarding();
            lastOnboarding = onboardingEvents[0];
        }
    }

    void OnOnboardingComplete(BaseOnboardingEvent e)
    {
        Debug.Log("hceking next");
        if (lastOnboarding != null && lastOnboarding.autoNextStep)
        {
            Debug.Log("should do next");
            var index = onboardingEvents.IndexOf(lastOnboarding);
            if (index + 1 <= onboardingEvents.Count - 1)
            {
                lastOnboarding = onboardingEvents[index + 1];
                lastOnboarding.StartOnboarding();
                Debug.Log("done next");

            }
        }

        if (e.completesTutorial)
            completedOnboarding = true;
    }
}
