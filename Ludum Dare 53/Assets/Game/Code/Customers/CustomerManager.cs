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
    public Vector2 mimMaxTimeBetweenOrders;


    public bool freezeOrders;
    public static System.Action<bool> onCompletedOrder;
    public static System.Action<bool, FoodOrder> onCompletedFoodOrder;
    public static System.Action<Customer> onGivenOrder;

    public CharacterCustomizations customizations;

    int ordersCompleted;

    public static CustomerManager instance;

    public Customer[] CurrentOrderingCustomers => Customers.Where(c => c.HasFoodOrder == true).ToArray();

    public float MinDistanceToOtherCustomer = 6f;

    public Customer GetRandomAvailableCustomer()
    {
        return Customers.Where(c => {

            bool hasOrder = c.HasFoodOrder;

            bool closetoCurrentCustomer = false;
            foreach (var item in CurrentOrderingCustomers)
            {
                if(Vector3.Distance(item.transform.position, c.transform.position) < MinDistanceToOtherCustomer)
                {
                    closetoCurrentCustomer = true;
                    break;
                }
            }

            return hasOrder == false && closetoCurrentCustomer == false;


        } ).RandomElement();
    }
    public Recipie GetRandomRecipie() => RecipiesInLevel.RandomElement();

    private void Awake()
    {     
        instance = this;
        GameManager.onGameStarted += OnGameStarted;
    }

    Recipie currentRecipe;
    Recipie lastRecipie;

    Recipie GetNextRecipe()
    {
        if(RecipiesInLevel.Count > 1)
        {
            var loopProtecter = 0;

            var nextRecipe = GetRandomRecipie();

            while (nextRecipe == lastRecipie && loopProtecter < 200)
            {
                nextRecipe = GetRandomRecipie();
                loopProtecter++;
            }

            return nextRecipe;
        }
        else
        {
            return GetRandomRecipie();
        }
    }

    public void Init(LevelManager.Level level)
    {
        customizations = level.characterCustomizations;
        Customers = level.stadium.customers;
        RecipiesInLevel = level.recipies;
        OrderTimeMin = level.minMaxOrderTime.x;
        OrderTimeMax = level.minMaxOrderTime.y;
        mimMaxTimeBetweenOrders = level.minMaxTimeBetweenOrders;
        ordersCompleted = 0;

        Customers.ForEach(e => e.Init(this));
    }

    public void OnGameStarted()
    {
        currentRecipe = GetNextRecipe();
        lastRecipeChange = Time.time;
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


        StartCoroutine(NextCustomer());


        Debug.Log($"Food Order Complete : {success}");
    }

    float lastRecipeChange = 0;

    IEnumerator NextCustomer()
    {
        var gun = HotDogCannon.Player.PlayerHandController.instance.gun;
        var prepTime = (currentRecipe != null && gun.currentAmmo != null && currentRecipe.CompareFoodObject(gun.currentAmmo) && gun.currentRounds > 0)
            ? Random.Range(0, 1) : Random.Range(15, 20);

        var extraTime = Random.Range(mimMaxTimeBetweenOrders.x, mimMaxTimeBetweenOrders.y);

        yield return new WaitForSeconds(extraTime);
        if (GameManager.gameState == GameManager.GameState.PLAYING)
        {
            if (!freezeOrders)
            {

                if(Time.time - lastRecipeChange > 15 && gun.currentRounds == 0)
                {
                    currentRecipe = GetNextRecipe();
                }

                var totalTime = Random.Range(OrderTimeMin, OrderTimeMax) + prepTime;
                var Customer = GetRandomAvailableCustomer();
                Customer.AssignFoodOrder(currentRecipe, totalTime);

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

