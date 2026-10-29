using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuzzleFlashSmoke : MonoBehaviour
{
    public float speed;
    private void Update()
    {
        if (Firearm.currentMuzzleFlashPos != null) { transform.position = Vector3.Lerp(transform.position, Firearm.currentMuzzleFlashPos.position, Time.deltaTime * speed); }
    }
}
