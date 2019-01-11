using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dim.Interaction {

    /////////////////////////////////////////////////
    /// Used to toggle Sounds based on Storage Energy Level
    /////////////////////////////////////////////////
    public class SoundMotor : InteractionBase
    {

        [SerializeField] byte activationEnergy;
        [SerializeField] bool exactEnergy;
        [SerializeField] bool changeVolume;
        [SerializeField] float volumeLevel;

        [SerializeField] AudioSource audioSource;

        bool active;


        public override void OnEnergyChange(byte newEnergy)
        {
            bool query = newEnergy >= activationEnergy;
            if (exactEnergy)
            {
                query = newEnergy == activationEnergy;
            }

            if (query)
            {
                if (!active)
                {
                    active = true;

                    if (Time.time < 0.5)
                    {
                        return;
                    }

                    if (changeVolume)
                    {
                        if (debug)
                            Debug.Log("Setting volume to " + volumeLevel);
                        audioSource.volume = volumeLevel;
                    }
                    else
                    {
                        if (!audioSource.isPlaying)
                        {
                            audioSource.Play();
                        }
                    }



                }
            }
            else
            {
                if (active)
                {
                    if (!changeVolume)
                    {
                        audioSource.Stop();
                    }
                    
                    active = false;
                }
            }
        }

    }
}

