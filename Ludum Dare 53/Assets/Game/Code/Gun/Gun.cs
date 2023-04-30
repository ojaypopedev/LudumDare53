using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HotDogCannon.FoodPrep;

namespace HotDogCannon.Player {
    public class Gun : MonoBehaviour
    {

        [Header("Setup")]
        public ArcCalculator arcCalculator;
        public Transform aimPivot;
        public int roundsPerRecipe = 5;
        public bool isActive;
        public float maxForce;
        public Vector2 minMaxHeight;


        // Actions
        public System.Action onNoAmmo;
        public System.Action onFired;


        public bool isLoaded { get; private set; }

        public int currentRounds
        {
            get { return _currentRounds; }
        }

        FoodObject currentLoaded;

        int _currentRounds;

        public void loadGun(FoodObject foodObject)
        {
            //if (currentLoaded != null)
              //  Destroy(currentLoaded.gameObject);

            currentLoaded = foodObject;
            _currentRounds = 5;
            foodObject.SetPhysics(true, true);
        }

        public void Refresh(Vector2 coords)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            arcCalculator.SetForce(Mathf.Lerp(0, maxForce, coords.y));
            arcCalculator.SetHeight(Mathf.Lerp(minMaxHeight.x, minMaxHeight.y, coords.y));

            aimPivot.transform.localRotation = Quaternion.Euler(Mathf.Lerp(-2, -20, coords.y), Mathf.Lerp(-20, 20, coords.x), 0);

            if (Input.GetMouseButtonDown(0) && currentRounds > 0 && isActive)
            {
                FoodObject instance = Instantiate(currentLoaded);
                instance.gameObject.SetActive(true);

                arcCalculator.ThrowOnArc(instance.gameObject, arcCalculator.GetLocalArcData(), 10, (data) =>
                {
                    if(data.HitCollider)
                    {
                        var customer = data.HitCollider.GetComponent<Customer>();

                        if (customer)
                        {
                            customer.GiveFood(instance);
                        }
                    }
                    Destroy(instance.gameObject);

                });

                _currentRounds--;

                onFired?.Invoke();
            }
            else if(isActive)
            {
                onNoAmmo?.Invoke();
            }
        }
    }
}
