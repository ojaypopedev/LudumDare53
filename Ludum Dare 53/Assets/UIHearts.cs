using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIHearts : MonoBehaviour
{
    private static UIHearts __instance__;
    private static UIHearts Instance
    {
        get
        {
            if (!__instance__)
            {
                __instance__ = FindObjectOfType<UIHearts>(true);
            }
            return __instance__;
        }

    }

    [SerializeField] Image[] heartImages;

    public static void EnableHearts(bool enabled = true)
    {
        Instance.gameObject.SetActive(enabled);
    }

    public static void DisableHearts()
    {
        EnableHearts(false);
    }

    public static void SetHearts(int amount, bool alsoEnable = true)
    {
        if (alsoEnable)
        {
            EnableHearts();
        }

        for (int i = 0; i < Instance.heartImages.Length; i++)
        {
            Instance.heartImages[i].enabled = amount > i;
        }

    }

    [ContextMenu("Set 1")]
    public void Set1()
    {
        SetHearts(1);
    }

    [ContextMenu("Set 2")]
    public void Set2()
    {
        SetHearts(2);
    }

    [ContextMenu("Set 3")]
    public void Set3()
    {
        SetHearts(3);
    }
    
    [ContextMenu("Disable")]
    public void Disable()
    {
        DisableHearts();

    }

}




    
