using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnboardingHotDog : MonoBehaviour
{
    Vector3 startpos;
    Quaternion startRot;

    private void Awake()
    {
        startpos = transform.position;
        startRot = transform.rotation;
    }

    public HotDogCannon.FoodPrep.FoodObject foodObj;
    private void Update()
    {
        if(transform.position.y <= 0)
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            transform.position = startpos;
            transform.rotation = startRot;
        }
    }
}
