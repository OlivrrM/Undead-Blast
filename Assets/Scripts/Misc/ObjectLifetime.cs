using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectLifetime : MonoBehaviour
{
    public float lifetime;
    float currentTime;
    private void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime >= lifetime) Destroy(gameObject);
    }
}
