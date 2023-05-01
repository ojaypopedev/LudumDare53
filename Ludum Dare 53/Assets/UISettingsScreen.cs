using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Unity.Burst;

public class UISettingsScreen : BaseScreen
{
    private static UISettingsScreen __instance__;
    private static UISettingsScreen Instance
    {
        get
        {
            if (!__instance__)
            {
                __instance__ = FindObjectOfType<UISettingsScreen>(true);
            }

            return __instance__;
        }
    }

    public Button closeButton;
    public Slider sensitivitySlider;

    public UnityEvent OnClosed = null;

    public static void ShowSettingsScreen()
    {
        Instance.Show();
    }
    public override void Show()
    {
        closeButton.onClick.RemoveAllListeners();
        closeButton.onClick.AddListener(() => {

            Hide();
            OnClosed?.Invoke();
          
        });

        sensitivitySlider.onValueChanged.RemoveAllListeners();
        sensitivitySlider.onValueChanged.AddListener(SetSenstivity);
        base.Show();

    }

    public static void SetSenstivity(float Value01)
    {
        Debug.Log("Sensitity Set" + Value01.ToString());
    }

}
  
