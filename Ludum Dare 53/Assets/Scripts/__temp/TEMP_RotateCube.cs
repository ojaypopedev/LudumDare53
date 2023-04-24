using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEMP_RotateCube : MonoBehaviour
{
   
    void Update() => transform.Rotate(0, 90 * Time.deltaTime, 0, Space.World);
   
}
