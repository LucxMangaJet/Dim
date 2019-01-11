using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Dim.Player
{

    /////////////////////////////////////////////////
    /// Responsible for the animations played by the character, connects directly to the Animator.
    /////////////////////////////////////////////////
    [RequireComponent(typeof(Animator))]
    public class PlayerAnimationHandler : MonoBehaviour
    {
        [SerializeField] float idleMaxSpeed;

        Animator animator;
        PlayerStateMachine pSM;
        PlayerController pc;
        Rigidbody rb;

        bool enable = false;
        bool oldDirIsLeft = false;
        string currentSetVar ="";
        
        void Start()
        {
            animator = GetComponent<Animator>();
            rb = GetComponent<Rigidbody>();
            pc = GetComponent<PlayerController>();
        }

        //Called by PlayerController Start();
        private void GetFSM(PlayerStateMachine fSM)
        {
            pSM = fSM;
            enable = true;
        }


        private void Update()
        {
            if (enable)
            {
                string toSet = "";
                float xVel = Mathf.Floor(Mathf.Abs(rb.velocity.x));
                bool moveLeft = rb.velocity.x < 0;
                float yVel = Mathf.Abs(rb.velocity.y);

                if (pSM.CurrentState == StateType.Emission)
                {
                    toSet = "isEmitting";
                }
                else if (pSM.CurrentState == StateType.Absorption)
                {
                    toSet = "isAbsorbing";
                }
                else
                {
                    if (!pc.OnGround)
                    {
                        toSet = "isJumping";
                    }
                    else
                    {
                        if (xVel < idleMaxSpeed)
                        {
                            if (InputController.GetCrouch(InputStateType.PRESSED))
                            {
                                toSet = "isCrouched";
                            }
                            else
                            {
                                toSet = "isIdle";
                            }
                        }
                        else
                        {
                            if (xVel < pc.SprintSpeedCap)
                            {
                                if (InputController.GetCrouch(InputStateType.PRESSED))
                                {
                                    toSet = "isCrouchedWalking";
                                }
                                else
                                {
                                    toSet = "isWalking";
                                }
                            }
                            else
                            {
                                toSet = "isRunning";
                            }
                        }
                    }
                }

                if (xVel > idleMaxSpeed)
                {
                    if (moveLeft != oldDirIsLeft)
                    {
                        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, (moveLeft ? -1:+1) *  Mathf.Abs(transform.localScale.z));
                        oldDirIsLeft = moveLeft;
                    }
                }

                if(currentSetVar != toSet)
                {
                    SetAnimationToState(toSet);
                }
            }

        }

        private void SetAnimationToState(string s)
        {
            animator.SetTrigger(s);
            currentSetVar = s;
        }

    }
}