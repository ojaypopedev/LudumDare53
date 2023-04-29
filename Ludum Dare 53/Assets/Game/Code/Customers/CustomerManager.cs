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

    public static CustomerManager instance;

    public Customer GetRandomAvailableCustomer()
    {
        return Customers.Where(c => c.HasFoodOrder == false).RandomElement();
    }
    public Recipie GetRandomRecipie() => RecipiesInLevel.RandomElement();

    private void Awake()
    {
        Customers.ToList().ForEach(e => e.Init(this));
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            GetRandomAvailableCustomer().AssignFoodOrder(GetRandomRecipie(), Random.Range(OrderTimeMin, OrderTimeMax));
        }
    }

    public void OnFoodOrderCompleted(bool success, FoodOrder foodOrder)
    {
        
        Debug.Log($"Food Order Complete : {success}");
    }

    public void OnFoodOrderFailed(FoodOrder order)
    {
        Debug.Log($"Food Order Failed)");// : {success}");
    }
}

public static class ListExtensionMethods
{

    public static T RandomElement<T>(this IEnumerable<T> enumerable) => enumerable.ToList()[Random.Range(0, enumerable.ToList().Count)];
    public static T RandomElement<T>(this IList<T> list) => list[Random.Range(0, list.Count)];  


}

