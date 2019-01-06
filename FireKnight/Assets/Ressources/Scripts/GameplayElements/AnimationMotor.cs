using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Dim.Interaction
{

    /////////////////////////////////////////////////
    /// Used to toggle Animations based on Storage Energy Level
    /////////////////////////////////////////////////
    public class AnimationMotor : InteractionBase
    {

        [SerializeField] byte activationEnergy;
        [SerializeField] bool exactEnergy;
        [SerializeField] bool setTrigger;
        [SerializeField] string triggerName;

        Animator animator;
        bool active;

        public void Start()
        {
            animator = GetComponent<Animator>();

        }

        public override void OnEnergyChange(byte newEnergy)
        {
            bool query = newEnergy >= activationEnergy;
            if (exactEnergy)
            {
                query = newEnergy == activationEnergy;
            }

            if(query)
            {
                if (!active)
                {
                    active = true;
                    if (setTrigger)
                    {
                        animator.SetTrigger(triggerName);
                    }
                    else
                    {
                        animator.enabled = true;
                    }


                }
            }
            else
            {
                if (active)
                {
                    if (setTrigger)
                    {
                       //do nothing
                    }
                    else
                    {
                        animator.enabled = false;
                    }

                    active = false;
                }
            }
        }

    }
}