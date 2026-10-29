using UnityEngine;

namespace Fragsurf.Movement
{

    public enum MoveType
    {
        None,
        Noclip,
        Ladder,
        Walk,
        Swim
    }

    public enum MoveStyle
    {
        FW,
        HSW,
        SW,
        BWHSW,
        BW
    }

    public class MoveData
    {
        public int InputTick = 0;
        public Vector3 Origin;
        public Vector3 ViewAngles;
        public Vector3 Velocity;
        public Vector3 BaseVelocity;
        public Vector3 LadderNormal;
        public Vector3 PreGroundedVelocity;
        public Vector3 PreviousOrigin;
        public MoveStyle Style;
        public InputActions Buttons;
        public InputActions OldButtons;
        public float ForwardMove;
        public float SideMove;
        public float UpMove;
        public float SurfaceFriction = 1f;
        public float GravityFactor = 1f;
        public float WalkFactor = 2f;
        public bool Ducked;
        public bool Momentum;
        public bool LimitedExecution;
        public bool JustJumped;
        public bool JustGrounded;
        public bool Surfing;
        public Vector3 SurfNormal;
        public Vector3 GroundNormal;
        public Vector3 PreJumpGroundNormal;
        public bool Sliding;
        public float WaterDepth;
        public float WaterJumpTime;
        public Vector3 MomentumModifier;
        public int GroundTest;
        public float GainCoefficient;
        public Vector3 RawLookDirection;
        public bool canJump = true;

        // Unity 5.4.1f1's compiler doesn't support expression-bodied members (=>), added in C# 6.
        public bool InWater { get { return WaterDepth > 0f; } }
        public Vector3 AbsVelocity { get { return BaseVelocity + Velocity; } }

        public void Reset()
        {
            Buttons = 0;
            JustJumped = false;
            JustGrounded = false;
            ForwardMove = 0;
            SideMove = 0;
            //Ducked = false;
            Momentum = false;
            Surfing = false;
            Sliding = false;
            WaterDepth = 0;
        }
    }
}
