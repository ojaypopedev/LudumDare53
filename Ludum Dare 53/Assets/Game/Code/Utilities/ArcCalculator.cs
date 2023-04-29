using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class ArcCalculator : MonoBehaviour
{
   
    public LayerMask raycastMask;

     float force = 10;
    float height = 15;

    public void SetForce(float force)
    {
        this.force = force;
    }

    public void SetHeight(float height)
    {
        this.height = height;
    }

    public float GetForce() => force;
   
    public ArcData GetArcData(Vector3 startPosition, Vector3 startVelocity, float timeStep = 0.02f, int maxPoints = 200)
    {
        List<Vector3> arcPoints = new List<Vector3>();
        ArcData newArcData = new ArcData();
        Vector3 position = startPosition;
        Vector3 velocity = startVelocity;
        Vector3 hitNormal = Vector3.zero;
        Vector3 prevPosition = Vector3.zero;
        float totalTime = 0;
        for (int i = 0; i < maxPoints; i++)
        {
            totalTime += timeStep;
            arcPoints.Add(position);

            prevPosition = position;
            position = startPosition + velocity * totalTime;

            velocity = startVelocity + Physics.gravity * (totalTime) * (totalTime);

            Ray ray = new Ray(position, position - prevPosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, velocity.magnitude / 2, raycastMask))
            {
                arcPoints.Add(hit.point);
                newArcData.HitCollider = hit.collider;
                newArcData.HitNormal = hit.normal;
                
                break;
            }

         

        }

        newArcData.positions = arcPoints.ToArray();
        return newArcData;
    }
    public void GizmoArc(ArcData data, Color? color = null)
    {
        if (color.HasValue)
        {
            Gizmos.color = color.Value;
        }

        for (int i = 1; i < data.positions.Length; i++)
        {
            Gizmos.DrawLine(data.positions[i], data.positions[i - 1]);
        }

        Gizmos.DrawSphere(data.positions[data.positions.Length - 1], 0.5f);
    }

    int cachedLocalArcDataFrame = 0;
    ArcData cachedLocalArcData;

    public ArcData GetLocalArcData()
    {
        if(Time.frameCount > cachedLocalArcDataFrame || cachedLocalArcData.positions == null || cachedLocalArcData.positions.Length ==0)
        {
            cachedLocalArcData = GetArcData(transform.position, transform.TransformDirection(new Vector3(0, height, force)), 0.1f, 200);
            cachedLocalArcDataFrame = Time.frameCount;
        }

        return cachedLocalArcData;
    }
    public void ThrowOnArc(GameObject obj, ArcData data, float speed, System.Action<ArcData> OnReachedEndOfArc = null)
    {
        Task t = MoveAlongArc(obj, data, speed, OnReachedEndOfArc);
    }
    
    async Task MoveAlongArc(GameObject obj, ArcData data, float speed, System.Action<ArcData> OnReachedEndOfArk = null)
    {
        float distance = 0;
        while(data.IsPastEndOfLine(distance) == false)
        {
            distance += Time.deltaTime * speed;
            obj.transform.position = data.GetPointAlongLine(distance);
            await Task.Yield();
        }

        OnReachedEndOfArk?.Invoke(data);
    }

    private void OnDrawGizmos()
    {
        GizmoArc(GetLocalArcData());
    }
}

[System.Serializable]
public struct ArcData
{
    public Vector3[] positions;

    public Vector3 HitNormal;

    public Collider HitCollider;

    public bool IsPastEndOfLine(float horizontalDistanceTravelled)
    {
        if (HorizontalDistance(positions[0], positions[positions.Length - 1]) < horizontalDistanceTravelled)
        {
            return true;
        }
        return false;
    }
    public Vector3 GetPointAlongLine(float horizontalDistanceTravelled)
    {
        if (HorizontalDistance(positions[0], positions[positions.Length - 1]) < horizontalDistanceTravelled)
        {
            return positions[positions.Length - 1];
        }

        int index = 0;

        while(HorizontalDistance(positions[0], positions[index])< horizontalDistanceTravelled)
        {
            index++;
        }

        float distanceA = HorizontalDistance(positions[0], positions[index-1]);// Vector3.Distance(new Vector3(positions[0].x, 0, positions[0].z), new Vector3(positions[index - 1].x, 0, positions[index - 1].z));
        float distanceB = HorizontalDistance(positions[0], positions[index]); //Vector3.Distance(new Vector3(positions[0].x, 0, positions[0].z), new Vector3(positions[index].x, 0, positions[index].z));

        float delta = (horizontalDistanceTravelled - distanceA) / (distanceB- distanceA);
        Debug.Log("DELTA: " + delta);
        return Vector3.Lerp(positions[index - 1], positions[index], delta);
    }

    public static float HorizontalDistance(Vector3 positionA, Vector3 positionB)
    {
        return Vector3.Distance(positionA, positionB);
    //    return //Vector3.Distance(new Vector3(positionA.x, 0, positionA.z), new Vector3(positionB.x, 0 ,positionB.z));
    }
    
    
}
