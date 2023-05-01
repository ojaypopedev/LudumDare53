using HotDogCannon.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAmmoDisplay : MonoBehaviour
{
    public Gun gunToDisplay;
    public GameObject[] images;

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < images.Length; i++)
        {
            images[i].SetActive(gunToDisplay.currentRounds >= i);
        }
    }
}
