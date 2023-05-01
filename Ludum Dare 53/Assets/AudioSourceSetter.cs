using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceSetter : MonoBehaviour
{
    AudioSource source => GetComponent<AudioSource>();
    private void Update()
    {
        source.volume = (SoundSettings.SoundActive ? 0.6f : 0f);
    }
}
