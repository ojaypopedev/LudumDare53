using HotDogCannon.FoodPrep;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FoodObjectRenderer : MonoBehaviour
{

    public Transform exampleTransform;

    public string Layer;

    public FoodObject current;
    public void RenderFoodObject(FoodObject obj)
    {

        Debug.Log("trying to rendder food object");

        if(current)
        {
            Destroy(current.gameObject);
        }

        current = Instantiate(obj);

        Destroy(current.GetComponentInChildren<Rigidbody>());
        current.enabled = false;

        SetLayerRecursively(current.transform, LayerMask.NameToLayer(Layer));

        current.ingredients.ForEach(e => Debug.Log("Ingredient:" + e.foodName.ToLower().Contains("bottle")));

        //if (current.ingredients.Find(e => e.foodName.ToLower().Contains("bottle")) != null)
        //{
        //    current.transform.localScale *= 2.5f;
        //}// == "Ketchup Bottle" || e.foodName == ""))
        current.transform.parent = transform;

        current.transform.position = exampleTransform.position;
        current.transform.rotation = exampleTransform.transform.rotation;
        //current.transform.localScale = exampleTransform.localScale;

    }

  
    void SetLayerRecursively(Transform obj, int layerIndex)
    {
        obj.gameObject.layer = layerIndex;

        foreach (Transform child in obj)
        {
            SetLayerRecursively(child, layerIndex);
        }
    }

}
