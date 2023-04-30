using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine.UIElements;
using System.Threading.Tasks;

public class ArcCalculator2 : MonoBehaviour
{
    [SerializeField] Transform arcStartPoint;
    [SerializeField] Camera cam;

    [SerializeField] LineRenderer renderer;
    public LayerMask mask;

    [SerializeField] GameObject tempMover;
    [SerializeField] float distance = 0;
    [SerializeField] Transform CrossHair;

    [SerializeField] ParticleSystem ps;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ArcData data = new ArcData(arcStartPoint.position, cam, 100, mask);
       
        arcStartPoint.forward = data.StartDirection;
       
        renderer.positionCount = data.PositionCount;
        renderer.SetPositions(data.Positions);

        CrossHair.transform.position = data.endPosition;

        if(data.HitTarget)
        {
            CrossHair.transform.forward = data.HitNormal;
        }
        else
        {
            CrossHair.transform.forward = cam.transform.forward;
        }
        if(Input.GetMouseButtonDown(0))
        {
            ps.Play();
            GameObject newMover = Instantiate(tempMover);
            ArcData.ShootAlongArc(data, newMover, 40, () => {

                if(newMover.GetComponent<Droplet>())
                {
                    newMover.GetComponent<Droplet>().Collide(data.HitNormal);
                }
                else
                {
                 Destroy(newMover);

                }

            });
        }


    }



    private void OnDrawGizmos()
    {
        //Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        //RaycastHit hit;

        //List<Vector3> points = new List<Vector3>();


        //if (Physics.Raycast(ray, out hit))
        //{
        //    Vector3 Raydirection = hit.point - arcStartPoint.position;
        //    Vector3 Cross = Vector3.Cross(Vector3.up, Raydirection).normalized;
        //    float distance = Vector3.Distance(hit.point, arcStartPoint.position);
        //    Vector3 prevPoint = Vector3.zero;


        //    Vector3 firstPoint = Vector3.zero, secondPoint = Vector3.zero;
        //    for (float i = 0; i < 200; i++)
        //    {
        //        float delta = (i) / 200;
             
        //        Vector3 pointAlongLine = Vector3.Lerp(arcStartPoint.position, hit.point, delta);
        //        pointAlongLine += Vector3.up * (delta * delta - delta) * -(0.0002f * Mathf.Pow(distance, 3));//Mathf.Clamp( (distance/2),0,10);
        //        pointAlongLine += Cross * (delta * delta - delta) * -(0.0002f * Mathf.Pow(distance, 3)); //;- Mathf.Clamp((distance/2),0,8);

        //        if (i == 0)
        //        {
        //            firstPoint = pointAlongLine;
        //        }
        //        if(i == 10)
        //        {
        //            secondPoint = pointAlongLine;
        //        }

        //        if (prevPoint != Vector3.zero)
        //        {
        //            Gizmos.DrawLine(prevPoint, pointAlongLine);
        //        }

        //        prevPoint = pointAlongLine;

        //        points.Add(pointAlongLine);

        //    }



        //    Vector3 direction = secondPoint - firstPoint;
        //    Gizmos.color = Color.red;
        //    Gizmos.DrawLine(arcStartPoint.position, arcStartPoint.position + direction*10);

        //    direction = direction.normalized;
        //    arcStartPoint.transform.forward = direction;


        //}
        //else
        //{

        //    points.Add(arcStartPoint.position);
        //    points.Add(arcStartPoint.position + cam.transform.forward * 20);
        //    Gizmos.DrawLine(arcStartPoint.position, arcStartPoint.position + cam.transform.forward * 20);

        //     arcStartPoint.transform.forward = (arcStartPoint.position + cam.transform.forward * 20) - arcStartPoint.position;
        //}

        //renderer.positionCount = points.Count;
        //renderer.SetPositions(points.ToArray());
    }
}


public struct ArcData
{
    private Vector3[] _positions;
    private Vector3 _hitNormal;
    private Collider _hitCollider;//null;

    public int PositionCount => _positions.Length;
    public Vector3 startPosition => _positions[0];
    public Vector3 endPosition => _positions[_positions.Length - 1];
    public Vector3 StartDirection => (_positions[1] - _positions[0]).normalized;
    public bool HitTarget => _hitCollider != null;
    public Vector3[] Positions => _positions;
    public Collider HitCollider => _hitCollider;
    public Vector3 HitNormal => _hitNormal;

    public Vector3 GetPointByDistance(float distance)
    {
        
        
        float totalDistance = 0;
        int index = 0;

        float previousDistance = 0;

        while(totalDistance < distance && index < PositionCount-1)
        {
            previousDistance = totalDistance;
            totalDistance += Vector3.Distance(Positions[index], Positions[index + 1]);
            index++;
        }

        float delta = (distance - previousDistance) / (totalDistance - previousDistance);
        //Debug.Log("DELTA: " + delta);
        return Vector3.Lerp(Positions[index - 1], Positions[index], delta);
    }

    public float TotalDistance
    {
        get
        {
            float distance = 0;
            for (int i = 0; i < PositionCount-1; i++)
            {
                distance += Vector3.Distance(Positions[i], Positions[i + 1]);
            }
            return distance;
        }
    }

    public static float ArcMultiplier = 0.006f;
    public static float ArcPower = 2.5f;

    public ArcData(Vector3 startPoint, Camera camera, float maxDistance = 100, int layerMask = -1, int points = 200)
    {
        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(new Vector3(camera.pixelWidth/2, camera.pixelHeight/2));

        if (Physics.Raycast(ray, out hit, maxDistance, layerMask))
        {

            Debug.Log("Hit");
            _hitCollider = hit.collider;

            float distance = Vector2.Distance(startPoint, hit.point);
            Vector3 direction = hit.point - startPoint;
            Vector3 Cross = Vector3.Cross(Vector3.up, direction).normalized;

            Vector3 arcDirection = (Vector3.up + Cross/4) * -(ArcMultiplier * Mathf.Pow(distance, ArcPower));

            _positions = Enumerable.Range(0, points).ToList().Select(e => Vector3.Lerp(startPoint, hit.point, GetDelta(e, points)) + arcDirection * GetDeltaArc(e, points)).ToArray();

            float GetDelta(int x, int max)
            {
                return (float)x / (float)max;
            }
            float GetDeltaArc(int x, int max)
            {
                float delta = GetDelta(x, max);
                return delta * delta - delta;
            }

            _hitNormal = hit.normal;

        }
        else
        {
            _hitCollider = null;
            _positions = new Vector3[] { startPoint, ray.GetPoint(maxDistance) };
            _hitNormal = Vector3.zero;
        }
    }


    public static void ShootAlongArc(ArcData data, GameObject gameObject, float speed, System.Action OnReachedEnd)
    {
        Task t = _shootAlongArcAsync(data, gameObject, speed, OnReachedEnd);
    }

    private static async Task _shootAlongArcAsync(ArcData data, GameObject obj, float speed, System.Action OnReachedEnd, System.Action OnFrame = null)
    {

        float distance = 0;
        while(distance < data.TotalDistance)
        {
            distance += speed * Time.deltaTime;
            obj.transform.position = data.GetPointByDistance(distance);
            OnFrame?.Invoke();
            await Task.Yield();
        }

        OnReachedEnd?.Invoke();

    }

}
