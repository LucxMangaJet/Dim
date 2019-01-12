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
        [SerializeField] bool debug;

        Animator animator;
        PlayerStateMachine pSM;
        PlayerController pc;
        Rigidbody rb;
        Dictionary<StateType, Tuple<string, string>> animationRelation;

        bool enable = false;
        bool oldDirIsLeft = false;

        
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

            animationRelation = new Dictionary<StateType, Tuple<string, string>>();

            animationRelation.Add(StateType.Idle, new Tuple<string, string>("isIdle", "MAIN_IDLE_ANIMATION"));
            animationRelation.Add(StateType.Moving, new Tuple<string, string>("isWalking", "MAIN_ANIM_WALKING"));
            animationRelation.Add(StateType.Crouch, new Tuple<string, string>("isCrouched", "MAIN_SNEAKIDLE_ANIMATION"));
            animationRelation.Add(StateType.CrouchMoving, new Tuple<string, string>("isCrouchedWalking", "MAIN_SNEAKING_ANIMATON"));
            animationRelation.Add(StateType.Sprint, new Tuple<string, string>("isRunning", "MAIN_RUNNING_ANIMATION"));
            animationRelation.Add(StateType.Jump, new Tuple<string, string>("isJumping", "MAIN_JUMPING_NEW"));
            animationRelation.Add(StateType.Emission, new Tuple<string, string>("isEmitting", "MAIN_EMIT_ANIM"));
            animationRelation.Add(StateType.Absorption, new Tuple<string, string>("isAbsorbing", "MAIN_ABSORPTION"));
        }


        private void Update()
        {
            if (enable)
            {
                CheckAnimationAndUpdate();
            }
        }

        private void CheckAnimationAndUpdate()
        {

            if(!animator.GetCurrentAnimatorStateInfo(0).IsName(animationRelation[pSM.CurrentState].Item2)  && animator.GetAnimatorTransitionInfo(0).duration ==0)
            {
                
                    if (debug)
                        Debug.Log("[ANIMATOR] Trigger to " + animationRelation[pSM.CurrentState].Item2);

                    animator.SetTrigger(animationRelation[pSM.CurrentState].Item1);
                
            }



            //set player direction
            float xVel = Mathf.Floor(Mathf.Abs(rb.velocity.x));
            bool moveLeft = rb.velocity.x < 0;

            if (xVel > idleMaxSpeed)
            {
                if (moveLeft != oldDirIsLeft)
                {
                    transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, (moveLeft ? -1 : +1) * Mathf.Abs(transform.localScale.z));
                    oldDirIsLeft = moveLeft;
                }
            }

        }

    }
}