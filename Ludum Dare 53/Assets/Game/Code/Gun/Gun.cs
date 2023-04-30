using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HotDogCannon.FoodPrep;
using UnityEngine.UI;

namespace HotDogCannon.Player {
    public class Gun : MonoBehaviour
    {

        [Header("Setup")]
        public Transform aimPivot;
        public LayerMask mask;
        public Transform shootPoint;
        public int roundsPerRecipe = 5;
        public bool isActive;
        public float maxForce;
        public Vector2 minMaxHeight;
        [SerializeField] LineRenderer renderer;
        public GameObject crossHair;
        public Camera cam;

        public float speed = 20;
        public float maxDistance = 30;

        public float arcPower;
        public float arcMultiplier;


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
            currentLoaded = foodObject;
            _currentRounds = 5;
            foodObject.SetPhysics(true, true);
        }

        public void Refresh(Vector2 coords)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            ArcData data = new ArcData(shootPoint.position, cam, maxDistance, mask);

            ArcData.ArcPower = arcPower;
            ArcData.ArcMultiplier = arcMultiplier;

            crossHair.gameObject.SetActive(isActive);
            renderer.enabled = isActive;

            aimPivot.forward = data.StartDirection;
            renderer.positionCount = data.PositionCount;
            renderer.SetPositions(data.Positions);
            crossHair.transform.position = data.endPosition;
            if (data.HitTarget)
            {
                crossHair.transform.forward = data.HitNormal;
            }
            else
            {
                crossHair.transform.forward = cam.transform.forward;
            }
            if (Input.GetMouseButtonDown(0) && currentRounds > 0 && isActive)
            {
                FoodObject instance = Instantiate(currentLoaded);
                instance.gameObject.SetActive(true);

                ArcData.ShootAlongArc(data, instance.gameObject, speed, () =>
                {

                    if (data.HitCollider)
                    {
                        var customer = data.HitCollider.GetComponent<Customer>();

                        if (customer)
                        {
                            customer.GiveFood(instance);
                        }
                    }
                    Destroy(instance.gameObject);

                    _currentRounds--;

                    onFired?.Invoke();

                });
            }
            else if (isActive)
            {
                onNoAmmo?.Invoke();
            }
        }
    }
}
