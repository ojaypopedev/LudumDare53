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

        Customer currentCustomer = null;

        public void Awake()
        {
            GameManager.onReset += OnReset;
        }

        public void OnReset()
        {
            isActive = false;
            Refresh(Vector2.zero);
        }

        public void loadGun(FoodObject foodObject)
        {
            currentLoaded = foodObject;
            _currentRounds = 5;
            foodObject.SetPhysics(true, true);
        }

        public void Refresh(Vector2 coords)
        {

            crossHair.gameObject.SetActive(isActive);
            renderer.enabled = isActive;

            if (!isActive)
            {
                var dir = transform.forward;
                dir = Vector3.Lerp(dir, Vector3.down, 2*Time.deltaTime);
                transform.LookAt(dir);

                return;
            }

            ArcData data = new ArcData(shootPoint.position, cam, maxDistance, mask);

            ArcData.ArcPower = arcPower;
            ArcData.ArcMultiplier = arcMultiplier;

            aimPivot.forward = data.StartDirection;
            renderer.positionCount = data.PositionCount;
            renderer.SetPositions(data.Positions);
            crossHair.transform.position = data.endPosition;

            if (data.HitTarget)
            {
                crossHair.transform.forward = data.HitNormal;

                Customer customer = data.HitCollider.GetComponent<Customer>();
                if (customer != null)
                {
                    if (currentCustomer != customer)
                    {
                        if (currentCustomer != null)
                        {
                            currentCustomer.Highlight.SetHighlight(false);
                            currentCustomer = null;
                        }

                        currentCustomer = customer;
                        currentCustomer.Highlight.SetHighlight(true);
                    }
                   
                }
                else
                {
                    if (currentCustomer != null)
                    {
                        currentCustomer.Highlight.SetHighlight(false);
                        currentCustomer = null;
                    }
                }

            }
            else
            {
                if (currentCustomer != null)
                {
                    currentCustomer.Highlight.SetHighlight(false);
                    currentCustomer = null;
                }

                crossHair.transform.forward = cam.transform.forward;
            }

            if (Input.GetMouseButtonDown(0) && currentRounds > 0 && isActive)
            {
                FoodObject instance = Instantiate(currentLoaded);
                instance.gameObject.SetActive(true);
                instance.transform.localScale *= 2.5f;

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
                });

                _currentRounds--;

                onFired?.Invoke();
            }
            else if (isActive)
            {
                onNoAmmo?.Invoke();
            }
        }
    }
}
