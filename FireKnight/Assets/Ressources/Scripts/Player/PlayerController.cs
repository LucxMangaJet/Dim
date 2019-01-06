using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using Dim.Interaction;
using System.IO;
using UnityEngine.Audio;

namespace Dim.Player
{


    /////////////////////////////////////////////////
    /// Centerpiece of the Player Behavior, controls Energy, damage taking and holds the PlayerStateMachine.
    /////////////////////////////////////////////////
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour, IGameObjectDamageTaker, Visualize.IExtraVisualization, IEnergyObject
    {

        [Header("Movement Settings")]
        public float MovementSpeedCap;
        public float CrouchedMovementSpeedCap;
        public float SprintSpeedCap;
        public float JumpForce;
        public float JumpGroundCollisionCheckStartingCD = 0.2f;
        public float MovementForce;
        public float SlowDownFactor;
        public bool IsInvincible = false;
        public bool LockControls = false;
        [SerializeField] PlayerFootSoundEmission leftFoot, rightFoot;

        [Header("Energy Settings")]
        [SerializeField] ParticleSystem emissionSystem;
        ParticleSystem.EmissionModule emissionEmissionModule;
        public Storage currentStorage = null;
        public float emissionEmitTime, emissionEndTime, absorptionAbsorbTime, absorbtionEndTime;

        [Header("Debug Settings")]
        public bool Debug;

        public event Action<byte> OnEnergyChange;

        private byte energyAmount;
        private float lastEmission;
        private bool onGround;
        private float jumpGroundCollisionCheckCD = 0;

        [HideInInspector]
        public Rigidbody rb;

        [Header("Sounds")]
        [SerializeField] AudioClip emitClip;
        [SerializeField] AudioClip absorbClip, deathClip;
        AudioSource audioSource;

        PlayerStateMachine myFSM;


        void Start()
        {
            EnergyHandler.AddEnergyObject(this);
            emissionEmissionModule = emissionSystem.emission;
            rb = GetComponent<Rigidbody>();
            audioSource = GetComponent<AudioSource>();
            rb.velocity = Vector3.zero;
            currentStorage = null;
            myFSM = new PlayerStateMachine(this);

            SendMessage("GetFSM", myFSM);
        }

        void Update()
        {
            CheckForDeath();
            if (!LockControls)
            {
                myFSM.Update();
            }
        }

        private void CheckForDeath()
        {
            if (EnergyAmount < 0)
            {
                KillPlayer();
            }
        }

        private void KillPlayer()
        {
            if (!IsInvincible && enabled)
            {
                UnityEngine.Debug.Log("Player Died, Respawning soon");
                audioSource.clip = deathClip;
                audioSource.Play();
                VisualDestructionHandler destructionHandler = GetComponent<VisualDestructionHandler>();
                if (destructionHandler != null)
                {
                    destructionHandler.Destroy(rb.velocity);
                }
                GetComponent<PlayerDeactivationHandler>().Dectivate();
                EnergyHandler.RemoveEnergyObject(this);
                Invoke("Respawn", 4);
                
            }
        }

        public void PlayEmitSound()
        {
            audioSource.clip = emitClip;
            audioSource.Play();
        }

        public void PlayAbsorbSound()
        {
            audioSource.clip = absorbClip;
            audioSource.Play();
        }

        public void Respawn()
        {
            LevelHandler.ReloadLevelAtLastCheckpoint();
        }

        public void ResetStats()
        {
            GetComponent<PlayerDeactivationHandler>().Activate();
            Start();
        }




        //public methods -------------------------------------

        public byte EnergyAmount
        {
            get
            {
                return energyAmount;
            }

            set
            {
                energyAmount = value;
                OnEnergyChange?.Invoke(energyAmount);
            }
        }

        public float LastEmission
        {
            get
            {
                return lastEmission;
            }

            set
            {
                lastEmission = value;
            }
        }


        public bool OnGround
        {
            get
            {
                return onGround;
            }

            set
            {
                onGround = value;
            }
        }

        public float JumpGroundCollisionCheckCD
        {
            get
            {
                return jumpGroundCollisionCheckCD;
            }

            set
            {
                jumpGroundCollisionCheckCD = value;
            }
        }

        public void TakeDamage()
        {
            KillPlayer();
        }


        public string[] GetExtraVisualizationElements()
        {
            return new string[]
            {
                "FSM State: " +myFSM.CurrentState.ToString(),
                "Energy: " +EnergyAmount.ToString(),
                "Velocity:" + rb.velocity
            };
        }

        public float GetEnergyAmount()
        {
            return 20;
        }

        public Transform GetTransform()
        {
            return transform;
        }

        public void SetEmissionSystem(bool enable)
        {
            if (enable)
            {
                emissionEmissionModule.rateOverTime = 100;
            }
            else
            {
                emissionEmissionModule.rateOverTime = 0;
            }
        }

        public void SetFeetLoudness(PlayerFootSoundEmission.StepType type)
        {
            leftFoot.SetLoudness(type);
            rightFoot.SetLoudness(type);
        }
    }
}