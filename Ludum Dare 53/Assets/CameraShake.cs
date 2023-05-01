using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private static CameraShake __instance__;
    private static CameraShake Instance
    {
        get
        {
            if (!__instance__)
            {
                __instance__ = FindObjectOfType<CameraShake>(true);
            }

            return __instance__;
        }
    }

    public enum ShakeSize { Small, Medium, Large};
    public static void Shake(ShakeSize shake)
    {
        switch (shake)
        {
            case ShakeSize.Small:
                Shake(0.05f, 0.1f);
                break;
            case ShakeSize.Medium:
                Shake(0.1f, 0.2f);
                break;
            case ShakeSize.Large:
                Shake(0.025f, 0.2f);
                break;
            default:
                break;
        }
    }

    private static void Shake(float intensity, float time)
    {
        Instance.shakeTimer = time;
        
        if(intensity > Instance.shakeIntensity)
        {
            Instance.shakeIntensity = intensity;    
        }

        Instance.shakeTimer = time;
        Instance.shakeIntensity = intensity;
    }


    public float shakeIntensity;
    public float shakeDuration;

    private float shakeTimer;
    private Vector3 originalPosition;

    void Start()
    {
        originalPosition = transform.localPosition;
    }

    void Update()
    {
        if (shakeTimer > 0)
        {
            transform.localPosition = originalPosition + Random.insideUnitSphere * shakeIntensity;

            shakeTimer -= Time.deltaTime;
        }
        else
        {
            shakeTimer = 0f;
            transform.localPosition = originalPosition;
            shakeIntensity = 0f;
        }
    }

    
}
