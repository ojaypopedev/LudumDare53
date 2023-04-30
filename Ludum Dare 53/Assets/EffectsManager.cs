using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsManager : MonoBehaviour
{
    private static EffectsManager __instance__;
    private static EffectsManager Instance
    {
        get
        {
            if(!__instance__)
            {
                __instance__ = FindObjectOfType<EffectsManager>(true);  
            }

            return __instance__;    
        }
    }

    [SerializeField] ParticleSystem _orderRecievedParticles;
    public static ParticleSystem OrderRecievedParticles => Instance._orderRecievedParticles;

    public static ParticleSystem CreateParticles(ParticleSystem prefab, Vector3 position, Transform parent = null)
    {
        ParticleSystem ps = Instantiate(prefab, position, prefab.transform.rotation, parent);
        Destroy(ps, 3);
        return ps;
    }
}
