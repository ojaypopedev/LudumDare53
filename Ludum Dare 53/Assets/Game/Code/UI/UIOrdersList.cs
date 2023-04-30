using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HotDogCannon.Player;

public class UIOrdersList : MonoBehaviour
{
    public UIOrderItem prefabitem;

    public List<UIOrderItem> items = new List<UIOrderItem>();

    public GameObject OrdersPanel;

    private void Awake()
    {
        CustomerManager.onGivenOrder += OnOrderGiven;
        GameManager.onReset += OnReset;
        prefabitem.gameObject.SetActive(false);
        PlayerHandController.onPlayerEquippedGun += OnPlayerEquippedGun;
    }


    void OnOrderGiven(Customer customer)
    {
        var newItem = Instantiate(prefabitem);
        newItem.transform.SetParent(prefabitem.transform.parent);
        newItem.Refresh(customer, this);
        newItem.gameObject.SetActive(true);
        items.Add(newItem);
    }

    void OnPlayerEquippedGun(bool on)
    {
        OrdersPanel.SetActive(!on);
    }

    void OnReset()
    {
        items.ForEach(i => Destroy(i.gameObject));
        items.Clear();
    }
}
