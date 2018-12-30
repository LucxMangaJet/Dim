using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dim.Enemies {

    /////////////////////////////////////////////////
    /// Responsible for the Behavior of a RhinoTrain.
    /////////////////////////////////////////////////
    public class RhinoTrainController : MonoBehaviour {

        public Interaction.RhinoTrainBase rhinoTrainBase;

        public float MovementCap;
        public float MovementForce;
        public float WaitTimeAfterPush;

        [HideInInspector] public Rigidbody rb;
        private RhinoTrainStateMachine sM;
        private float timestamp= -10;
        bool isDestroyed = false;
        [HideInInspector] public bool MoveDirectionIsRight; 
        [HideInInspector] public GroundChecker groundChecker;

        private void Start()
        {
            transform.position = rhinoTrainBase.transform.position;
            rb = GetComponent<Rigidbody>();
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
            timestamp = Time.time;
        }

        public  bool ShouldActivate()
        {
            if (Time.time - timestamp < 0.2)
            {
                return true;
            }
            return false;
        }


        void Update()
        {
            if (!isDestroyed)
            {
                sM.Update();
            }
        }




    }
}
