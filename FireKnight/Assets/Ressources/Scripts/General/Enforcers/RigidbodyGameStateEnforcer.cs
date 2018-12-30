using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Dim.Enforcer
{


    /////////////////////////////////////////////////
    /// [UNIMPLEMENTED]
    /////////////////////////////////////////////////
    public class RigidbodyGameStateEnforcer : MonoBehaviour
    {

        [SerializeField] GameState.State state;
        bool isFrozen = false;
        Rigidbody rb;
        // Use this for initialization

        Vector3 storedVelocity;
        void Start()
        {
            rb = GetComponent<Rigidbody>();

            GameState.instance.StateChange += ControlRigidbody;
        }

        private void OnDestroy()
        {
            GameState.instance.StateChange -= ControlRigidbody;
        }

        void ControlRigidbody(GameState.State _state)
        {
            if (isFrozen)
            {
                if(state == _state)
                {
                    rb.isKinematic = false;
                    rb.velocity = storedVelocity;
                    isFrozen = false;
                }
            }
            else
            {
                if (state != _state)
                {
                    storedVelocity = rb.velocity;
                    rb.velocity = Vector3.zero;
                    rb.isKinematic = true;
                    isFrozen = true;
                }
            }
        }
    }
}