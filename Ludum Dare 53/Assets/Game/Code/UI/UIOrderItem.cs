using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HotDogCannon.Utils;

public class UIOrderItem : MonoBehaviour
{
    public Gradient timerColor;

    public RawImage icon;
    public Customer customer;

    public Image fill;

    UIOrdersList linkedList;

    public void Refresh(Customer c, UIOrdersList list)
    {
        linkedList = list;
        if (c.currentFoodOrder == null) return;
        c.currentFoodOrder.OnTimeRanOut += OnOrderFailed;
        customer = c;
        icon.texture = c.currentFoodOrder.recipie.icon;
    }


    private void OnEnable()
    {
        if(customer == null || customer.currentFoodOrder == null
            || customer.currentFoodOrder.TimeLeft <= 0)
        {
            linkedList.items.Remove(this);
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (customer == null || customer.currentFoodOrder == null) return;
        var timeDT = customer.currentFoodOrder.TimeLeftPercentage;
        fill.fillAmount = timeDT;
        fill.color = timerColor.Evaluate(timeDT);
    }

    void OnOrderFailed()
    {
        PosAnims.AnimateShake(transform, transform.position, 3, 1, () =>
        {
            if (this == null) return;
            linkedList.items.Remove(this);
            Destroy(gameObject);
        });
    }

    
}
