using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public static Dictionary<string, bool> activators = new Dictionary<string, bool>();
    public static void AddActivator(string name, bool state)
    {
        if (activators.ContainsKey(name)){
            Debug.LogError("Tried to add cursor activator '"+ name + "', however activator already exists");
        }
        else{
            activators.Add(name, state);
        }
    }
    public static void UpdateActivator(string name, bool state)
    {
        activators[name] = state;
        RefreshCursorState();
    }
    public static void RefreshCursorState()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        foreach (KeyValuePair<string,bool> activator in activators)
        {
            if (activator.Value)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                break;
            }
        }
    }
}
