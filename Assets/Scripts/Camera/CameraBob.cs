using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBob
{
    public float force = 10f; //Target
    public float pointer = 1f; //Target pointer
    public float smoothness = 1f; //Target pointer speed
    public float drag = 1f; //Target decay speed

    public bool independant = false; //Determines if the camera bob will move anchored graphics as well
}
