using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HotDogCannon.Player;

public class CustomerFeedOnboardingHelper : MonoBehaviour
{
    public bool infiniteAmmo;

    BaseOnboardingEvent onboardingEvent;

    public void Awake()
    {
        onboardingEvent = GetComponent<BaseOnboardingEvent>();
        CustomerManager.onCompletedFoodOrder += OnCompletedOrder;
    }

    private void Start()
    {
        PlayerHandController.instance.gun.onFired += OnFired;
    }

    void OnCompletedOrder(bool success, FoodOrder order)
    {
        if (success)
        {
            onboardingEvent.CompleteOnboarding();
        }
    }

    void OnFired()
    {
        if (infiniteAmmo && onboardingEvent.isStarted && !onboardingEvent.isCompleted)
            PlayerHandController.instance.gun.FroceSetAmmoCount(5);
    }
}
