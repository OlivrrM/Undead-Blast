using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandheldSway : MonoBehaviour
{
    public float swayMultiplier = 1.0f;
    public float smooth = 5.0f;

    private Quaternion currentVelocity = Quaternion.identity;

    private void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * swayMultiplier;
        float mouseY = Input.GetAxis("Mouse Y") * swayMultiplier;

        Quaternion rotationX = Quaternion.AngleAxis(-mouseY, Vector3.right);
        Quaternion rotationY = Quaternion.AngleAxis(mouseX, Vector3.up);
        Quaternion targetRotation = rotationX * rotationY;

        transform.localRotation = Utilities.SmoothDamp(transform.localRotation, targetRotation, ref currentVelocity, 1f / smooth, Time.deltaTime);
    }
}