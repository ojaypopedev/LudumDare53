using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HotDogCannon.FoodPrep;

namespace HotDogCannon.Player {
    public class AmmoLoader : MonoBehaviour
    {
        public Gun linkedGun;

        private void OnTriggerEnter(Collider other)
        {
            if (other.attachedRigidbody == null) return;

            var foodObj = other.attachedRigidbody.GetComponent<FoodObject>();

            if(foodObj != null)
            {
                linkedGun.loadGun(foodObj);
            }
        }
    }
}
