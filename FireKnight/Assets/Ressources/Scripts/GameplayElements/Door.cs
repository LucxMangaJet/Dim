using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Dim.Interaction
{

    /////////////////////////////////////////////////
    /// Door component that opens dependent on a Storage.
    /////////////////////////////////////////////////
    [RequireComponent(typeof(BoxCollider))]
    public class Door : InteractionBase , Visualize.IExtraVisualization
    {
        [SerializeField] bool startOpen;
        [SerializeField] float openingRequiredEnergy;


        bool isOpen;
        bool inTransition;
        [SerializeField] BoxCollider boxCollider;
        Animator animator;
        AudioSource audioSource;

        private void Start()
        {
            animator = GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();

            if (startOpen)
            {
                isOpen = true;
                Open();
            }
        }

        public override void OnEnergyChange(byte newEnergy)
        {
           if(newEnergy >= openingRequiredEnergy)
            {
                if (!isOpen) {
                    if (inTransition)
                    {
                        operationsQueue.Enqueue(Open);
                    }
                    else
                    {
                        Open();
                    }
                }
            }
            else
            {
                if (isOpen)
                {
                    if (inTransition)
                    {
                        operationsQueue.Enqueue(Close);
                    }
                    else
                    {
                        Close();
                    }
                }
            }
        }



        private void Open()
        {
            isOpen = true;
            inTransition = true;
            Invoke("DisableCollider", 0.3f);
            animator.SetTrigger("Open");
            Invoke("PlaySound", 0.3f);
            inTransition = false;
            CheckQueue();
        }

        private void DisableCollider()
        {
            boxCollider.enabled = false;
        }


        private void PlaySound()
        {
            audioSource.Play();
        }

        private void Close()
        {
            isOpen = false;
            inTransition = true;
            boxCollider.enabled = true;
            animator.SetTrigger("Close");
            Invoke("PlaySound", 0.7f);
            inTransition = false;
            CheckQueue();
        }

     
        private void CheckQueue()
        {
            if (operationsQueue.Count > 0)
            {
                operationsQueue.Dequeue().Invoke();
            }
        }

        public new string[] GetExtraVisualizationElements()
        {
            return new string[]
            {
                "State: "+ ((isOpen)?"Open":"Closed"),
                (Storage!=null)? "Linked to " + Storage.name: "UNLINKED",
                "Queue: " + operationsQueue.Count
            };
        }

    }
}