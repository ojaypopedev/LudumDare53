using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Stadium : MonoBehaviour
{
    public List<Customer> customers = new List<Customer>();


    [ContextMenu("Auto Populate")]
    public void AutoPopulate()
    {
        customers.Clear();
        customers = GetComponentsInChildren<Customer>().ToList();
    }
}
