using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Dim.Enemies
{

    /////////////////////////////////
    /// Deals with playing the animations of a CoreDrainer.
    //////////////////////////////// 
    public class CoreDrainerAnimationHandler : MonoBehaviour
    {

        Animator animator;
        CoreDrainerStateMachine pSM;
        CoreDrainerController pgC;


        void Start()
        {
            animator = GetComponentInChildren<Animator>();
            pgC = GetComponent<CoreDrainerController>();
        }


        private void GetFSM(CoreDrainerStateMachine fSM)
        {
            pSM = fSM;
            pSM.OnTransitionEvent += CatchStateChange;
        }

        private void CatchStateChange(CoreDrainerStateType from, CoreDrainerStateType to)
        {
            string stringToSet = "";

            switch (to)
            {
                case CoreDrainerStateType.PrepareToDrain:
                    stringToSet = "isDraining";
                    break;

                case CoreDrainerStateType.WalkOutOfRecharging:
                    stringToSet = "isWalking";
                    break;

                case CoreDrainerStateType.Idle:
                    stringToSet = "isIdle";
                    break;

                case CoreDrainerStateType.Recharging:
                    stringToSet = "isSleeping";
                    break;

                case CoreDrainerStateType.WalkToSound:
                    stringToSet = "isWalking";
                    break;

                case CoreDrainerStateType.WalkToRecharging:
                    stringToSet = "isWalking";
                    break;
            }

            if(stringToSet!="")
            animator.SetTrigger(stringToSet);

        }
    }

}
