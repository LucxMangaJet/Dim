using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Dim.Interaction
{

    /////////////////////////////////////////////////
    /// Behavior script for Platforms.
    /////////////////////////////////////////////////
    public class Platform : InteractionBase
    {
        public Vector3[] platformEnergyPositions;

        public float MovementSpeed;
        [SerializeField] BoxCollider pInRange, pOnPlatform;
        [SerializeField] GameObject b1, b2;

        bool inOperation = false;
        bool isActive = false;
        AudioSource audioSource;

        public event System.Action<bool> OnIsActiveChange;


         public override void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            base.Awake();
        }

        public override void OnEnergyChange(byte newEnergy)
        {
            if (inOperation)
            {
                operationsQueue.Enqueue(delegate { OnEnergyChange(newEnergy); });
                return;
            }


            if (platformEnergyPositions[newEnergy] != Vector3.zero)
            {
                inOperation = true;

                if (PlayerIsInRange())
                {
                    StartCoroutine(WaitForPlayer(newEnergy));
                }
                else
                {
                    StartCoroutine(MoveToCorrectPosition(newEnergy));
                }
            }
        }

        private bool PlayerIsInRange()
        {
            return pInRange.bounds.Contains(LevelHandler.GetPlayer().position);
        }

        private bool PlayerIsOnPlatform()
        {
            return pOnPlatform.bounds.Contains(LevelHandler.GetPlayer().position);
        }

        public bool IsActive
        {
            get
            {
                return isActive;
            }

            set
            {
                isActive = value;
                OnIsActiveChange?.Invoke(value);
            }
        }

        private IEnumerator WaitForPlayer(byte level)
        {
            audioSource.Stop();
            IsActive = true;
            while (true)
            {
                yield return null;

                if (operationsQueue.Count > 0)
                {
                    inOperation = false;
                    IsActive = false;
                    operationsQueue.Dequeue().Invoke();
                    yield break;
                }

                if (PlayerIsOnPlatform() || !PlayerIsInRange())
                {
                    StartCoroutine(MoveToCorrectPosition(level));
                    break;
                }
            }
        }

        private IEnumerator MoveToCorrectPosition(byte level)
        {
            if (Storage == null)
            {
                yield break;
            }
            IsActive = true;
            b1.SetActive(true);
            b2.SetActive(true);

            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }

            Vector3 target = platformEnergyPositions[level];
            Vector3 dir = (target- transform.position).normalized;

            while(Vector3.Distance(transform.position, target) > 0.3)
            {
                transform.position = transform.position + dir * MovementSpeed*Time.deltaTime*60;
                yield return null;
            }

            transform.position = target;

            inOperation = false;
            b1.SetActive(false);
            b2.SetActive(false);
            IsActive = false;
            
            

            if (operationsQueue.Count > 0)
            {
                operationsQueue.Dequeue().Invoke();
            }

            audioSource.Stop();
        }

        private void OnCollisionEnter(Collision collision)
        {
            collision.transform.SetParent(transform);
        }

        private void OnCollisionExit(Collision collision)
        {
            collision.transform.SetParent(null);
        }

        public override string[] GetExtraVisualizationElements()
        {
            return new string[]
            {
                (Storage!=null)? "Linked to " + Storage.name: "UNLINKED",
                "Moving: " + inOperation,
                "Moves to still do: " + operationsQueue.Count,
                "Player in Waiting Range: " + PlayerIsInRange()
            };
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
        }
    }
}
