using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandController : MonoBehaviour
{
    public Transform pivotPoint;

    public Vector3 ForwardAxis = new Vector3(0,0,1);

    public float moveSensitivity;
    public float rotSensitivity;

    public Vector2 minMaxForward;
    public Vector2 minMaxLeft;
    public Vector2 minMaxUp;

    float currentLeft;
    float currentForward;

    float currentRot;

    Vector3 startPos;

    private void Awake()
    {
        startPos = pivotPoint.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 inputDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        inputDelta = inputDelta * moveSensitivity;

        currentLeft += inputDelta.x;
        currentForward += inputDelta.y;

        currentLeft = Mathf.Clamp(currentLeft, -1, 1);
        currentForward = Mathf.Clamp(currentForward, -1, 1);

        var localxPos = Mathf.Lerp(minMaxLeft.x, minMaxLeft.y, Mathf.InverseLerp(-1, 1, currentLeft));
        var localzPos = Mathf.Lerp(minMaxForward.x, minMaxForward.y, Mathf.InverseLerp(-1, 1, currentForward));
        var localyPos = Mathf.Lerp(minMaxUp.x, minMaxUp.y, Mathf.InverseLerp(-1,1, currentForward));


        var rotDirection = 0;
        if (currentLeft <= -1) rotDirection = -1;
        if (currentLeft >= 1) rotDirection = 1;

        currentRot += rotDirection * rotSensitivity * Time.deltaTime;

        transform.rotation = Quaternion.Euler(0, currentRot, 0);

        var forwardDot = Vector3.Dot(Vector3.forward, transform.forward);

        float moveAxisDT = Mathf.Clamp01(Mathf.InverseLerp(0, -1, forwardDot));

        var axis = Vector3.Lerp(Vector3.forward, Vector3.up, moveAxisDT);

        var moveAmount = Mathf.Lerp(localzPos, localyPos, moveAxisDT);

        var forwardMove = axis * moveAmount;

        Debug.Log(transform.eulerAngles.y + "  " + moveAxisDT);

        pivotPoint.transform.localPosition = startPos + (new Vector3(localxPos, 0, 0) + forwardMove);

    }
}
