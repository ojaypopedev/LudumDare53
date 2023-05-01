using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunningTrack : MonoBehaviour
{
   [SerializeField] Transform[] points = null;
    private void OnDrawGizmos()
    {
        for (int i = 0; i < points.Length; i++)
        {
            Gizmos.DrawLine(points[i].position, points[i + 1].position);
        }
        Gizmos.DrawLine(points[0].position, points[points.Length - 1].position);
    }

    public Vector3 GetPoint(int index)
    {
        return points[index % points.Length].position;
    }

}
