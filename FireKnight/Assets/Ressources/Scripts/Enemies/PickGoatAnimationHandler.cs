using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dim.Enemies
{
    /////////////////////////////////////////////////
    /// Responsible for the Animations on a PickGoat.
    /////////////////////////////////////////////////
    public class PickGoatAnimationHandler : MonoBehaviour
    {



        Animator animator;
        PickGoatStateMachine pSM;
        PickGoatController pgC;


        void Start()
        {
            animator = GetComponentInChildren<Animator>();
            pgC = GetComponent<PickGoatController>();
            pgC.OnDestroyed += CatchDestroyed;
        }


        private void GetFSM(PickGoatStateMachine fSM)
        {
            pSM = fSM;
            pSM.OnTransitionEvent += CatchStateChange;
        }

        private void CatchStateChange(PickGoatStateType from, PickGoatStateType to)
        {
            string stringToSet = "";

            switch (to)
            {
                case PickGoatStateType.Walk:
                    stringToSet = "isWalking";
                    break;
                case PickGoatStateType.Mine:
                    stringToSet = "isMining";
                    break;
                

            }

            animator.SetTrigger(stringToSet);

        }


        private void CatchDestroyed()
        {
            animator.SetTrigger("isDead");

        }
        



    }
}