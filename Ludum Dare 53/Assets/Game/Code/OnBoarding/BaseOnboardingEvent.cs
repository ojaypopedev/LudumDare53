using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BaseOnboardingEvent : MonoBehaviour
{
    public string eventName;

    [TextArea]
    public string onBoardingText;

    public System.Action onEventStarted;
    public System.Action onEventCompleted;

    public UnityEvent onStarted;
    public UnityEvent onCompleted;

    public bool autoNextStep;

    public virtual void StartOnboarding()
    {
        if(onBoardingText != "")
        {
            UIOnboardingMessage.instance.ShowMessage(onBoardingText);
        }
        onEventStarted?.Invoke();
        onStarted?.Invoke();
    }

    public virtual void CompleteOnboarding()
    {
        UIOnboardingMessage.instance.OnClickContinue();
        onEventCompleted?.Invoke();
        onCompleted?.Invoke();
    }
}
