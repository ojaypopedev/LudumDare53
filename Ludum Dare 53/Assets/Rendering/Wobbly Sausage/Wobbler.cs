using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Wobbler : MonoBehaviour
{
    Renderer rend => GetComponent<Renderer>();
    Material material => rend.material;
    float Magnitude
    {
        get
        {
            return material.GetFloat("_Magnitude");
        }
        set
        {
            material.SetFloat("_Magnitude", value);
        }
    }
    Vector3 Axis
    {
        get
        {
            return material.GetVector("_Axis");
        }
        set
        {
            material.SetVector("_Axis", value);
        }
    }

    public bool UseFixedUpdate = false;


    Vector3 prevAxis = Vector3.zero;
    float mag = 0;
    public void UpdateWobble()
    {

        //position Change is global. convert this into local space 
        MovementData data = GetMovementData();
        mag = Mathf.MoveTowards(mag, data._magnitude, Time.deltaTime * (Mathf.Abs(mag) > 1f ? 5f : 0.2f));

        Magnitude = Mathf.Sin(Time.time * 8) * 5 * mag;// * Mathf.MoveTowards(Magnitude, data._magnitude * 8, Time.deltaTime * ((Magnitude > 0.1f)?15f : 2f));
        Debug.Log("MAG: " + Magnitude);

        Axis = Vector3.MoveTowards(Axis, new Vector3(-data._axis.y, data._axis.x, data._axis.z), Time.deltaTime * 3) + prevAxis * 0.001f;
        
        if(Axis.magnitude > 0.001f)
        prevAxis = Axis;

       
    }

    private void Update()
    {
        if (Application.isPlaying == false) return;
        if (UseFixedUpdate) return;
        UpdateWobble();
    }

    private void FixedUpdate()
    {
        if (Application.isPlaying == false) return;
        if (!UseFixedUpdate) return;
        UpdateWobble();
    }

    public abstract MovementData GetMovementData();
}

[System.Serializable]
public struct MovementData
{
    public Vector3 _axis;
    public float _magnitude;

    public MovementData(Vector3 axis, float magnitude)
    {
        _axis = axis;
        _magnitude = magnitude;
    }

    public MovementData(Vector3 velocity)
    {
        _axis = velocity;
        _magnitude = velocity.magnitude;
    }

    public override string ToString()
    {
        return $"Mag: {_magnitude}, Axis: {_axis}";
    }
}

