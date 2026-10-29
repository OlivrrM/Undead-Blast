using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitGroundBob : MonoBehaviour
{
    public CameraBobManager cameraBobManager;

    public float justGroundedBobAmount;
    public float requiredHitGroundVelocity;
    private void Awake()
    {
        //FixedUpdateQueue.functions.Add(new Function { function = "Listen", instance = this, orderIndex = 5 });
    }
    private void Start()
    {
        cameraBobManager.AddNewBob("HitGroundBob", 15f, 2.5f,true);
    }
    /*
    private void Listen()
    {
        if (FragMovementListener.justGrounded)
        {
            if (Cache.moveData.PreGroundedVelocity.y< requiredHitGroundVelocity){
                cameraBobManager.AddBobForce("HitGroundBob", -(Cache.moveData.PreGroundedVelocity.y * justGroundedBobAmount));
            }
            else
            {
                cameraBobManager.AddBobForce("HitGroundBob", -((Cache.moveData.PreGroundedVelocity.y * justGroundedBobAmount)/5f));
            }
        }
    }
     * */
    void LateUpdate()
    {
        if (FragMovementListener.justGrounded)
        {
            if (Cache.moveData.PreGroundedVelocity.y < requiredHitGroundVelocity)
            {
                cameraBobManager.AddBobForce("HitGroundBob", -(Cache.moveData.PreGroundedVelocity.y * justGroundedBobAmount));
            }
            else
            {
                //cameraBobManager.AddBobForce("HitGroundBob", -((Cache.moveData.PreGroundedVelocity.y * justGroundedBobAmount) / 5f));
            }
        }
    }
}
