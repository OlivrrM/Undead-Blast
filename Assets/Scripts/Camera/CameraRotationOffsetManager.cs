using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fragsurf.Movement;

public class CameraRotationOffsetManager : MonoBehaviour
{
    Dictionary<string, CameraRotationOffset> offsets = new Dictionary<string, CameraRotationOffset>();

    [HideInInspector] public bool forcedOffsetsOnly;

    //[HideInInspector] public CinemachineCameraRotationFixer cinemachineCameraRotationFixer;
    [HideInInspector] public SurfCharacter surfCharacter;
    public void Start()
    {
        //cinemachineCameraRotationFixer = gameObject.GetComponent<CinemachineCameraRotationFixer>();
        //surfCharacter = Cache.surfCharacter;
        Cache.cameraRotationOffsetManager = this;
    }
    public void AddOffset(string name, CameraRotationOffset offset)
    {
        if (!offsets.ContainsKey(name)){
            offsets.Add(name, offset);
        }
        else { Debug.LogError("Attempted to add new camera rotation offset with name '" + name + "', however offset already existed."); }
    }
    public void UpdateOffset(string name, CameraRotationOffset offset)
    {
        offsets[name] = offset;
    }
    public void ResetOffset(string offsetName)
    {
        foreach (KeyValuePair<string, CameraRotationOffset> offset in offsets){
            if (offset.Key== offsetName){
                offsets[offsetName].offset = Vector4.zero;
                break;
            }
        }
    }
    private void Update()
    {
        //cinemachineCameraRotationFixer.m_Offset = Vector3.zero;
        Cache.surfCharacter.CamAngleFixedOffset = Vector3.zero;
        Cache.surfCharacter.camZRotation = 0f;
        foreach (KeyValuePair<string, CameraRotationOffset> offset in offsets){
            if (forcedOffsetsOnly) { if (!offset.Value.forceBob) { continue; } }
            //cinemachineCameraRotationFixer.m_Offset += new Vector3(offset.Value.offset.x, offset.Value.offset.y, offset.Value.offset.z);
            Cache.surfCharacter.CamAngleFixedOffset += new Vector3(offset.Value.offset.x, offset.Value.offset.y, offset.Value.offset.z);
            Cache.surfCharacter.camZRotation += offset.Value.offset.w;
        }
    }
}
