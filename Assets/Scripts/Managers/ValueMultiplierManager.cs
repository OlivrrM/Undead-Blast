using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValueMultiplierManager : MonoBehaviour
{
    public Dictionary<string, object> values = new Dictionary<string, object>();
    public void AddValue(string name, object value)
    {
        if (!values.ContainsKey(name))
        {
            values.Add(name,value);
        }
        else { Debug.LogError("Attempted to add new value with name '" + name + "', however value already existed."); }
    }
    public void UpdateValue(string targetName, object value)
    {
        values[targetName] = value;
    }
}
