using HotDogCannon.Player;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIAmmoDisplay : MonoBehaviour
{
    public Gun gunToDisplay;
    public GameObject[] images;
    [SerializeField] TextMeshProUGUI textMesh;

    // Update is called once per frame
    void Update()
    {
        GetComponent<CanvasGroup>().alpha = gunToDisplay.currentAmmo != null ? 1 : 0;
        for (int i = 0; i < images.Length; i++)
        {
            images[i].SetActive(gunToDisplay.currentRounds > i);
        }

        textMesh.text = gunToDisplay.currentRounds.ToString();
    }
}
