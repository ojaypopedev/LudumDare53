using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcCalculator2 : MonoBehaviour
{
    [SerializeField] Transform arcStartPoint;
    [SerializeField] Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;




        if(Physics.Raycast(ray, out hit))
        {
            Vector3 Raydirection = hit.point - arcStartPoint.position;
            Vector3 Cross = Vector3.Cross(Vector3.up, Raydirection).normalized;
            float distance = Vector3.Distance(hit.point, arcStartPoint.position);
            Vector3 prevPoint = Vector3.zero;



            Vector3 firstPoint = Vector3.zero, secondPoint = Vector3.zero;
            for (float i = 0; i < 1000; i++)
            {
                float delta = (i) / 1000;
                
             

                Vector3 pointAlongLine = Vector3.Lerp(arcStartPoint.position, hit.point, delta);
                pointAlongLine += Vector3.up * (delta * delta - delta) * -(0.0002f * Mathf.Pow(distance, 3));//Mathf.Clamp( (distance/2),0,10);
                pointAlongLine += Cross * (delta * delta - delta) * -(0.0002f * Mathf.Pow(distance, 3)); //;- Mathf.Clamp((distance/2),0,8);

                if (i == 0)
                {
                    firstPoint = pointAlongLine;
                }
                if(i == 10)
                {
                    secondPoint = pointAlongLine;
                }

                if (prevPoint != Vector3.zero)
                {
                    Gizmos.DrawLine(prevPoint, pointAlongLine);
                }

                prevPoint = pointAlongLine;


            }


            Vector3 direction = secondPoint - firstPoint;
            Gizmos.color = Color.red;
            Gizmos.DrawLine(arcStartPoint.position, arcStartPoint.position + direction*10);

            direction = direction.normalized;
            arcStartPoint.transform.forward = direction;


        }
        else
        {
            

            Gizmos.DrawLine(arcStartPoint.position, arcStartPoint.position + cam.transform.forward * 20);

             arcStartPoint.transform.forward = (arcStartPoint.position + cam.transform.forward * 20) - arcStartPoint.position;
        }
    }
}
