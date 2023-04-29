using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionChangeWobbler : Wobbler
{

    Vector3 previousPosition = Vector3.zero;
    
    public override MovementData GetMovementData()
    {
        Vector3 direction = transform.position - previousPosition;
        previousPosition = transform.position;

        return new MovementData(direction);
    }
}
