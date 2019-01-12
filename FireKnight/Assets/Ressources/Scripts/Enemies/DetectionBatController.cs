using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Dim.Interaction;

namespace Dim.Enemies
{

    /////////////////////////////////////////////////
    /// Responsible for the Behavior of a DetectionBat, is connected to a Storage.
    /////////////////////////////////////////////////
    public class DetectionBatController : Interaction.InteractionBase, ISoundMechanicTaker
    {
        float repetitionMinimum = 6;
        float repetitionLoudness = 10;
        [SerializeField] float repetitionRange;
        [SerializeField] float cooldownTime;
        [SerializeField] float minEnergyToPlaySound;
        [SerializeField] Light light;

        float cooldownTimestamp=0;
        AudioSource audioSource;
        Animator animator;

        void Start()
        {
            SoundMechanicHandler.AddListener(transform);
            audioSource = GetComponent<AudioSource>();
            animator = GetComponent<Animator>();
        }

        public void RegisterSoundHeard(SoundHeard sound)
        {
            if (Storage.Energy >= minEnergyToPlaySound)
            {
                if (sound.Loudness >= repetitionMinimum && Time.timeSinceLevelLoad - cooldownTimestamp > cooldownTime)
                {
                    PlaySound();
                }
            }
        }

        public override void OnEnergyChange(byte newEnergy)
        {

            if( newEnergy>= minEnergyToPlaySound)
            {
                //PlaySound();
            }
        }

        private void PlaySound()
        {
            cooldownTimestamp = Time.timeSinceLevelLoad;
            animator.SetTrigger("isScreaming");
            
            Invoke("Play", 1f);   
        }

        private void Play()
        {
            audioSource.Play();
            SoundMechanicHandler.PlaySound(transform, transform.position, repetitionLoudness, repetitionRange,true);
            light.enabled = true;
            Invoke("DisableLight", 1);
        }

        private void DisableLight()
        {
            light.enabled = false;
        }

        private void OnDestroy()
        {
            SoundMechanicHandler.RemoveListener(transform);
        }
    }
}