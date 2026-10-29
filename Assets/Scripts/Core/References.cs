using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class References : MonoBehaviour {

    public Dictionary<string, GameObject> bulletHoles = new Dictionary<string, GameObject>();
    public List<string> bulletHoles_KEYS = new List<string>();
    public List<GameObject> bulletHoles_VALUES = new List<GameObject>();

    void Awake()
    {
        InitReferences();
        Cache.references = this;
    }
    void InitReferences()
    {
        for (int i = 0; i < bulletHoles_KEYS.Count; i++){
            bulletHoles.Add(bulletHoles_KEYS[i],bulletHoles_VALUES[i]);
        }
    }
}
