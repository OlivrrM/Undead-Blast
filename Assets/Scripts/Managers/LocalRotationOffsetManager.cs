using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalRotationOffsetManager : ValueMultiplierManager
{
    Vector3 defaultOffset;
    private void Start()
    {
        defaultOffset = transform.localEulerAngles;
    }
    private void Update()
    {
        transform.localEulerAngles = defaultOffset;
        defaultOffset = transform.localEulerAngles;
        foreach (KeyValuePair<string, object> value in values)
        {
            transform.localEulerAngles += (Vector3)value.Value;
        }
    }
}
