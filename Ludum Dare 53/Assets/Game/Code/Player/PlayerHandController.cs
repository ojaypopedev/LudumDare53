using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HotDogCannon.FoodPrep;

namespace HotDogCannon.Player
{
    public class PlayerHandController : MonoBehaviour
    {
        [Header("Setup")]
        public Transform pivotPoint;
        public Transform rayCastPoint;
        public Gun gun;

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
            Cursor.lockState = CursorLockMode.Locked;
            anim = GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
            Grabbing();
            //Movement();
            RadialMovement();
            Gun();
        }

        public void Grabbing()
        {
            RaycastHit hit;

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

                if (Input.GetMouseButtonDown(0))
                {
                    if (FoodObject.currentPotentialGrab != null)
                    {
                        FoodObject.currentPotentialGrab.Grab(rayCastPoint);
                    }
                }

                if (Input.GetMouseButtonUp(0))
                {
                    if (FoodObject.currentGrabbed != null)
                        FoodObject.currentGrabbed.UnGrab();
                }
            }
        }

        public void Gun()
        {
            var forwardDot = Vector3.Dot(Vector3.forward, transform.forward);

            bool isShooting = forwardDot < -0.9f;
           // Debug.Log(forwardDot);
            anim.SetBool("IsShooting", isShooting);
            var wasActive = gun.isActive;
            gun.isActive = isShooting;

            if(wasActive != gun.isActive)
            {
                onPlayerEquippedGun?.Invoke(gun.isActive);
            }

            var camRot = Camera.main.transform.localRotation;

            if (!gun.isActive)
                Camera.main.transform.localRotation = Quaternion.Lerp(camRot, Quaternion.Euler(prepCamRot), 3 * Time.deltaTime);
            else
                Camera.main.transform.localRotation = Quaternion.Lerp(camRot, Quaternion.Euler(new Vector3(Mathf.Lerp(minMaxCamAimPos.y, minMaxCamAimPos.x, currentForward), camRot.eulerAngles.y, camRot.eulerAngles.z)), 10 * Time.deltaTime);

            gun.Refresh(new Vector2(Mathf.InverseLerp(-1, 1, currentLeft), Mathf.InverseLerp(-1, 1, currentForward)));
        }

        float playerAngle;
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

            Debug.Log(dir);

            var dis = Mathf.Lerp(minMaxDistance.x, minMaxDistance.y, currentForward);

            var pos = transform.position + (transform.forward * dis);

            var forwardDot = Vector3.Dot(Vector3.forward, transform.forward);

            upAxisDT = Mathf.Clamp01(Mathf.InverseLerp(.9f, 0, forwardDot));
            transform.rotation = Quaternion.Euler(0, radialAngle, 0);

            var localyPos = Mathf.Lerp(minMaxUp.x, minMaxUp.y, upAxisDT);

            pos.y = localyPos;

            pivotPoint.transform.position = pos;

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

            transform.rotation = Quaternion.Euler(0, currentRot, 0);

            var forwardDot = Vector3.Dot(Vector3.forward, transform.forward);

            upAxisDT = Mathf.Clamp01(Mathf.InverseLerp(.9f, 0, forwardDot));

            var localyPos = Mathf.Lerp(minMaxUp.x, minMaxUp.y, upAxisDT);

            pivotPoint.transform.localPosition = startPos + (new Vector3(localxPos, localyPos, localzPos));
        }
    }
}
