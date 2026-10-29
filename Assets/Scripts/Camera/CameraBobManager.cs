using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBobManager : MonoBehaviour
{
    Dictionary<string, CameraBob> bobs = new Dictionary<string, CameraBob>();

    CameraRotationOffsetManager cameraRotationOffsetManager;
    LocalRotationOffsetManager anchoredGraphicsRotationOffsetManager;
    private void Awake()
    {
        Cache.camBobManager = this;
    }
    private void Start()
    {
        cameraRotationOffsetManager = gameObject.GetComponent<CameraRotationOffsetManager>();
        anchoredGraphicsRotationOffsetManager = GameObject.Find("PlayerGraphicsParentOffset").GetComponent<LocalRotationOffsetManager>();

        cameraRotationOffsetManager.AddOffset("CamBob", new CameraRotationOffset { offset = Vector4.zero });
        if (anchoredGraphicsRotationOffsetManager != null) { anchoredGraphicsRotationOffsetManager.AddValue("CamBobIndepentant", Vector3.zero); }
    }
    public void AddNewBob(string name, float smoothness, float drag, bool independant = false)
    {
        if (!bobs.ContainsKey(name)){
            bobs.Add(name, new CameraBob { force = 0f, smoothness = smoothness, drag = drag, pointer = 0f, independant = independant });
        }
        else { Debug.LogError("Attempted to add new bob with name '" + name + "', however bob already existed."); }
    }
    public void AddBobForce(string targetBob, float amount)
    {
        bobs[targetBob].force += amount;
    }
    private void Update()
    {
        float bobX = 0f;
        float independantBobX = 0f;
        foreach (KeyValuePair<string,CameraBob> bob in bobs){
            bob.Value.force = Mathf.Lerp(bob.Value.force, 0f, Time.deltaTime * bob.Value.drag);
            bob.Value.pointer = Mathf.Lerp(bob.Value.pointer, bob.Value.force, Time.deltaTime * bob.Value.smoothness);
            bobX += bob.Value.pointer;
            if (bob.Value.independant) { independantBobX += bob.Value.pointer; }
        }
        cameraRotationOffsetManager.UpdateOffset("CamBob", new CameraRotationOffset { offset = new Vector4(bobX, 0, 0, 0) });
        if (anchoredGraphicsRotationOffsetManager != null) { anchoredGraphicsRotationOffsetManager.UpdateValue("CamBobIndepentant", new Vector3(-independantBobX, 0, 0)); }
    }
}
