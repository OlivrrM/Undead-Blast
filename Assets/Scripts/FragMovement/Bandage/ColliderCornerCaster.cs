using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderCornerCaster : MonoBehaviour
{
    public LayerMask mask;
    public SurfClippingInfo bottomCollision;
    private void Start()
    {
        Cache.colliderCornerCaster = this;
    }
    /*private void Update()
    {
        float x = SurfClippingAmount();
        if (x!=0f && x != 1.83f) { print(x); }
    }*/
    private void Update()
    {
        bottomCollision = SurfClippingAmount(-0.1f);
    }
    public SurfClippingInfo SurfClippingAmount(float yIncrease = 0f)
    {
        SurfClippingInfo surfClippingInfo = new SurfClippingInfo();
        RaycastHit[] hits = new RaycastHit[4];
        RaycastHit hit;
        Debug.DrawRay(new Vector3(Cache.surfCharacter.transform.position.x + (Cache.surfCharacter.ColliderSize.x * 0.5f), Cache.surfCharacter.transform.position.y + Cache.surfCharacter.ColliderSize.y + yIncrease, Cache.surfCharacter.transform.position.z + (Cache.surfCharacter.ColliderSize.z * -0.5f)),Vector3.down* Cache.surfCharacter.ColliderSize.y, Color.red);
        Debug.DrawRay(new Vector3(Cache.surfCharacter.transform.position.x + (Cache.surfCharacter.ColliderSize.x * -0.5f), Cache.surfCharacter.transform.position.y + Cache.surfCharacter.ColliderSize.y + yIncrease, Cache.surfCharacter.transform.position.z + (Cache.surfCharacter.ColliderSize.z * 0.5f)), Vector3.down* Cache.surfCharacter.ColliderSize.y, Color.red);
        Debug.DrawRay(new Vector3(Cache.surfCharacter.transform.position.x + (Cache.surfCharacter.ColliderSize.x * 0.5f), Cache.surfCharacter.transform.position.y + Cache.surfCharacter.ColliderSize.y + yIncrease, Cache.surfCharacter.transform.position.z + (Cache.surfCharacter.ColliderSize.z * 0.5f)), Vector3.down* Cache.surfCharacter.ColliderSize.y, Color.red);
        Debug.DrawRay(new Vector3(Cache.surfCharacter.transform.position.x + (Cache.surfCharacter.ColliderSize.x * -0.5f), Cache.surfCharacter.transform.position.y + Cache.surfCharacter.ColliderSize.y + yIncrease, Cache.surfCharacter.transform.position.z + (Cache.surfCharacter.ColliderSize.z * -0.5f)), Vector3.down* Cache.surfCharacter.ColliderSize.y, Color.red);
        if (
            Physics.Raycast(new Vector3(Cache.surfCharacter.transform.position.x+(Cache.surfCharacter.ColliderSize.x*0.5f),Cache.surfCharacter.transform.position.y+ Cache.surfCharacter.ColliderSize.y + yIncrease, Cache.surfCharacter.transform.position.z + (Cache.surfCharacter.ColliderSize.z * -0.5f)),Vector3.down,out hits[0],Cache.surfCharacter.ColliderSize.y,mask)||
            Physics.Raycast(new Vector3(Cache.surfCharacter.transform.position.x+(Cache.surfCharacter.ColliderSize.x*-0.5f),Cache.surfCharacter.transform.position.y+ Cache.surfCharacter.ColliderSize.y + yIncrease, Cache.surfCharacter.transform.position.z + (Cache.surfCharacter.ColliderSize.z * 0.5f)),Vector3.down, out hits[1], Cache.surfCharacter.ColliderSize.y,mask) ||
            Physics.Raycast(new Vector3(Cache.surfCharacter.transform.position.x+(Cache.surfCharacter.ColliderSize.x*0.5f),Cache.surfCharacter.transform.position.y+ Cache.surfCharacter.ColliderSize.y + yIncrease, Cache.surfCharacter.transform.position.z + (Cache.surfCharacter.ColliderSize.z * 0.5f)),Vector3.down, out hits[2], Cache.surfCharacter.ColliderSize.y,mask) ||
            Physics.Raycast(new Vector3(Cache.surfCharacter.transform.position.x+(Cache.surfCharacter.ColliderSize.x*-0.5f),Cache.surfCharacter.transform.position.y+ Cache.surfCharacter.ColliderSize.y + yIncrease, Cache.surfCharacter.transform.position.z + (Cache.surfCharacter.ColliderSize.z * -0.5f)),Vector3.down, out hits[3], Cache.surfCharacter.ColliderSize.y,mask)
            ) 
        {
            float highest = -999;
            for (int i = 0; i < hits.Length; i++){
                if (hits[i].distance == 0) { hits[i].distance = (Cache.surfCharacter.ColliderSize.y + yIncrease); }
                if (((Cache.surfCharacter.ColliderSize.y + yIncrease) -hits[i].distance) > highest) {
                    highest = (Cache.surfCharacter.ColliderSize.y + yIncrease) - hits[i].distance;
                    surfClippingInfo.amount = highest;
                }
                if (hits[i].normal != Vector3.zero) { surfClippingInfo.normal = hits[i].normal; }
            }
        }
        return surfClippingInfo;
    }
}
