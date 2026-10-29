using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpHeightManager : ValueMultiplierManager
{
    [HideInInspector] public float defaultJumpHeight;
    private void Start()
    {
        defaultJumpHeight = Cache.surfCharacter.MoveConfig.JumpPower;
        Cache.jumpHeightManager = this;
    }
    private void Update()
    {
        if (defaultJumpHeight == 0) { defaultJumpHeight = Cache.surfCharacter.MoveConfig.JumpPower; }
        Cache.surfCharacter.MoveConfig.JumpPower = defaultJumpHeight;
        foreach (KeyValuePair<string, object> value in values){
            Cache.surfCharacter.MoveConfig.JumpPower *= System.Convert.ToSingle(value.Value);
        }
    }
}
