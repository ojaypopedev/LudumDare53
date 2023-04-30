using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class SmoothLineRenderer : MonoBehaviour
{
    public List<Transform> transforms;
    public List<Vector3> points => transforms.Select(t => t.position).ToList();
    public float lineThickness = 0.1f;  
    public int resolution = 10;         

    private LineRenderer lineRenderer; 

    // Use this for initialization
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.startWidth = lineThickness;
        lineRenderer.endWidth = lineThickness;
        lineRenderer.positionCount = resolution * (points.Count - 1) + 1;
     
    }

    private void Update()
    {
        DrawSmoothLine();
    }

    // Draw a smooth line using Catmull-Rom splines
    private void DrawSmoothLine()
    {
        int index = 0;
        for (int i = 0; i < points.Count - 1; i++)
        {
            for (int j = 0; j < resolution; j++)
            {
                float t = (float)j / resolution;
                Vector3 point = CatmullRomSpline(points[i > 0 ? i - 1 : i], points[i], points[i + 1], points[Mathf.Min(i + 2, points.Count - 1)], t);
                lineRenderer.SetPosition(index, point);
                index++;
            }
        }

        // Add the last point
        lineRenderer.SetPosition(index, points[points.Count - 1]);
    }

    // Compute a point on a Catmull-Rom spline
    private Vector3 CatmullRomSpline(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        float tt = t * t;
        float ttt = tt * t;
        Vector3 b1 = 0.5f * (p2 - p0);
        Vector3 b2 = 0.5f * (p3 - p1);
        Vector3 p = (2 * p1 - 2 * p2 + b1 + b2) * ttt + (-3 * p1 + 3 * p2 - 2 * b1 - b2) * tt + b1 * t + p1;
        return p;
    }
}
