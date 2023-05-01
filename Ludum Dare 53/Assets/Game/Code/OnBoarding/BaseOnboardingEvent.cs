using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseOnboardingEvent : MonoBehaviour
{
    public string eventName;

    public System.Action onEventStarted;
    public System.Action onEventCompleted;

    public virtual void StartOnboarding()
    {
        onEventStarted?.Invoke();
    }

    public virtual void CompleteOnboarding()
    {
        onEventCompleted?.Invoke();
    }
}
