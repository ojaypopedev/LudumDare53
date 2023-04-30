using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonArcShooter : MonoBehaviour
{
    [SerializeField] float input_sensitivity = 90f;

    [SerializeField] float head_min_y_rotation, head_max_y_rotation;
    float head_y_rotation;

    [SerializeField] Camera head;

    private void Start()
    {
        head_y_rotation = head.transform.localRotation.eulerAngles.y;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, input_sensitivity *  Input.GetAxis("Mouse X"), 0);

        head_y_rotation = Mathf.Clamp(head_y_rotation - Input.GetAxis("Mouse Y") * input_sensitivity, head_min_y_rotation, head_max_y_rotation);
        head.transform.localRotation = Quaternion.Euler(head_y_rotation,0, 0);
    }
}
