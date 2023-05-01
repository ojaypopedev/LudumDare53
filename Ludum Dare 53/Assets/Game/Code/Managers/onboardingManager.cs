using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class onboardingManager : MonoBehaviour
{
    public List<BaseOnboardingEvent> onboardingEvents = new List<BaseOnboardingEvent>();

    public bool completedOnboarding
    {
        get { return PlayerPrefs.GetInt("onBoarding.completed", 0) == 1; }
        set { PlayerPrefs.SetInt("onBoarding.completed", value ? 1 : 0); }
    }

    private void Awake()
    {
        GameManager.onGameStarted += OnGameStarted;
    }

    public void OnGameStarted()
    {
        StartOnboarding();
    }

    public void StartOnboarding()
    {
        if (!completedOnboarding)
            onboardingEvents[0].StartOnboarding();
    }
}
