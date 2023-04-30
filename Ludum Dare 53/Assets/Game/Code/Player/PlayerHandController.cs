using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HotDogCannon.FoodPrep;

namespace HotDogCannon.Player
{
    public class PlayerHandController : MonoBehaviour
    {
        public static PlayerHandController instance;

        [Header("Setup")]
        public Transform pivotPoint;
        public Transform rayCastPoint;
        public Gun gun;
        public Animator handAnim;

        [Header("Sensitivity")]
        public float moveSensitivity;
        public float rotSensitivity;

        [Header("movement constraints (localPos)")]
        public Vector2 minMaxForward;
        public Vector2 minMaxLeft;
        public Vector2 minMaxUp;

        public Vector2 minMaxDistance;

        [Header("CameraSettings")]
        public Vector3 prepCamRot;
        public Vector2 minMaxCamAimPos;

#if UNITY_EDITOR
        [Header("Dev Settings")]
        public bool testOverride;
#endif

        float currentLeft;
        float currentForward;
        float currentRot;
        float upAxisDT;

        Animator anim;

        Vector3 startPos;

        // Actions
        public static System.Action onPlayerHandEmpty;
        public static System.Action onPlayerInteract;
        public static System.Action<bool> onPlayerEquippedGun;

        private void Awake()
        {
            startPos = pivotPoint.transform.position;
            anim = GetComponent<Animator>();
            instance = this;
        }

        // Update is called once per frame
        void Update()
        {
#if UNITY_EDITOR
            if (testOverride)
            {

            }
            else
#endif
            if (GameManager.gameState != GameManager.GameState.PLAYING) return;
            Grabbing();
            GunInput();
           
        }

        private void FixedUpdate()
        {
            if (GameManager.gameState != GameManager.GameState.PLAYING) return;
            RadialMovement();
            Gun();
        }

        Highlight HotDogloaderHighlight;

        public void Grabbing()
        {
            RaycastHit hit;

            handAnim.SetBool("isGrabbing", FoodObject.currentGrabbed != null);

            var fromPos = FoodObject.currentGrabbed == null ? rayCastPoint.position : FoodObject.currentGrabbed.transform.position;

            if (Physics.Raycast(fromPos, Vector3.down, out hit, 2))
            {
                FoodObject foodObj = hit.transform.gameObject.GetComponent<FoodObject>();

                if (foodObj != null)
                {
                    if (FoodObject.currentGrabbed == null ||
                        (FoodObject.currentGrabbed != null && FoodObject.currentGrabbed != foodObj))
                        foodObj.OnHandOver();

                    else
                    {
                        onPlayerHandEmpty?.Invoke();
                    }
                }
                else
                {
                    onPlayerHandEmpty?.Invoke();
                }

                if(hit.transform.gameObject.layer == LayerMask.NameToLayer("HotDogLoader"))
                {
                    HotDogloaderHighlight = hit.transform.gameObject.GetComponent<Highlight>();
                    HotDogloaderHighlight.SetHighlight(true);
                }
                else 
                {
                    if (HotDogloaderHighlight && HotDogloaderHighlight.highlight == true)
                        HotDogloaderHighlight.SetHighlight(false);
                }

                if (Input.GetMouseButtonDown(0))
                {
                    if (FoodObject.currentPotentialGrab != null)
                    {
                        FoodObject.currentPotentialGrab.Grab(rayCastPoint);
                    }
                }

                if (Input.GetMouseButtonUp(0))
                {

                    if (HotDogloaderHighlight && HotDogloaderHighlight.highlight && FoodObject.currentGrabbed)
                    {
                        FoodObject.currentGrabbed.SetPhysics(true, false);
                        Utils.PosAnims.AnimatPos(FoodObject.currentGrabbed.transform, FoodObject.currentGrabbed.transform.position,
                            HotDogloaderHighlight.transform.position, 0.2f);

                        FoodObject.currentGrabbed.UnGrab();
                    }
                    else
                    {

                        if (FoodObject.currentGrabbed != null)
                            FoodObject.currentGrabbed.UnGrab();
                    }
                }
            }
        }

        public void GunInput()
        {
            gun.Refresh(new Vector2(Mathf.InverseLerp(-1, 1, currentLeft), Mathf.InverseLerp(-1, 1, currentForward)));
        }

        public void Gun()
        {
            var forwardDot = Vector3.Dot(Vector3.forward, transform.forward);

            bool isShooting = forwardDot < -0.85f;
            anim.SetBool("IsShooting", isShooting);
            var wasActive = gun.isActive;
            gun.isActive = isShooting;

            if(wasActive != gun.isActive)
            {
                onPlayerEquippedGun?.Invoke(gun.isActive);
            }

            var camRot = Camera.main.transform.localRotation;

            if (!gun.isActive)
                Camera.main.transform.localRotation = Quaternion.Lerp(camRot, Quaternion.Euler(prepCamRot), 3 * Time.fixedDeltaTime);
            else
                Camera.main.transform.localRotation = Quaternion.Lerp(camRot, Quaternion.Euler(new Vector3(Mathf.Lerp(minMaxCamAimPos.y, minMaxCamAimPos.x, currentForward), camRot.eulerAngles.y, camRot.eulerAngles.z)), 10 * Time.fixedDeltaTime);

          
        }

        float radialAngle;

        public void RadialMovement()
        {
            Vector2 inputDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

            inputDelta = inputDelta * moveSensitivity;

            currentForward += inputDelta.y;

            currentLeft = Mathf.Clamp(currentLeft, -1, 1);
            currentForward = Mathf.Clamp(currentForward, 0, 1);
            var localxPos = Mathf.Lerp(minMaxLeft.x, minMaxLeft.y, Mathf.InverseLerp(-1, 1, currentLeft));
            var localzPos = Mathf.Lerp(minMaxForward.x, minMaxForward.y, Mathf.InverseLerp(-1, 1, currentForward));

            radialAngle += inputDelta.x * rotSensitivity;
            var dir = Quaternion.AngleAxis(radialAngle, Vector3.up) * transform.forward;

            var dis = Mathf.Lerp(minMaxDistance.x, minMaxDistance.y, currentForward);

            var pos = transform.position + (transform.forward * dis);

            var forwardDot = Vector3.Dot(Vector3.forward, transform.forward);

            upAxisDT = Mathf.Clamp01(Mathf.InverseLerp(.9f, 0, forwardDot));
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, radialAngle, 0), Time.fixedDeltaTime * 10);

            var localyPos = Mathf.Lerp(minMaxUp.x, minMaxUp.y, upAxisDT);

            pos.y = localyPos;

            pivotPoint.transform.position = Vector3.Lerp(pivotPoint.transform.position,pos, Time.fixedDeltaTime * 10);
        }

        public void Movement()
        {
            Vector2 inputDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

            inputDelta = inputDelta * moveSensitivity;

            currentLeft += inputDelta.x;
            currentForward += inputDelta.y;

            currentLeft = Mathf.Clamp(currentLeft, -1, 1);
            currentForward = Mathf.Clamp(currentForward, -1, 1);

            var localxPos = Mathf.Lerp(minMaxLeft.x, minMaxLeft.y, Mathf.InverseLerp(-1, 1, currentLeft));
            var localzPos = Mathf.Lerp(minMaxForward.x, minMaxForward.y, Mathf.InverseLerp(-1, 1, currentForward));
         

            var rotDirection = 0;
            if (currentLeft <= -1) rotDirection = -1;
            if (currentLeft >= 1) rotDirection = 1;

            currentRot += rotDirection * rotSensitivity * Time.deltaTime;


            var forwardDot = Vector3.Dot(Vector3.forward, transform.forward);

            upAxisDT = Mathf.Clamp01(Mathf.InverseLerp(.9f, 0, forwardDot));

            var localyPos = Mathf.Lerp(minMaxUp.x, minMaxUp.y, upAxisDT);

            pivotPoint.transform.localPosition = startPos + (new Vector3(localxPos, localyPos, localzPos));
        }
    }
}
