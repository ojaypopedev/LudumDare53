using HotDogCannon.FoodPrep;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{

    public List<Recipie> RecipiesInLevel = new List<Recipie>();
    public List<Customer> Customers = new List<Customer>();
    public float OrderTimeMin = 2, OrderTimeMax = 5;
    public int totalOrders = 10;
    public Vector2 mimMaxTimeBetweenOrders;


    public bool freezeOrders;
    public static System.Action<bool> onCompletedOrder;
    public static System.Action<bool, FoodOrder> onCompletedFoodOrder;
    public static System.Action<Customer> onGivenOrder;

    public CharacterCustomizations customizations;

    int ordersCompleted;

    public static CustomerManager instance;

    public Customer GetRandomAvailableCustomer()
    {
        return Customers.Where(c => c.HasFoodOrder == false).RandomElement();
    }
    public Recipie GetRandomRecipie() => RecipiesInLevel.RandomElement();

    private void Awake()
    {     
        instance = this;
        GameManager.onGameStarted += OnGameStarted;
    }

    public void Init(LevelManager.Level level)
    {
        customizations = level.characterCustomizations;
        Customers = level.stadium.customers;
        RecipiesInLevel = level.recipies;
        OrderTimeMin = level.minMaxOrderTime.x;
        OrderTimeMax = level.minMaxOrderTime.y;
        totalOrders = level.numOrders;
        mimMaxTimeBetweenOrders = level.minMaxTimeBetweenOrders;
        ordersCompleted = 0;

        Customers.ForEach(e => e.Init(this));
    }

    public void OnGameStarted()
    {
        StartCoroutine(NextCustomer());
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            GetRandomAvailableCustomer().AssignFoodOrder(GetRandomRecipie(), Random.Range(OrderTimeMin, OrderTimeMax));
        }
    }

    public void OnReset()
    {

    }

    public void SetFreezeOrders(bool on)
    {
        freezeOrders = on;
    }

    public void OnFoodOrderCompleted(bool success, FoodOrder foodOrder)
    {
        onCompletedOrder?.Invoke(success);
        onCompletedFoodOrder?.Invoke(success, foodOrder);

        if (success)
            ordersCompleted++;

        if(ordersCompleted >= totalOrders)
        {
            GameManager.CompleteLevel(GameManager.CompleteState.WIN);
        }
        else
        {
            StartCoroutine(NextCustomer());
        }

        Debug.Log($"Food Order Complete : {success}");
    }

    IEnumerator NextCustomer()
    {
        var extraTime = Random.Range(mimMaxTimeBetweenOrders.x, mimMaxTimeBetweenOrders.y);
        yield return new WaitForSeconds(extraTime);
        if (GameManager.gameState == GameManager.GameState.PLAYING)
        {
            if (!freezeOrders)
            {
                var totalTime = Random.Range(OrderTimeMin, OrderTimeMax);
                var Customer = GetRandomAvailableCustomer();
                Customer.AssignFoodOrder(GetRandomRecipie(), totalTime);

                onGivenOrder?.Invoke(Customer);
            }
        }
    }
}

public static class ListExtensionMethods
{

    public static T RandomElement<T>(this IEnumerable<T> enumerable) => enumerable.ToList()[Random.Range(0, enumerable.ToList().Count)];
    public static T RandomElement<T>(this IList<T> list) => list[Random.Range(0, list.Count)];  


}

