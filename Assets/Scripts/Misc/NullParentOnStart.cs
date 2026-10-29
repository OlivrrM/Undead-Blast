using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NullParentOnStart : MonoBehaviour
{
    public bool preserveScale;
    Vector3 originalScale;
    private void Awake(){
        originalScale = transform.localScale;
        transform.SetParent(null);
        if (preserveScale) { transform.localScale = originalScale; }
    }
}
