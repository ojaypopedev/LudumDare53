using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ArcLineRenderer : MonoBehaviour
{
    LineRenderer renderer => GetComponent<LineRenderer>();
    public ArcCalculator calculator;

    public float force = 10;

    public GameObject End;

    // Update is called once per frame
    void Update()
    {
        ArcData data = calculator.GetLocalArcData();
        renderer.positionCount = data.positions.Length;
        renderer.SetPositions(data.positions);
        if(End)
        {
            End.transform.position = data.positions[data.positions.Length - 1];
        }
    }
}
