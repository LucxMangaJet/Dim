using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dim.Interaction
{

    /////////////////////////////////////////////////
    /// Used to toggle Scripts based on Storage Energy Level
    /////////////////////////////////////////////////
    public class ScriptStarterMotor : InteractionBase
    {


        [SerializeField] byte activationMinEnergy;
        [SerializeField] Behaviour script;

        [Header("IfMovementEnviromentController")]
        [SerializeField] bool smooth;
        [SerializeField] float smoothFactor;

        MovingEnvironmentController motor;

        float smoothTarget = 1f;
        bool active;

        void Start ()
        {
            if (smooth)
            {
                if (script is MovingEnvironmentController)
                {
                    motor = (MovingEnvironmentController)script;
                }
                else
                {
                    Debug.LogWarning("smoothing is only possible for type MovingEnvironmentController_TEMP");
                }
            }
        }

        void FixedUpdate ()
        {
            if (motor != null) {
                if (motor.speed != smoothTarget)
                {
                    if (motor.speed < smoothTarget)
                    {
                        motor.speed += smoothFactor;
                        if (motor.speed > smoothTarget)
                        {
                            motor.speed = smoothTarget;
                        }
                    }
                    else
                    {
                        motor.speed -= smoothFactor;
                        if (motor.speed < smoothTarget)
                        {
                            motor.speed = smoothTarget;
                        }
                    }
                }
            }
        }

        public override void OnEnergyChange(byte newEnergy)
        {
            if (newEnergy >= activationMinEnergy)
            {
                if (!active)
                {
                    if (motor != null) {
                        smoothTarget = 1;
                    } else
                    {
                        script.enabled = true;
                    }
                    active = true;
                }
            }
            else
            {
                if (active)
                {
                    if (motor != null)
                    {
                        smoothTarget = 0;
                    }
                    else
                    {
                        script.enabled = false;
                    }
                }
                active = false;
            }
        }

    }
}