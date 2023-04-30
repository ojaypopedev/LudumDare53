using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatTracker : MonoBehaviour
{
    public static StatTracker instance;

    public static int completedOrders;
    public static int hotDogsFired;
    public static int failedOrders;
    static List<float> orderTimes = new List<float>();
    public static float averageTime
    {
        get
        {
            var total = 0f;

            orderTimes.ForEach(t => total += t);

            return total / orderTimes.Count;
        }
    }
    public static float accuracy
    {
        get
        {
           var acc =  (float)completedOrders/ (float)hotDogsFired;
           if (float.IsNaN(acc))
               acc = 0;

            return acc;
        }
    }

    private void Awake()
    {
        GameManager.onReset += OnReset;
    }

    // Start is called before the first frame update
    void Start()
    {
        CustomerManager.onCompletedFoodOrder += OnOrderCompleted;
        var gun = HotDogCannon.Player.PlayerHandController.instance.gun;
        gun.onFired += OnFiredGun;
    }

    void OnFiredGun()
    {
        hotDogsFired++;
    }

    void OnReset()
    {
        completedOrders = 0;
        hotDogsFired = 0;
        failedOrders = 0;
        orderTimes.Clear();
    }

    public void OnOrderCompleted(bool success, FoodOrder foodOrder)
    {
        orderTimes.Add(foodOrder.TimeElapsed);
        if (success)
            completedOrders++;
        else
            failedOrders++;
    }
}
