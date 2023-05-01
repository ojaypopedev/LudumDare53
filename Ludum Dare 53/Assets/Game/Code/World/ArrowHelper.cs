using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowHelper : MonoBehaviour
{
    public static ArrowHelper instance;
    public Transform target;
    public GameObject model;
    private void Awake()
    {
        instance = this;
        model.gameObject.SetActive(false);
        BaseOnboardingEvent.onEventStarted += OnStartedOnboarding;
        BaseOnboardingEvent.onEventCompleted += OnCompleteOnboarding;
        GameManager.onReset += OnReset;
    }

    void OnReset()
    {
        RemoveCurrentTarget();
    }

    private void Update()
    {
        if(target != null)
        {
            transform.position = target.position;
            transform.position += Vector3.up * 0.3f;
        }
    }

    public void RemoveCurrentTarget()
    {
        target = null;
        model.gameObject.SetActive(false);
    }

    public void OnStartedOnboarding(BaseOnboardingEvent e)
    {
        if (e.arrowHelperTarget)
        {
            target = e.arrowHelperTarget;
            model.gameObject.SetActive(true);
        }

    }

    public void OnCompleteOnboarding(BaseOnboardingEvent e)
    {
        model.gameObject.SetActive(false);
        target = null;
    }
}
