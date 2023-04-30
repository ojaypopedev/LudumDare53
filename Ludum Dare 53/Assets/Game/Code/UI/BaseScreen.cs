using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseScreen : MonoBehaviour
{
    [Header("Screen Setup")]
    public GameObject panel;

    public bool startHidden;

    public virtual void Awake()
    {
        panel.SetActive(!startHidden);
    }

    public virtual void Show()
    {
        panel.gameObject.SetActive(true);
    }

    public virtual void Hide()
    {
        panel.gameObject.SetActive(false);
    }
}
