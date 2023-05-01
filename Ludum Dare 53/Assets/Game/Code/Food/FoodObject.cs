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
        public bool isPersistent;
        public bool ignoreMerge;

        public Ingredient ingredient;

        FoodSpawner fromSpawner;

        public static FoodObject currentPotentialGrab;
        public static FoodObject currentPotentialMerge;
        public static FoodObject currentGrabbed;

        public enum GrabBehaviours
        {
            PCIKUP_SELF,
            PICKUP_INSTANTIED
        }

        public GrabBehaviours grabBehaviours;

        [Header("Pickup instanstiated options")]
        public FoodObject objectToPickup;
        public Ingredient objectToPickupIngredient;

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
        public static System.Action<FoodObject> onGrabbedGlobal;
        static System.Action<FoodObject> onGrabItemChanged;
        public static System.Action<FoodObject> onMergedFoodGlobal;

        [HideInInspector] public bool highlighted;
        [HideInInspector] public bool isMerged;
        bool dropped;

        private void Awake()
        {
            onGrabItemChanged += OnItemChanged;
            Player.PlayerHandController.onPlayerHandEmpty += OnHandExit;
            GameManager.onReset += OnReset;
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

            if(Application.isPlaying) EffectsManager.CreateParticles(EffectsManager.MergeFoodParticles, transform.position);
            fromItem.GetBehaviour().OnMerge(fromItem, this);
            currentPotentialMerge = null;
            onMergedFoodGlobal?.Invoke(this);

        }

        public void Grab(Transform grabTarget)
        {
            switch (grabBehaviours)
            {
                case GrabBehaviours.PCIKUP_SELF:
                    GrabThis(grabTarget);
                    break;
                case GrabBehaviours.PICKUP_INSTANTIED:
                    GrabInstantiated(grabTarget);
                    break;
                default:
                    break;
            }
        }

        void GrabThis(Transform grabTarget)
        {
            SetPhysics(true, false);
            dropped = false;
            PosAnims.AnimatPos(transform, transform.position, grabTarget, 0.2f, () => {
                if (this == null) return;
                if (!dropped) transform.SetParent(grabTarget, true);
            });
            currentGrabbed = this;
            onGrabbed?.Invoke(this);
            onGrabbedGlobal?.Invoke(this);
        }

        void GrabInstantiated(Transform grabTarget)
        {
            if (objectToPickup == null) return;

            var foodobj = Instantiate(objectToPickup);
            foodobj.OnSpawn(objectToPickupIngredient);
            foodobj.SetPhysics(true, false);
            foodobj.dropped = false;
            PosAnims.AnimatPos(foodobj.transform, transform.position, grabTarget, 0.2f, () =>
            {
                if (this == null) return;
                if (!dropped) foodobj.transform.SetParent(grabTarget, true);
            });
            currentGrabbed = foodobj;
            onGrabbed?.Invoke(foodobj);
            onGrabbedGlobal?.Invoke(this);
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
            if (currentGrabbed != null && ignoreMerge) return;
            mergedItems.ForEach(m => m.OnHandOver());
            tempColor = col.GetComponentInChildren<Renderer>().material.color;
            col.GetComponentsInChildren<Renderer>().ToList().ForEach(r => r.materials.ToList().ForEach(m => {
                var col = m.GetColor("_HighlightColor");
                col.a = 1;
                m.SetColor("_HighlightColor", col);
            }));

            highlighted = true;
            onGrabItemChanged?.Invoke(this);
        }

        public void OnHandExit()
        {
            if (!highlighted) return;
            mergedItems.ForEach(m => m.OnHandExit());
            currentPotentialGrab = null;
            currentPotentialMerge = null;
            col.GetComponentsInChildren<Renderer>().ToList().ForEach(r => r.materials.ToList().ForEach(m => {
                var col = m.GetColor("_HighlightColor");
                col.a = 0;
                m.SetColor("_HighlightColor", col);
            }));

            highlighted = false;
        }

        public void OnItemChanged(FoodObject fromItem)
        {
            if (isMerged) return;

            if (fromItem != this)
                OnHandExit();

            if (currentGrabbed)
            {
                if(!ignoreMerge && !fromItem.ignoreMerge)
                    currentPotentialMerge = fromItem;
            }
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
            if (!Application.isPlaying)
                DestroyImmediate(rb);
            else
                Destroy(rb);
        }

        public BaseFoodAffect GetBehaviour()
        {
            switch (ingredient.mergeAffectBehaviour)
            {
                case Ingredient.FoodAffectType.ATTACH:
                    return new AttachFood();
                case Ingredient.FoodAffectType.BOTTLE:
                    return new BottleSqueeze();
                        
            }

            return null;
        }

        public void OnReset()
        {
            if(!isPersistent)
                Destroy(gameObject);
        }

        private void OnDestroy()
        {
            GameManager.onReset -= OnReset;
            onGrabItemChanged -= OnItemChanged;
            Player.PlayerHandController.onPlayerHandEmpty -= OnHandExit;
        }
    }
}
