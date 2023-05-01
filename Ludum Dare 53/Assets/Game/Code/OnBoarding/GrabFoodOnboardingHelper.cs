using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HotDogCannon.FoodPrep;
using HotDogCannon.Player;

public class GrabFoodOnboardingHelper : MonoBehaviour
{
    public Ingredient ingredient;

    public Recipie expectRecipe;

    bool canComplete;

    BaseOnboardingEvent onboardingEvent;
    
    private void Awake()
    {
        FoodObject.onGrabbedGlobal += CheckGrab;
        FoodObject.onMergedFoodGlobal += OnMerge;
        onboardingEvent = GetComponent<BaseOnboardingEvent>();
    }

    private void Update()
    {
        if(onboardingEvent.isStarted && !onboardingEvent.isCompleted && canComplete && Input.GetMouseButtonUp(0))
        {
            onboardingEvent.CompleteOnboarding();
        }
    }

    public void CheckGrab(FoodObject obj)
    {
        if(onboardingEvent.isStarted && !onboardingEvent.isCompleted && obj.ingredient.foodName == ingredient.foodName)
        {
            if(expectRecipe == null)
                canComplete = true;

            ArrowHelper.instance.RemoveCurrentTarget();
        }
    }

    public void OnMerge(FoodObject merged)
    {
        if (expectRecipe != null && onboardingEvent.isStarted && !onboardingEvent.isCompleted && expectRecipe.CompareFoodObject(merged))
        {
            onboardingEvent.CompleteOnboarding();
        }
    }
}
