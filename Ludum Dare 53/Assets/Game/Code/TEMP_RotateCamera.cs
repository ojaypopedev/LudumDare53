using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEMP_RotateCamera : MonoBehaviour
{

    public Collider currentSelected = null;

    [SerializeField] ArcCalculator calculator;
    [SerializeField] Transform camera;

    public Material Selected, Deselected;

    public GameObject hotDogPrefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    float yPos = 0;    // Update is called once per frame
    void Update()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        transform.Rotate(0, 2 * Input.GetAxis("Mouse X"), 0);
        yPos += Input.GetAxis("Mouse Y") / 5;
        yPos = Mathf.Clamp(yPos, 0, 1);
        calculator.SetForce(Mathf.Lerp(0, 20, yPos));

        camera.localRotation = Quaternion.Euler(Mathf.Lerp(5, -5, yPos),90, 0);

        Collider collider = calculator.GetLocalArcData().HitCollider;

        if(Input.GetMouseButtonDown(0))
        {
            GameObject instance = Instantiate(hotDogPrefab);

            calculator.ThrowOnArc(instance, calculator.GetLocalArcData(), 10, (data) => { 
            
                if(data.HitCollider)
                {
                    if(data.HitCollider.tag == "Crowd")
                    {
                        data.HitCollider.transform.localScale = Vector3.zero;
                    }
                }

                Destroy(instance.gameObject);
                
            });
        }

        if (collider != null)
        {
            if(collider.tag == "Crowd")
            {
                if (currentSelected != null && currentSelected != collider)
                {
                    currentSelected.GetComponent<Renderer>().material = Deselected;
                }

                currentSelected = collider;
                currentSelected.GetComponent<Renderer>().material = Selected;
            }
            else
            {
                if (currentSelected != null)
                {
                    currentSelected.GetComponent<Renderer>().material = Deselected;
                }
                currentSelected = null;
            }
            
        }else
        {
            if (currentSelected != null)
            {
                currentSelected.GetComponent<Renderer>().material = Deselected;
            }
            currentSelected = null;
        }
    }
}
