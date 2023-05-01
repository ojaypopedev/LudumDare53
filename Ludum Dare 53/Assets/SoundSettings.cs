using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSettings : MonoBehaviour
{

    public GameObject X;
    public CanvasGroup Speaker;

    public static bool SoundActive
    {
        get => PlayerPrefs.GetInt("SOUND_ACTIVE", 1) == 1 ? true : false;
        set => PlayerPrefs.SetInt("SOUND_ACTIVE", value == true ? 1 : 0);
    }

    private void Start()
    {
        Refresh();
    }

    public void ToggleSound()
    {
        SoundActive = !SoundActive;
        Refresh();
    }

    void Refresh()
    {
        X.SetActive(!SoundActive);
        Speaker.alpha = SoundActive ? 1 : 0.5f;
    }





}
