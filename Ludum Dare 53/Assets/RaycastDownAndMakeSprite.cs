using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastDownAndMakeSprite : MonoBehaviour
{
    [SerializeField] SpriteRenderer sprite;
    

    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit))
        {
            sprite.enabled = true;
            sprite.transform.position = hit.point + Vector3.up * 0.02f;
        }
        else
        {
            sprite.enabled = false;
        }
    }
}
