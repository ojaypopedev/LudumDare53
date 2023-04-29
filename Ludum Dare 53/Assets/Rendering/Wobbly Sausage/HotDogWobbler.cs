using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class HotDogWobbler : MonoBehaviour
{

    public Vector3 Axis;
    
    Renderer renderer => GetComponent<Renderer>();

    public float Magnitude = 2;
    // Update is called once per frame
    void Update()
    {
        if (Application.isPlaying == false)
        {
            return;
        }

        renderer.material.SetVector("_Axis", Axis);

        float wobble = Mathf.Sin(Time.time *10 * (1/Mathf.Max(0.1f, Magnitude))) * Magnitude * 2.5f;
       
        Magnitude -= Time.deltaTime;
        Magnitude = Mathf.Max(Magnitude, 0);
       
        renderer.material.SetFloat("_Magnitude", wobble);

        if(Input.GetKeyDown(KeyCode.Space))
        {
            Magnitude = 2;
        }

    }
}
