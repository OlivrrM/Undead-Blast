using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpBob : MonoBehaviour
{
    public CameraBobManager cameraBobManager;

    public Vector2 jumpBobAmountAB;
    private void Awake()
    {
        //FixedUpdateQueue.functions.Add(new Function { function = "Check", instance = this, orderIndex = 3 });
    }
    private void Start()
    {
        cameraBobManager.AddNewBob("JumpBob", 15f, 2f);
    }
    public void Check()
    {
        ///Use LateUpdate to seperate thingy behind yield WaitTillEndOfFrame
        /*if (FragMovementListener.justJumped){
            cameraBobManager.AddBobForce("JumpBob", Random.RandomRange(jumpBobAmountAB.x*(Cache.surfCharacter.MoveConfig.JumpPower/ Cache.jumpHeightManagerScript.defaultJumpHeight), jumpBobAmountAB.y* (Cache.surfCharacter.MoveConfig.JumpPower / Cache.jumpHeightManagerScript.defaultJumpHeight)));
        }*/
    }
    void LateUpdate()
    {
        if (FragMovementListener.justJumped)
        {
            cameraBobManager.AddBobForce("JumpBob", Random.RandomRange(jumpBobAmountAB.x * (Cache.surfCharacter.MoveConfig.JumpPower / Cache.jumpHeightManager.defaultJumpHeight), jumpBobAmountAB.y * (Cache.surfCharacter.MoveConfig.JumpPower / Cache.jumpHeightManager.defaultJumpHeight)));
        }
    }
}
