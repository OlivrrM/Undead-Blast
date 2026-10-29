using UnityEngine;

namespace Fragsurf.Movement
{

    //public delegate void CollisionHandler(Collision collision);
    public delegate void ColliderHandler(Collider collision);

    public class CollisionRaiser : MonoBehaviour
    {

        ///// Fields /////

        //public event CollisionHandler CollisionEnter;
        //public event CollisionHandler CollisionStay;
        //public event CollisionHandler CollisionExit;

        public event ColliderHandler TriggerEnter;
        public event ColliderHandler TriggerStay;
        public event ColliderHandler TriggerExit;

        ///// Methods /////

        private void OnDestroy()
        {
            TriggerEnter = null;
            TriggerStay = null;
            TriggerExit = null;
        }

        //private void OnCollisionEnter(Collision collision)
        //{
        //    TriggerEnter?.Invoke(collision.collider);
        //}

        //private void OnCollisionStay(Collision collision)
        //{
        //    TriggerStay?.Invoke(collision.collider);
        //}

        //private void OnCollisionExit(Collision collision)
        //{
        //    TriggerExit?.Invoke(collision.collider);
        //}

        public void RaiseTriggerEnter(Collider other)
        {
            // Unity 5.4.1f1's compiler doesn't support the null-conditional operator (?.), added in C# 6.
            if (TriggerEnter != null) TriggerEnter.Invoke(other);
        }

        public void RaiseTriggerStay(Collider other)
        {
            if (TriggerStay != null) TriggerStay.Invoke(other);
        }

        public void RaiseTriggerExit(Collider other)
        {
            if (TriggerExit != null) TriggerExit.Invoke(other);
        }

    }
}

