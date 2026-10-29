using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandheldBob : MonoBehaviour
{
    public LocalRotationOffsetManager localRotationOffsetManager;

    public float hitGroundBobAmount;
    float targetHitGroundBob;
    float currentHitGroundBob;
    public float hitGroundBobSmoothness;
    public float hitGroundBobSpeed;
    private void Start()
    {
        localRotationOffsetManager.AddValue("HitGroundHandheldBob", Vector3.zero);
    }
    private void Update()
    {
        currentHitGroundBob = Mathf.Lerp(currentHitGroundBob, targetHitGroundBob, Time.deltaTime * hitGroundBobSmoothness);
        targetHitGroundBob = Mathf.Lerp(targetHitGroundBob, 0, Time.deltaTime * hitGroundBobSpeed);

        localRotationOffsetManager.UpdateValue("HitGroundHandheldBob", new Vector3(currentHitGroundBob, 0, 0));
    }
    private void LateUpdate()
    {
        if (FragMovementListener.justGrounded)
        {
            targetHitGroundBob = -Cache.moveData.PreGroundedVelocity.y * (Cache.moveData.PreGroundedVelocity.y > -10f ? hitGroundBobAmount / 2f : hitGroundBobAmount);
        }
    }
}
