using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningFlash : MonoBehaviour
{
    private static WarningFlash __instance__;
    private static WarningFlash Instance
    {
        get
        {
            if (!__instance__)
            {
                __instance__ = FindObjectOfType<WarningFlash>(true);
            }

            return __instance__;
        }
    }

    Animator anim => GetComponent<Animator>();

    private void Start()
    {
        CustomerManager.onCompletedFoodOrder += (sucess,_) => {
            
            if(!sucess)
            {
                FlashOnce();
                CameraShake.Shake(CameraShake.ShakeSize.Medium);
            }
        
        };
    }

    public static void FlashOnce()
    {
        Instance.anim.SetTrigger("FlashOnce");
    }

    public static void SetFlash(bool flashing)
    {
        Instance.anim.SetBool("Flashing", flashing);
    }



    // Update is called once per frame
    void Update()
    {
        
    }

   
}
