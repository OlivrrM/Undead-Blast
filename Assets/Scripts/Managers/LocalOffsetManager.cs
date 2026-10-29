using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalOffsetManager : ValueMultiplierManager
{
    Vector3 defaultOffset;
    private void Start()
    {
        defaultOffset = transform.localPosition;
    }
    private void Update()
    {
        transform.localPosition = defaultOffset;
        defaultOffset = transform.localPosition;
        foreach (KeyValuePair<string, object> value in values){
            transform.localPosition += (Vector3)value.Value;
        }
    }
}
