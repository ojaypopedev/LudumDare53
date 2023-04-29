using System.Collections;
using System.Linq;
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
        public static FoodObject currentPotentialMerge;
        public static FoodObject currentGrabbed;

        public List<FoodObject> mergedItems = new List<FoodObject>();

        public List<Ingredient> ingredients
        {
            get
            {
                List<Ingredient> allIngredients = new List<Ingredient>();
                allIngredients.Add(ingredient);
                allIngredients.AddRange(mergedItems.Select(m => m.ingredient));

                return allIngredients;
            }
        }

        // Actions
        public System.Action<FoodObject> onGrabbed;
        static System.Action<FoodObject> onGrabItemChanged;

        [HideInInspector] public bool highlighted;
        [HideInInspector] public bool isMerged;
        bool dropped;

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

        public void Merge(FoodObject fromItem)
        {

            fromItem.GetBehaviour().OnMerge(fromItem, this);
            currentPotentialMerge = null;
        }

        public void Grab(Transform grabTarget)
        {
            SetPhysics(true, false);
            dropped = false;
            PosAnims.AnimatPos(transform, transform.position, grabTarget, 0.2f, () => {
                if (this == null) return;
                if(!dropped) transform.SetParent(grabTarget, true);
            });
            currentGrabbed = this;
            onGrabbed?.Invoke(this);
        }

        public void UnGrab()
        {
            if (currentGrabbed == null) return;
            dropped = true;
            transform.SetParent(null);

            SetPhysics(false, true);

            highlighted = false;
            currentGrabbed = null;

            if(currentPotentialMerge != null && currentPotentialMerge.ingredient != ingredient)
            {
                currentPotentialMerge.Merge(this);
            }
        }

        Color tempColor;

        public void OnHandOver()
        {
            if (highlighted || (currentGrabbed != null && currentGrabbed.ingredient == ingredient)) return;
            mergedItems.ForEach(m => m.OnHandOver());
            tempColor = GetComponentInChildren<Renderer>().material.color;
            col.GetComponentsInChildren<Renderer>().ToList().ForEach(r => r.material.color = Color.grey);
            highlighted = true;
            onGrabItemChanged?.Invoke(this);
        }

        public void OnHandExit()
        {
            if (!highlighted) return;
            mergedItems.ForEach(m => m.OnHandExit());
            currentPotentialGrab = null;
            currentPotentialMerge = null;
            col.GetComponentsInChildren<Renderer>().ToList().ForEach(r => r.material.color = tempColor);

            highlighted = false;
        }

        public void OnItemChanged(FoodObject fromItem)
        {
            if (isMerged) return;

            if (fromItem != this)
                OnHandExit();

            if (currentGrabbed)
                currentPotentialMerge = fromItem;
            else
                currentPotentialGrab = fromItem;
        }

        public void SetPhysics(bool kinematic, bool collider)
        {
            if (rb) rb.isKinematic = kinematic;
            if (col) col.enabled = collider;
        }

        public void DestroyRigidBody()
        {
            Destroy(rb);
        }

        public BaseFoodAffect GetBehaviour()
        {
            switch (ingredient.mergeAffectBehaviour)
            {
                case Ingredient.FoodAffectType.ATTACH:
                    return new AttachFood();

                        
            }

            return null;
        }

        private void OnDestroy()
        {
            onGrabItemChanged -= OnItemChanged;
            Player.PlayerHandController.onPlayerHandEmpty -= OnHandExit;
        }
    }
}
