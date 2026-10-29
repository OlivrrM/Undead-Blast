using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fragsurf.Movement;
//using TMPro;

public class FragMovementListener : MonoBehaviour
{
    /// Should only contain static booleans
    public LayerMask groundedLayerMask;
    public static bool justGrounded;
    public static bool grounded;
    public static bool running;
    public static bool justJumped;
    public static bool surfing;
    public static bool shitSurfing;
    public static bool shitMovement;
    public static bool movingBackwards;
    public static Vector3 localVelocity;
    public static Vector3 preGroundedVelocity;

    public static float hSpeedPercentage;
    float defaultWalkFactor;

    //public TextMeshProUGUI TEST;
    //public TextMeshProUGUI TEST1;
    private void Awake()
    {
        //FixedUpdateQueue.functions.Add(new Function { function = "Listen", instance = this, orderIndex = 999 });
    }
    private void Start()
    {
        defaultWalkFactor = Cache.moveData.WalkFactor;
    }
    private void FixedUpdate()
    {
        //justGrounded = Cache.moveData.JustGrounded;
        justJumped = Cache.moveData.JustJumped&&Cache.surfCharacter.active;

    }
    /// <summary> DEPRECATED ->
    /// Update is used to run all functions within FIxedUpdateQueue.cs, using variables set from this script within fixed update
    /// </summary> <- DEPRECATED
    private void LateUpdate()
    {
        float walkFactorDivisor = 5.76f * defaultWalkFactor;
        hSpeedPercentage = Mathf.Approximately(walkFactorDivisor, 0f) ? 0f : new Vector3(Cache.moveData.Velocity.x, 0, Cache.moveData.Velocity.z).magnitude / walkFactorDivisor;
        //grounded = (Cache.moveData.Velocity.y > -0.01f && Cache.moveData.Velocity.y < 0.01f) ? true : false;
        //grounded = Physics.Raycast(Cache.surfCharacter.transform.position+Cache.surfCharacter.ViewOffset, Vector3.down, Cache.surfCharacter.ViewOffset.y+0.03f, groundedLayerMask);
        justGrounded = false;
        if (SurfController.grounded){
            if (!grounded) { justGrounded = true; }
        }
        grounded = SurfController.grounded;
        ///Surfing checks need to be fixed
        ///if movedata.surfing is true, set true, then check if (normal == null || grounded), if so then set false
        //surfing = Cache.moveData.SurfNormal.y <= SurfPhysics.SurfSlope&& Cache.moveData.SurfNormal.y>0 && !grounded;// && !SurfController.nullNormal;
        if (Cache.moveData.Surfing) { surfing = true; }
        if (grounded || (Cache.colliderCornerCaster.bottomCollision.amount!=0f && Cache.colliderCornerCaster.bottomCollision.normal.y > SurfPhysics.SurfSlope)||Cache.colliderCornerCaster.bottomCollision.amount==0) { surfing = false; }
        if (!surfing && Cache.moveData.Surfing) { surfing = true; }
        //if (Input.GetKeyDown(KeyCode.F)) { print(surfing + "   " + grounded + "   " + Cache.colliderCornerCaster.bottomCollision.amount + "   " + Cache.colliderCornerCaster.bottomCollision.normal.y); }
        shitSurfing = Tracer.lastHitWasShit && surfing;
        shitMovement = grounded && SurfController.allCastsShit;

        localVelocity = Cache.surfCharacter.transform.InverseTransformDirection(Cache.moveData.Velocity);
        if (localVelocity.z < -0.1f){ movingBackwards = true; }
        else { movingBackwards = false; }
        //print(grounded+"    "+shitMovement);

        //Debug.DrawRay(Cache.surfCharacter.transform.position + Cache.surfCharacter.ViewOffset, Vector3.down * (Cache.surfCharacter.ViewOffset.y+0.03f), Color.cyan);
        running = grounded && new Vector3(Cache.moveData.Velocity.x, 0, Cache.moveData.Velocity.z).magnitude != 0 ? true : false;
        
        if (!grounded) { preGroundedVelocity = Cache.moveData.Velocity; }

        StartCoroutine(EndOfFrame());
    }
    IEnumerator EndOfFrame()
    {
        yield return new WaitForEndOfFrame();
        //justGrounded = false;
        justJumped = false;
    }
}
