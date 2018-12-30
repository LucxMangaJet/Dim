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

        [SerializeField] byte activationMinEnergy;

        Animator animator;
        bool active;

        public void Start()
        {
            animator = GetComponent<Animator>();

        }

        public override void OnEnergyChange(byte newEnergy)
        {
            if(newEnergy>= activationMinEnergy)
            {
                if (!active)
                {
                    active = true;
                    animator.enabled = true;
                }
            }
            else
            {
                if (active)
                {
                    animator.enabled = false;
                    active = false;
                }
            }
        }

    }
}