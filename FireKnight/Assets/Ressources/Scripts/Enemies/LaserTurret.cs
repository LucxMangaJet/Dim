using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Dim.Enemies {


    /////////////////////////////////////////////////
    /// Responsible for the Behavior of a LaserTurret, inherits from InteractionBase like other gameplay elements.
    /////////////////////////////////////////////////
    public class LaserTurret : Interaction.InteractionBase, Visualize.IExtraVisualization
    {

        [SerializeField] float coolDown;
        [Header("Detection Cone")]
        [SerializeField] float detectionRange;
        [SerializeField] float detectionBaseRadius;
        [SerializeField] Transform tip;
        float timeStamp = 0;
        bool isActive;
        
        AudioSource audioSource;
        Animator animator;
        EnergyDetectionLook visualizer;
        Transform laserTurretTipPosiiton;
        [SerializeField] AudioClip shootingSound;

        List<Transform> objectsShot = new List<Transform>();

        public float DetectionRange
        {
            get
            {
                return detectionRange;
            }
        }

        public float DetectionBaseRadius
        {
            get
            {
                return detectionBaseRadius;
            }
        }

        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
            animator = GetComponent<Animator>();
            visualizer = GetComponent<EnergyDetectionLook>();

            laserTurretTipPosiiton = transform;
            while (laserTurretTipPosiiton.name != "Tip")
            {
                laserTurretTipPosiiton = laserTurretTipPosiiton.GetChild(0);
            }
        }

        private void Update()
        {
            if(Storage.Energy == 0 && Time.time-timeStamp>coolDown)
            {
                isActive = true;
                Transform t=null;
                if (EnergyHandler.GetStrongestEnergyInCone(tip.position, Vector3.down, detectionRange, detectionBaseRadius, out t))
                {
                    if (!objectsShot.Contains(t))
                    {
                        Instantiate(PrefabHolder.EnergyLaser(), laserTurretTipPosiiton.position, Quaternion.Euler(Vector3.right*90f));
                        audioSource.clip = shootingSound;
                        audioSource.Play();
                        animator.SetTrigger("Shoot");
                        objectsShot.Add(t);
                        if (t != null)
                        {
                            var damageTaker = t.GetComponent<IGameObjectDamageTaker>();
                            if (damageTaker != null)
                            {
                                damageTaker.TakeDamage();
                            }
                        }
                        timeStamp = Time.time;
                    }
                }
            } else
            {
                isActive = false;
            }

            if (isActive != visualizer.isVisible)
            {
                visualizer.UpdateVisibility(isActive);
            }
        }

        public override void OnEnergyChange(byte newEnergy)
        {

        }


        protected override void OnDrawGizmos()
        {
            if (debug)
            {
                Gizmos.color = Color.yellow;
                Visualize.GizmosUtils.DrawCone(tip.position, Vector3.down, detectionRange, DetectionBaseRadius);
            }

            base.OnDrawGizmos();
        }

       
    }
}
