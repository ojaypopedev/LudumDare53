using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BaseOnboardingEvent : MonoBehaviour
{
    public string eventName;

    [TextArea]
    public string onBoardingText;

    public Transform arrowHelperTarget;

    public static System.Action<BaseOnboardingEvent> onEventStarted;
    public static System.Action<BaseOnboardingEvent> onEventCompleted;

    public UnityEvent onStarted;
    public UnityEvent onCompleted;

    public bool autoNextStep;

    public bool isCompleted
    {
        get { return PlayerPrefs.GetInt(eventName + ".onboardingComplete", 0) == 1; }
        set { PlayerPrefs.SetInt(eventName + ".onboardingComplete", value ? 1 : 0); }
    }

    public bool isStarted
    {
        get { return PlayerPrefs.GetInt(eventName + ".started", 0) == 1; }
        set { PlayerPrefs.SetInt(eventName + ".started", value ? 1 : 0); }
    }

    public virtual void StartOnboarding()
    {
        if (isCompleted || isStarted) return;
        if (onBoardingText != "")
        {
            UIOnboardingMessage.instance.ShowMessage(onBoardingText);
        }
        onEventStarted?.Invoke(this);
        onStarted?.Invoke();
        isStarted = true;
    }

    public virtual void CompleteOnboarding()
    {
        if (isCompleted || !isStarted) return;
        UIOnboardingMessage.instance.OnClickContinue();
        onEventCompleted?.Invoke(this);
        onCompleted?.Invoke();
        isStarted = false;
        isCompleted = true;
    }
}
