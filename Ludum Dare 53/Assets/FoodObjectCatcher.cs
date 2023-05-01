using HotDogCannon.FoodPrep;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodObjectCatcher : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {

        FoodObject food = other.GetComponent<FoodObject>();

        if (food != null)
        {
            if(food.isPersistent == false)
            {
                Debug.Log($"Food Destroyed: {food.gameObject}");
                Destroy(food.gameObject);
            
            }

        }
    }
}
