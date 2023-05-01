using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnboardingHotDog : MonoBehaviour
{
    public BaseOnboardingEvent linkedEvennt;
    public HotDogCannon.FoodPrep.FoodObject foodObj;
    private void Awake()
    {
        HotDogCannon.Player.PlayerHandController.instance.gun.onLoadedAmmo += OnLoadedAmmo;
    }

    public void OnLoadedAmmo(HotDogCannon.FoodPrep.FoodObject obj)
    {
        if(obj == foodObj) { linkedEvennt.CompleteOnboarding(); }

    }

    private void OnDestroy()
    {
        HotDogCannon.Player.PlayerHandController.instance.gun.onLoadedAmmo -= OnLoadedAmmo;
    }
}
