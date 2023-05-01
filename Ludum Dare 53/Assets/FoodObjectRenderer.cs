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

        Bounds exampleBounds = new Bounds(exampleTransform.position, Vector2.zero);
        Bounds NewFoodObjectBounds = new Bounds(exampleTransform.position, Vector2.zero);

        Debug.Log("Example: " + GetBoundsZSize(exampleTransform.gameObject, ref exampleBounds).size);
        Debug.Log("New Food Object: " + GetBoundsZSize(current.gameObject, ref NewFoodObjectBounds).size);
        current.transform.parent = transform;

        current.transform.position = exampleTransform.position;
        current.transform.rotation = exampleTransform.transform.rotation;
        current.transform.localScale = exampleTransform.localScale;

    }

    public Bounds GetBoundsZSize(GameObject go, ref Bounds bounds)
    {
        

        foreach (Renderer child in GetComponentsInChildren<Renderer>(true))
        {
            if(child == go.GetComponent<Renderer>())
            {
                Debug.Log("Child: "+ child.name);
                bounds.Encapsulate(child.bounds);

                GetBoundsZSize(child.gameObject, ref bounds);
            }
          
        }

        return bounds;
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
