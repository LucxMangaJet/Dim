using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dim.Interaction
{

    public class IntroDoorScript : MonoBehaviour {

        [SerializeField] Storage Storage;

        Animator animator;
        AudioSource audioSource;
        bool activated = false;
        ParticleSystem Particlestop;

        public void Awake()
        {
            if (Storage != null)
            {
                Storage.OnEnergyChange += OnEnergyChange;
            }
            else
            {
                Debug.LogError("No Storage connected to " + gameObject.name);
            }
        }

        public void Start()
        {
            animator = GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();
            Particlestop = GameObject.FindGameObjectWithTag("DoorParticle").GetComponent<ParticleSystem>();

            animator.enabled = false;

        }

        void OnEnergyChange(byte newEnergy)
        {
            if (!activated)
            {
                if ((int)newEnergy == 0)
                {
                    animator.enabled = true;
                    audioSource.Play();
                    Particlestop.Stop();

                    activated = true;
                }
            }
        }
    }
}
