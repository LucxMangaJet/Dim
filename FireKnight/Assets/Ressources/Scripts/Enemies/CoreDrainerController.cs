using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Audio;
using Dim.Interaction;
using UnityEngine.Audio;

namespace Dim.Enemies
{
    /////////////////////////////////////////////////
    /// Responsible for the Behavior of a CoreDrainer.
    /////////////////////////////////////////////////
    public class CoreDrainerController : MonoBehaviour, IGameObjectDamageTaker, ISoundMechanicTaker , IEnergyObject, Visualize.IExtraVisualization
    {
        [Header("Debug")]
        public bool Debug;

        [Header("Energy Settings")]
        private byte energy;
        

        [Header("Drain Settings")]
        public float DrainPreparationTimeInSec;
        public float DrainDurationInSec;
        public GameObject AbsorptionArea;
        [HideInInspector] public Visualize.CoreDrainerParticleFlowController DrainFlow;

        [Header("Sound Detection")]
        public SoundHeard LastSoundHeard;
        public float HearingCoolDown;
        public float SoundInterestLength;

        [Header("Movement")]
        public float MovementCap;
        public float MovementForce;
        public CoreDrainerBase Base;
        public float IdleToWalkBackTime;
        public float RechargeMinTime;

        [Header("Sound")]
        [SerializeField] AudioClip walkSound;
        [SerializeField] AudioClip succSound;

        private bool isDestroyed = false;

        [HideInInspector] public Rigidbody rb;
        private CoreDrainerStateMachine sM;
        private AudioSource audioSource;

        private float hearingTimeStamp;

        public event System.Action<byte> OnEnergyChange;

        public byte Energy
        {
            get
            {
                return energy;
            }

            set
            {
                energy = value;
                OnEnergyChange?.Invoke(value);
            }
        }

        public event System.Action OnDestroyed;

        void Start()
        {
            audioSource = GetComponent<AudioSource>();
            SoundMechanicHandler.AddListener(transform);
            EnergyHandler.AddEnergyObject(this);
            Energy = 0;
            rb = GetComponent<Rigidbody>();
            sM = new CoreDrainerStateMachine(this);
            SendMessage("GetFSM", sM);
        }


        void Update()
        {
            if (!isDestroyed)
            {
                CheckIfInterestRanOutOnSound();
                sM.Update();
                UpdateFaceDirection();
            }
        }


        private void UpdateFaceDirection()
        {
            Vector3 dir = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            if (dir.magnitude > 0.1)
            {
                transform.forward = Vector3.Lerp(transform.forward, dir, 0.51f);
            }
        }

        public void Destroy()
        {
            EnergyArea ha;
            ha = EnergyHandler.CreateEnergyArea(transform.position);
            for (int i = 0; i < GetEnergyAmount(); i++)
            {
                ha.AddEnergy(false);
            }
            Energy = 0;
            isDestroyed = true;
            OnDestroyed?.Invoke();
            rb.isKinematic = true;
            Destroy(GetComponent<Collider>());
            Destroy(AbsorptionArea);
            SoundMechanicHandler.RemoveListener(transform);
            EnergyHandler.RemoveEnergyObject(this);

        }


        public void CheckIfInterestRanOutOnSound()
        {
            if (LastSoundHeard != null)
            {
                if ((Time.time - LastSoundHeard.TimeStamp) > SoundInterestLength)
                {
                    LastSoundHeard = null;
                }
            }
        }




        public void TakeDamage()
        {
            Destroy();
        }


        public void RegisterSoundHeard(SoundHeard sound)
        {
            if(Energy + Base.Storage.Energy == 0)
            {
                return;
            }

            if (sound.Loudness < 5)
            {
                return;
            }

            if(Time.time- hearingTimeStamp < HearingCoolDown)
            {
                return;
            }

            hearingTimeStamp = Time.time;


            if (LastSoundHeard == null)
            {
                LastSoundHeard = sound;
            }
            else
            {
                //UnityEngine.Debug.Log("S1: " + LastSoundHeard.Intesity + " at " + LastSoundHeard.TimeStamp + " = " + (float)LastSoundHeard);
                //UnityEngine.Debug.Log("S2: " + sound.Intesity + " at " + sound.TimeStamp + " = " + (float)sound);
                //UnityEngine.Debug.Log("Is S1 smaller the S2? = " + (LastSoundHeard < sound));

                if (LastSoundHeard < sound)
                {
                    LastSoundHeard = sound;
                }
            }
        }

        public string[] GetExtraVisualizationElements()
        {
            return new string[]
            { 
               (isDestroyed)?"DESTROYED" : "FSM State: " + sM.CurrentState.ToString(),
                "Energy: "+ Energy.ToString()
            };

        }

        public float GetEnergyAmount()
        {
            return Energy;
        }

        public Transform GetTransform()
        {
            return transform;
        }

        public void PlayAbsorbtionSound()
        {
            audioSource.loop = false;
            audioSource.clip = succSound;
            audioSource.Play();
        }

        public void PlayWalkingSound()
        {
            audioSource.loop = true;
            audioSource.clip = walkSound;
            audioSource.Play();
        }
        
    }


}