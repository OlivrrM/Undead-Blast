using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneTargetRotation : MonoBehaviour
{
    public Transform target;
    private void LateUpdate()
    {
        transform.rotation = target.rotation;
    }
}
