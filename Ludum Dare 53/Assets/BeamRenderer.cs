using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class BeamRenderer : MonoBehaviour
{

    LineRenderer renderer => GetComponent<LineRenderer>();
    float totalLength
    {
        get
        {
            Vector3[] positions = new Vector3[renderer.positionCount];
            renderer.GetPositions(positions);
            float totalDistance = 0;
            for (int i = 0; i < positions.Length-1; i++)
            {
                totalDistance += Vector3.Distance(positions[i], positions[i + 1]);
            }
            return totalDistance;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Application.isPlaying)
            renderer.material.SetFloat("_TotalLength", totalLength);   
    }
}
