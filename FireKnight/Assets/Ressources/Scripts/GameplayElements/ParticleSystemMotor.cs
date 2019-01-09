using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dim.Interaction
{
    public class ParticleSystemMotor : InteractionBase
    {
        [SerializeField] byte activationMinEnergy;

        [SerializeField] bool justEnable;

        [Header("Change Intensity")]
        [SerializeField] int addAmount;
        [SerializeField] bool burstOnDisable;
        [SerializeField] int burstAmount;

        bool active;
        ParticleSystem ps;


        private void Start()
        {
            ps = GetComponent<ParticleSystem>();
            //startingIntensity = GetComponent<Light>().intensity;
        }

        public override void OnEnergyChange(byte newEnergy)
        {

            if (newEnergy >= activationMinEnergy)
            {
                if (!active)
                {
                    if (Time.time < 2)
                    {
                        active = true;
                        return;
                    }

                    if (justEnable)
                    {
                        Enable();
                    }
                    else
                    {
                        AddParticles();
                    }


                    active = true;
                }
            }
            else
            {
                if (active)
                {

                    if (justEnable)
                    {
                        Disable();
                    }
                    else
                    {
                        RemoveParticles();
                    }
                }
                active = false;
            }
        }

        private void AddParticles()
        {
            var pse = ps.emission;
            pse.rateOverTime= pse.rateOverTime.constant + addAmount;
            
        }

        private void RemoveParticles()
        {
            var pse = ps.emission;
            pse.rateOverTime = pse.rateOverTime.constant - addAmount;

            if (burstOnDisable)
            {
                ps.Emit(burstAmount);
            }
                
        }

        private void Enable()
        {
            if (debug)
            {
                Debug.Log("Playing " + ps.name);
            }
            ps.Play();
        }

        private void Disable()
        {
            if (debug)
            {
                Debug.Log("Stopping " + ps.name);
            }
            ps.Stop();
        }

        

    }
}
