using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using HotDogCannon.FoodPrep;

namespace HotDogCannon.Player {
    public class AmmoLoader : MonoBehaviour
    {
        public Gun linkedGun;
        public UnityEvent<FoodObject> onFoodDropped;

        public ParticleSystem foodLoaderParticles;

        private void OnTriggerEnter(Collider other)
        {
            if (other.attachedRigidbody == null) return;

            foodLoaderParticles?.Play();

            var foodObj = other.attachedRigidbody.GetComponent<FoodObject>();

            if(foodObj != null)
            {
                linkedGun.loadGun(foodObj);
                onFoodDropped?.Invoke(foodObj);
            }

            foodObj.gameObject.SetActive(false);
        }
    }

    
}
