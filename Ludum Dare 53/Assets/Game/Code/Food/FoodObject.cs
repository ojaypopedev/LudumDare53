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

        public static FoodObject currentPotentialGrab;
        public static FoodObject currentGrabbed;

        // Actions
        public System.Action<FoodObject> onGrabbed;
        static System.Action<FoodObject> onGrabItemChanged;


        private void Awake()
        {
            onGrabItemChanged += OnItemChanged;
            Player.PlayerHandController.onPlayerHandEmpty += OnHandExit;
        }

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
            PosAnims.AnimatPos(transform, transform.position, grabTarget, 0.2f, () => {
                if (this == null) return;
                transform.SetParent(grabTarget, true);
            });
            currentGrabbed = this;
            onGrabbed?.Invoke(this);
        }

        public void UnGrab()
        {
            if (currentGrabbed == null) return;
            transform.SetParent(null);
            rb.isKinematic = false;
            highlighted = false;
            currentGrabbed = null;
        }

        public Color tempColor;
        public bool highlighted;

        public void OnHandOver()
        {
            if (!highlighted)
                tempColor = col.GetComponent<Renderer>().material.color;

            if (currentGrabbed) return;

            col.GetComponent<Renderer>().material.color = Color.grey;
            highlighted = true;
            onGrabItemChanged?.Invoke(this);
        }

        public void OnHandExit()
        {
            if (!highlighted) return;
            currentPotentialGrab = null;
            col.GetComponent<Renderer>().material.color = tempColor;
            highlighted = false;
        }

        public void OnItemChanged(FoodObject fromItem)
        {
            if (fromItem != this)
                OnHandExit();

            currentPotentialGrab = fromItem;
        }

        private void OnDestroy()
        {
            onGrabItemChanged -= OnItemChanged;
            Player.PlayerHandController.onPlayerHandEmpty -= OnHandExit;
        }
    }
}
