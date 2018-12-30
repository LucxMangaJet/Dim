using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Dim.Player
{

    /////////////////////////////////////////////////
    /// Used to make the camera change mode,position and perspective.
    /////////////////////////////////////////////////
    [ExecuteInEditMode]
    public class CameraTransitionTrigger : MonoBehaviour
    {

        [Header("General")]
        public readonly bool debug = false;
        public bool FollowPlayer;
        public bool IsSetup;
        public bool InEditing;
        public List<GameObject> editingObjects;


        public Vector3 Rotation;
        public Vector3 StaticPos;
        public Vector3 FollowRelativePos;

        private void Awake()
        {
            editingObjects = new List<GameObject>();

        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                CameraBehavior camB = Dim.LevelHandler.GetCamera().GetComponent<CameraBehavior>();
                if (!IsSetup)
                {
                    Debug.LogError("This Camera Transition is not setup.");
                    return;
                }

                if (FollowPlayer)
                {
                    camB.SetToPlayerFollow(FollowRelativePos, Rotation);
                }
                else
                {
                    camB.SetToStatic(StaticPos, Rotation);
                }

            }
        }
    }
}