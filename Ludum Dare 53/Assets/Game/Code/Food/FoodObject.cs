using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HotDogCannon.Utils;

namespace HotDogCannon.FoodPrep
{
    public class FoodObject : MonoBehaviour
    {
        [Header("Setup")]
        public Transform mergepos;
        public Rigidbody rb;
        public Collider col;

        [HideInInspector] public Ingredient ingredient;

        FoodSpawner fromSpawner;

        // Actions
        System.Action onGrabbed;

        public void OnSpawn(Ingredient fromeIngredient)
        {
            ingredient = fromeIngredient;
        }

        public void LinkSpawner(FoodSpawner foodSpawner)
        {
            fromSpawner = foodSpawner;
        }

        public void Grab(Transform grabTarget)
        {
            rb.isKinematic = true;

            PositionAnims.AnimatPos(transform, transform.position, grabTarget, 0.2f);

            onGrabbed?.Invoke();
        }
    }
}
