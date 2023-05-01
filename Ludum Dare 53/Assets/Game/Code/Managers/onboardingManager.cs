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

    BaseOnboardingEvent lastOnboarding;

    private void Awake()
    {
        GameManager.onGameStarted += OnGameStarted;
        BaseOnboardingEvent.onEventCompleted += OnOnboardingComplete;
        if(completedOnboarding == false)
        {
            PlayerPrefs.DeleteAll();
        }
    }

    public void OnGameStarted()
    {
        StartOnboarding();
    }

    public void StartOnboarding()
    {
        if (!completedOnboarding)
        {
            onboardingEvents[0].StartOnboarding();
            lastOnboarding = onboardingEvents[0];
        }
    }

    void OnOnboardingComplete(BaseOnboardingEvent e)
    {
        Debug.Log("hceking next");
        if (lastOnboarding.autoNextStep)
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
