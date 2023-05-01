using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HotDogCannon.FoodPrep;

public class OnboardingHotDog : MonoBehaviour
{
    Vector3 startpos;
    Quaternion startRot;

    private void Awake()
    {
        startpos = transform.position;
        startRot = transform.rotation;
        GameManager.onGameStarted += OnGameReset;
    }

    public void OnGameReset()
    {
        if(GameManager.gameMode == GameManager.GameMode.TUTORIAL)
        {
            var rb = GetComponent<Rigidbody>();
            if (rb == null)
                rb = gameObject.AddComponent<Rigidbody>();

            GetComponent<FoodObject>().rb = rb;
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<FoodObject>().SetPhysics(false, true);
            transform.position = startpos;
            transform.rotation = startRot;
        }
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
