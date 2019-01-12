using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dim.Enemies {

    /////////////////////////////////////////////////
    /// Responsible for the Behavior of a RhinoTrain.
    /////////////////////////////////////////////////
    public class RhinoTrainController : MonoBehaviour, Visualize.IExtraVisualization {

        public Interaction.RhinoTrainBase rhinoTrainBase;

        public float MovementCap;
        public float MovementForce;
        public float WaitTimeAfterPush;

        [HideInInspector] public Rigidbody rb;
        private RhinoTrainStateMachine sM;
        private float timestamp = -10;
        bool isDestroyed = false;
        [HideInInspector] public bool MoveDirectionIsRight;
        [HideInInspector] public GroundChecker groundChecker;
        Animator animator;
        AudioSource source;
        [SerializeField] AudioClip chargeStartClip, walkBackStartClip;

        private void Start()
        {
            transform.position = rhinoTrainBase.transform.position;
            rb = GetComponent<Rigidbody>();
            animator = GetComponent<Animator>();
            source = GetComponent<AudioSource>();

            groundChecker = GetComponentInChildren<GroundChecker>();
            sM = new RhinoTrainStateMachine(this);
            rhinoTrainBase.OnSoundHeard += CatchActivation;
            //SendMessage("GetFSM", sM);

            if(rhinoTrainBase.target.x > rhinoTrainBase.transform.position.x)
            {
                transform.eulerAngles = new Vector3(0, 90, 0);
                MoveDirectionIsRight = true;
            }
            else
            {
                transform.eulerAngles = new Vector3(0, -90, 0);
                MoveDirectionIsRight = false;
            }
        }

        private void CatchActivation()
        {
            timestamp = Time.timeSinceLevelLoad;
        }

        public  bool ShouldActivate()
        {
            if (Time.timeSinceLevelLoad - timestamp < 0.2)
            {
                return true;
            }
            return false;
        }

        public void SetAnimatorWalk(bool active)
        {
            animator.SetBool("isWalking", active);
        }

        public void SetAnimatorWalkBack(bool active)
        {
            animator.SetBool("isWalkingBack", active);
        }

        public void PlayChargeSound()
        {
            source.clip = chargeStartClip;
            source.loop = false;
            source.Play();
        }

        public void PlayWalkBackSound()
        {
            source.clip = walkBackStartClip;
            source.loop = true;
            source.Play();
        }

        public void StopSound()
        {
            source.Stop();
        }

        void Update()
        {
            if (!isDestroyed)
            {
                sM.Update();
            }
        }

        public string[] GetExtraVisualizationElements()
        {
            return new string[] {
                    sM.CurrentState.ToString()
            };
        }
    }
}
