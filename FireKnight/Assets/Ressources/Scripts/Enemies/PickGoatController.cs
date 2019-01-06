using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Audio;

namespace Dim.Enemies
{
    /////////////////////////////////////////////////
    /// Responsible for the Behavior of a PickGoat.
    /////////////////////////////////////////////////
    public class PickGoatController : MonoBehaviour, IGameObjectDamageTaker, IEnergyObject
    {
        [Header("Debug")]
        public bool Debug;


        public BoxCollider headDamageCollider;
        [Header("Movement")]
        public float MovementForce;
        public float MovementCap;
        [Header("Detection")]
        public float DetectionRange;
        public float DetectionRadius;
        public Transform DetectionOrigin1, DetectionOrigin2;

        [Header("Sounds")]
        [SerializeField] AudioClip deathSound;


        private bool isDestroyed=false;
        private PickGoatStateMachine sM;
        [HideInInspector] public Rigidbody rb;
        AudioSource audioSource;
        [HideInInspector] public GroundChecker groundChecker;

        public System.Action OnDestroyed;

        void Start()
        {
            EnergyHandler.AddEnergyObject(this);
            rb = GetComponent<Rigidbody>();
            groundChecker = GetComponentInChildren<GroundChecker>();
            sM = new PickGoatStateMachine(this);
            SendMessage("GetFSM", sM);
            audioSource = GetComponent<AudioSource>();
        }


        void Update()
        {
            if ( !isDestroyed)
            {
                sM.Update();

                if (Mathf.Abs(rb.velocity.x)>0.1)
                {
                    transform.forward = new Vector3(-rb.velocity.x, 0, 0);
                }
                
            }
        }

        public void Destroy()
        {
            if (isDestroyed)
            {
                return;
            }
            isDestroyed = true;
            OnDestroyed?.Invoke();
            rb.isKinematic = true;
            Destroy(GetComponent<Collider>());
            Destroy(headDamageCollider);
            Destroy(transform.GetComponentInChildren<SkinnedMeshRenderer>());
            audioSource.clip = deathSound;
            audioSource.Play();
            EnergyHandler.RemoveEnergyObject(this);

            EnergyArea ha;
            ha = EnergyHandler.CreateEnergyArea(transform.position);
            ha.AddEnergy(false);

            VisualDestructionHandler destructionHandler = GetComponent<VisualDestructionHandler>();
            destructionHandler.Destroy(rb.velocity);

        }


        private void OnDrawGizmos()
        {
            if (!Debug)
            {
                return;
            }

            Gizmos.color = Color.yellow;
            Visualize.GizmosUtils.DrawCone(DetectionOrigin1.position, DetectionOrigin1.forward, DetectionRange, DetectionRadius);
            Visualize.GizmosUtils.DrawCone(DetectionOrigin2.position, DetectionOrigin2.forward, DetectionRange, DetectionRadius);
        }
        

        public void TakeDamage()
        {
            Destroy();
        }

        public float GetEnergyAmount()
        {
            return 10;
        }

        public Transform GetTransform()
        {
            return transform;
        }
    }

    
}
