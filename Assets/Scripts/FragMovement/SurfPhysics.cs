using UnityEngine;

namespace Fragsurf.Movement
{
    public class SurfPhysics
    {

        public static int GroundLayerMask = (1 << 0) | (1 << 16);
        public static int LadderLayerMask = (1 << 1);
        private static Collider[] _colliders = new Collider[MaxCollisions];
        private const int MaxCollisions = 128;

        //public const float SurfSlope = 0.7f;
        public static float SurfSlope = .7f;

        public static void ResolveCollisions(ISurfControllable controller)
        {
            var staticOrigin = controller.MoveData.Origin + new Vector3(0, controller.Collider.bounds.extents.y, 0);
            var numOverlaps = Physics.OverlapBoxNonAlloc(staticOrigin, controller.Collider.bounds.extents, _colliders,
                controller.Orientation, GroundLayerMask, QueryTriggerInteraction.Ignore);

            for (int i = 0; i < numOverlaps; i++)
            {
                if(!_colliders[i].enabled)
                {
                    continue;
                }

                // Unity 5.4.1f1's compiler doesn't support inline `out` variable declarations
                // (added in C# 7), so the out parameters are declared beforehand instead.
                // Physics.ComputePenetration itself doesn't exist in Unity 5.4.1f1 at all (added
                // in a later Unity version), so this calls our own SAT-based replacement below
                // instead of UnityEngine.Physics's.
                Vector3 direction;
                float distance;
                var pen = ComputePenetration(controller.Collider,
                    controller.MoveData.Origin,
                    controller.Orientation,
                    _colliders[i],
                    _colliders[i].transform.position,
                    _colliders[i].transform.rotation,
                    out direction,
                    out distance);

                if (!pen)
                {
                    continue;
                }

                // don't resolve if moving away from it
                // this was needed at one point but it might be causing problems now.  keeping it just in case
                //if (Vector3.Dot(direction, controller.MoveData.Velocity.normalized) > 0)
                //{
                //    continue;
                //}

                var penetrationVec = direction * distance;
                var velocityVec = -Vector3.Project(controller.MoveData.Velocity, -direction);

                controller.MoveData.Origin += penetrationVec;
                staticOrigin += penetrationVec;

                if (controller.MoveData.Surfing)
                {
                    ClipVelocity(controller.MoveData.Velocity, direction, ref controller.MoveData.Velocity, distance);
                }
                else
                {
                    controller.MoveData.Velocity += velocityVec;
                }
            }
        }

        // --- Physics.ComputePenetration replacement -----------------------------------------
        // Unity 5.4.1f1 doesn't have Physics.ComputePenetration (added in a later Unity
        // version), so this reimplements its minimum-translation-vector behavior using the
        // separating axis theorem (SAT). For two boxes, SAT is mathematically exact - it finds
        // the true axis and depth of minimum penetration, same as PhysX would for box-vs-box.
        // Any collider that isn't a BoxCollider doesn't have a simple analytic oriented box, so
        // its world-space AABB (Bounds) is used as a stand-in box instead; this is only an
        // approximation of the real shape for spheres/capsules/mesh/terrain colliders, since
        // their exact geometry isn't available without PhysX's own penetration query.
        public static bool ComputePenetration(Collider colliderA, Vector3 positionA, Quaternion rotationA,
            Collider colliderB, Vector3 positionB, Quaternion rotationB,
            out Vector3 direction, out float distance)
        {
            Vector3 centerA, extentsA;
            Vector3[] axesA;
            GetBoxInfo(colliderA, positionA, rotationA, out centerA, out extentsA, out axesA);

            Vector3 centerB, extentsB;
            Vector3[] axesB;
            GetBoxInfo(colliderB, positionB, rotationB, out centerB, out extentsB, out axesB);

            return BoxBoxPenetration(centerA, extentsA, axesA, centerB, extentsB, axesB, out direction, out distance);
        }

        // Builds an oriented-box representation (world center, half-extents, orthonormal axes)
        // for a collider placed at the given position/rotation override, matching what
        // Physics.ComputePenetration's parameters describe. BoxColliders are handled exactly
        // (accounting for the collider's local center/size and the transform's scale); anything
        // else falls back to its current world-space AABB, ignoring the position/rotation
        // override since a general shape can't be re-posed without the physics engine itself.
        private static void GetBoxInfo(Collider collider, Vector3 position, Quaternion rotation,
            out Vector3 center, out Vector3 extents, out Vector3[] axes)
        {
            axes = new Vector3[3]
            {
                rotation * Vector3.right,
                rotation * Vector3.up,
                rotation * Vector3.forward
            };

            var box = collider as BoxCollider;
            if (box != null)
            {
                var scale = box.transform.lossyScale;
                extents = Vector3.Scale(box.size, scale) * 0.5f;
                center = position + rotation * Vector3.Scale(box.center, scale);
                return;
            }

            var bounds = collider.bounds;
            center = bounds.center;
            extents = bounds.extents;
            axes[0] = Vector3.right;
            axes[1] = Vector3.up;
            axes[2] = Vector3.forward;
        }

        // Full 15-axis SAT test for two oriented boxes: the 3 face-normal axes of each box, plus
        // the 9 cross products between them. If every axis shows overlap, the boxes intersect and
        // the axis with the smallest overlap is the minimum translation vector (direction *
        // distance) that separates them - oriented so it points away from box B.
        private static bool BoxBoxPenetration(Vector3 centerA, Vector3 extentsA, Vector3[] axesA,
            Vector3 centerB, Vector3 extentsB, Vector3[] axesB,
            out Vector3 direction, out float distance)
        {
            direction = Vector3.zero;
            distance = 0f;

            var minOverlap = float.MaxValue;
            var minAxis = Vector3.zero;
            var centerDiff = centerA - centerB;

            for (int i = 0; i < 15; i++)
            {
                Vector3 axis;
                if (i < 3) axis = axesA[i];
                else if (i < 6) axis = axesB[i - 3];
                else
                {
                    var ai = (i - 6) / 3;
                    var bi = (i - 6) % 3;
                    axis = Vector3.Cross(axesA[ai], axesB[bi]);
                }

                var sqrMag = axis.sqrMagnitude;
                if (sqrMag < 1e-6f)
                {
                    // Near-parallel edge axes don't add separating-axis information beyond
                    // what the face-normal axes already cover, and can't be normalized safely.
                    continue;
                }
                axis = axis / Mathf.Sqrt(sqrMag);

                var rA = Mathf.Abs(Vector3.Dot(axesA[0], axis)) * extentsA.x
                       + Mathf.Abs(Vector3.Dot(axesA[1], axis)) * extentsA.y
                       + Mathf.Abs(Vector3.Dot(axesA[2], axis)) * extentsA.z;

                var rB = Mathf.Abs(Vector3.Dot(axesB[0], axis)) * extentsB.x
                       + Mathf.Abs(Vector3.Dot(axesB[1], axis)) * extentsB.y
                       + Mathf.Abs(Vector3.Dot(axesB[2], axis)) * extentsB.z;

                var centerDist = Mathf.Abs(Vector3.Dot(centerDiff, axis));
                var overlap = rA + rB - centerDist;

                if (overlap <= 0f)
                {
                    // A separating axis exists, so the boxes aren't overlapping at all.
                    return false;
                }

                if (overlap < minOverlap)
                {
                    minOverlap = overlap;
                    minAxis = axis;
                }
            }

            // Orient the axis so it points away from B - the direction colliderA should move
            // to resolve the overlap, matching Physics.ComputePenetration's documented `direction`.
            if (Vector3.Dot(centerDiff, minAxis) < 0f)
            {
                minAxis = -minAxis;
            }

            direction = minAxis;
            distance = minOverlap;
            return true;
        }

        public static void Friction(ref Vector3 velocity, float stopSpeed, float friction, float deltaTime)
        {
            var speed = velocity.magnitude;

            if (speed < 0.0001905f)
            {
                return;
            }

            var drop = 0f;

            // apply ground friction
            var control = (speed < stopSpeed) ? stopSpeed : speed;
            drop += control * friction * deltaTime;

            // scale the velocity
            var newspeed = speed - drop;
            if (newspeed < 0)
                newspeed = 0;

            if (newspeed != speed)
            {
                newspeed /= speed;
                velocity *= newspeed;
            }
        }

        public static Vector3 AirAccelerate(Vector3 velocity, Vector3 wishdir, float wishspeed, float accel, float airCap, float deltaTime, out float gainCoefficient)
        {
            gainCoefficient = 0f;
            var wishSpd = wishspeed;

            // Cap speed
            if (wishSpd > airCap)
            {
                wishSpd = airCap;
            }

            // Determine veer amount
            var currentspeed = Vector3.Dot(velocity, wishdir);

            // See how much to add
            var addspeed = wishSpd - currentspeed;

            // If not adding any, done.
            if (addspeed <= 0)
            {
                return Vector3.zero;
            }

            // Determine acceleration speed after acceleration
            var accelspeed = accel * wishspeed * deltaTime;

            // Cap it
            if (accelspeed > addspeed)
            {
                accelspeed = addspeed;
            }

            if (currentspeed < airCap)
            {
                gainCoefficient = (wishSpd - Mathf.Abs(currentspeed)) / wishSpd;
            }

            return accelspeed * wishdir;
        }

        public static Vector3 Accelerate(Vector3 currentVelocity, Vector3 wishdir, float wishspeed, float accel, float deltaTime, float surfaceFriction)
        {
            // See if we are changing direction a bit
            var currentspeed = Vector3.Dot(currentVelocity, wishdir);

            // Reduce wishspeed by the amount of veer.
            var addspeed = wishspeed - currentspeed;

            // If not going to add any speed, done.
            if (addspeed <= 0)
            {
                return Vector3.zero;
            }

            // Determine amount of accleration.
            var accelspeed = accel * deltaTime * wishspeed * surfaceFriction;

            // Cap at addspeed
            if (accelspeed > addspeed)
                accelspeed = addspeed;

            return accelspeed * wishdir;
        }

        private static RaycastHit[] _hits = new RaycastHit[32];

        //-- Used when needing to calculate velocity change when hitting a non-surfable surface. Will not work if bhopping up a surface, but will going down a surface or flat normal
        public static void Reflect(ISurfControllable surfer, float deltaTime, Vector3 hitNormal)
        {
            var workingVel = surfer.MoveData.JustGrounded
                ? surfer.MoveData.PreGroundedVelocity
                : surfer.MoveData.Velocity;
            workingVel += surfer.MoveData.BaseVelocity;

            if (hitNormal.y <= SurfSlope
                || hitNormal.y >= 1)
            {
                return;
            }

            ClipVelocity(workingVel, hitNormal, ref workingVel, 1f);

            workingVel -= surfer.MoveData.BaseVelocity;
            surfer.MoveData.Velocity = workingVel;
        }

        //-- Not used
        public static void Reflect(ISurfControllable surfer, float deltaTime)
        {
            var workingVel = surfer.MoveData.JustGrounded
                ? surfer.MoveData.PreGroundedVelocity
                : surfer.MoveData.Velocity;
            workingVel += surfer.MoveData.BaseVelocity;
            var clipped = false;

            //if (contactOffset != 0)
            //{
            //    var longSide = Mathf.Sqrt(contactOffset * contactOffset + contactOffset * contactOffset);
            //    distance += longSide;
            //    extents *= (1f - contactOffset);
            //}

            //var center = surfer.MoveData.Origin + new Vector3(0, surfer.Collider.bounds.extents.y, 0);
            //var end = center + (Vector3.down * 0.2f) + (workingVel * deltaTime);
            //var dist = Vector3.Distance(center, end);
            //var dir = (end - center).normalized;

            var center = surfer.MoveData.Origin + new Vector3(0, surfer.Collider.bounds.extents.y + 0.1f, 0);
            var dist = 0.2f;
            var dir = (Vector3.down + (workingVel * deltaTime)).normalized;
            //var dir = (Vector3.down + (workingVel * deltaTime)).normalized;

            var hitCount = Physics.BoxCastNonAlloc(results: _hits,
                center: center,
                direction: dir,
                orientation: surfer.Orientation,
                maxDistance: dist,
                halfExtents: surfer.Collider.bounds.extents,
                // Unity 5.4.1f1 names this parameter `layermask` (all lowercase), not the
                // camelCase `layerMask` used in later Unity versions.
                layermask: GroundLayerMask,
                queryTriggerInteraction: QueryTriggerInteraction.Ignore);

            for (int i = 0; i < hitCount; i++)
            {
                if (Tracer.HitIsShit(_hits[i])
                    || _hits[i].collider == null)
                {
                    continue;
                }

                if (_hits[i].normal.y <= SurfSlope
                    || _hits[i].normal.y >= 1)
                {
                    continue;
                }

                var slopeDir = Vector3.Cross(Vector3.up, Vector3.Cross(Vector3.up, _hits[i].normal));
                var dot = Vector3.Dot(workingVel.normalized, slopeDir);
                var goingAgainstSlope = dot > 0;

                if (!goingAgainstSlope)
                {
                    continue;
                }

                ClipVelocity(workingVel, _hits[i].normal, ref workingVel, 1f);
                clipped = true;
            }

            workingVel -= surfer.MoveData.BaseVelocity;

            if (clipped)
            {
                surfer.MoveData.Velocity = workingVel;
            }

            //var oldVelocityMagnitude2d = new Vector2(surfer.MoveData.Velocity.x, surfer.MoveData.Velocity.z).magnitude;

            //

            //var newVelocityMagnitude2d = new Vector2(surfer.MoveData.Velocity.x, surfer.MoveData.Velocity.z).magnitude;
            //float fLateralStoppingAmount = oldVelocityMagnitude2d - newVelocityMagnitude2d;
            //Debug.Log(fLateralStoppingAmount);
        }

        public static void ClipVelocity(Vector3 input, Vector3 normal, ref Vector3 output, float overbounce, bool debug = false)
        {
            // Determine how far along plane to slide based on incoming direction.
            var backoff = Vector3.Dot(input, normal) * overbounce;

            for (int i = 0; i < 3; i++)
            {
                var change = normal[i] * backoff;
                output[i] = input[i] - change;
            }

            // iterate once to make sure we aren't still moving through the plane
            float adjust = Vector3.Dot(output, normal);
            if (adjust < 0.0f)
            {
                output -= (normal * adjust);
            }

            if (debug) Debug.Log(input + ":" + normal + ":" + output);
        }

        public static float ClampAngle(float angle, float from, float to)
        {
            if (angle < 0f) angle = 360 + angle;
            if (angle > 180f) return Mathf.Max(angle, 360 + from);
            return Mathf.Min(angle, to);
        }

    }
}
