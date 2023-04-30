using HotDogCannon.FoodPrep;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer : MonoBehaviour
{
    public FoodOrder currentFoodOrder { get; private set; }
    FoodOrderUI ui;
    public CustomerManager manager;

    [SerializeField] Transform uiTransform;

    public CustomCharacter character;

    public Highlight Highlight => character.Highlight;
    public bool HasFoodOrder => currentFoodOrder != null;

    private void Awake()
    {
        GameManager.onReset += OnReset;

    }

    public void Init(CustomerManager manager)
    {
        this.manager = manager;



        CustomCharacter[] characters = GetComponentsInChildren<CustomCharacter>(true);

        for (int i = 0; i < characters.Length; i++)
        {
            characters[i].gameObject.SetActive(false);
        }
        character = characters[Random.Range(0, characters.Length - 1)];
        character.gameObject.SetActive(true);
        character.Setup(manager.customizations.GetRandom());
        character.Animator.Play("Sitting", -1, Random.Range(0f, 1f));
    }
    public void AssignFoodOrder(Recipie recipie, float totaltime)
    {
        if(currentFoodOrder != null)
        {
            return;
        }

        currentFoodOrder = new FoodOrder(recipie, this, totaltime);

    }
    public void GiveFood(FoodObject foodObject)
    {
        if (currentFoodOrder != null)
        {
            if(foodObject != null)
            {
                EffectsManager.CreateParticles(EffectsManager.OrderRecievedParticles, transform.position + transform.forward, null);
                CompleteFoodOrder(currentFoodOrder.recipie.CompareFoodObject(foodObject), currentFoodOrder);
                currentFoodOrder = null;
                
            }
        }
        else
        {
            Debug.Log("Customer has no food order!");
        }

    }
    public void CompleteFoodOrder(bool success, FoodOrder order = null)
    {
        manager.OnFoodOrderCompleted(success, order);
    }

    // Update is called once per frame
    void Update()
    {
        if(currentFoodOrder != null)
        {
            if (!ui)
            {
                ui = FoodOrderUI.Create(currentFoodOrder);
                ui.transform.position = uiTransform.position;
                ui.transform.parent = transform;
            }

            if(currentFoodOrder.TimeLeft <= 0)
            {
                CompleteFoodOrder(false, currentFoodOrder);
                currentFoodOrder.OnTimeRanOut?.Invoke();
                currentFoodOrder = null;
            }
        }
        else
        {
            if(ui)
            {
                ui.Hide();
            }
        }
    }

    public void OnReset()
    {
        currentFoodOrder = null;
    }
}

public class FoodOrder
{
    public Recipie recipie;
    public Customer customer;
    public float StartTime;
    public float TotalTime;
    public float TimeLeft => TotalTime - (Time.time - StartTime);
    public float TimeElapsed => (Time.time - StartTime);
    public float TimeLeftPercentage => TimeLeft / TotalTime;

    public System.Action OnTimeRanOut;

    public FoodOrder(Recipie recipie, Customer owner,  float totaltime)
    {
        this.customer = owner;
        StartTime = Time.time;
        this.recipie = recipie;
        this.TotalTime = totaltime;
    }

}

