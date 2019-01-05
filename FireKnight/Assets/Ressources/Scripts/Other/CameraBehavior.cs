using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;


namespace Dim.Player
{

    /////////////////////////////////////////////////
    /// Handles movement of the main Camera, gets informations about new transitions from CameraTransitionTrigger.
    /////////////////////////////////////////////////
    [RequireComponent(typeof(Camera))]
    public class CameraBehavior : MonoBehaviour
    {

        [SerializeField] bool debug;

        [Header("Default Settings")]
        [SerializeField] float transitionSpeed;
        [SerializeField] float rotationSpeed;
        [SerializeField] float snapToDistance = 1f;
        [SerializeField] float snaptransitionSpeed = 0.4f;
        [SerializeField] float lockOnDistance = 0.1f;
        //[SerializeField] PostProcessingProfile postProcessing;


        public enum CamState { Static, PlayerFollow, PorterPipe, CutScene }

        private CamState state;
        private float camDistZ;
        Vector3 dispFromPlayer;
        bool zFreeze;

        Vector3 targetPos;
        Vector3 targetRot;

        private Transform player;
        private Camera cam;

        private bool isSnapped;

        //CutScenes
        Coroutine cutSceneCoroutine;
        public event System.Action OnCutSceneEnd;
        CamState oldState;

        void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
            cam = GetComponent<Camera>();

            SetToPlayerFollow(new Vector3(0, 0, -7), Vector3.zero);
        }




        void Update()
        {
            if (state != CamState.CutScene)
            {
                switch (state)
                {
                    case CamState.PlayerFollow:
                        targetPos = player.position + dispFromPlayer;                    
                        break;

                    case CamState.PorterPipe:
                        targetPos = GetCamPos(player.position, camDistZ, zFreeze);
                        break;
                }

                    MoveTowardsTarget();

            }
            else
            {
                if (InputController.GetJump(InputStateType.JUST_PRESSED))
                {
                    StopCoroutine(cutSceneCoroutine);
                    state = oldState;
                    OnCutSceneEnd?.Invoke();
                    if (debug)
                    {
                        Debug.Log("Cutscene cancelled by Player.");
                    }
                }
            }

        }

        private void UnSnap()
        {
            
            isSnapped = false;
        }

        private void MoveTowardsTarget()
        {
            if (isSnapped)
            {
                transform.position = targetPos;
            }
            else
            {
                float dist = Vector3.Distance(transform.position, targetPos);

                if(dist< lockOnDistance)
                {
                    isSnapped = true;
                }

                if (dist < snapToDistance)
                {
                    transform.position = Vector3.Lerp(transform.position, targetPos, snaptransitionSpeed);
                }
                else
                {
                    transform.position = Vector3.Lerp(transform.position, targetPos, transitionSpeed);
                }
            }
            
            transform.eulerAngles = Dim.GlobalMethods.LerpAngle(transform.eulerAngles, targetRot, rotationSpeed);
        }

        private Vector3 GetCamPos(Vector3 pPos, float zDist, bool zFreeze)
        {
            Vector3 pos = pPos;
            if (zFreeze)
            {
                pos.z = 0;
            }

            pos -= transform.forward * zDist;
            return pos;
        }


        public void SetToStatic(Vector3 pos, Vector3 rot)
        {
            state = CamState.Static;
            targetPos = pos;
            targetRot = rot;
            UnSnap();

        }

        public void SetToPlayerFollow(Vector3 displacement, Vector3 rot)
        {
            state = CamState.PlayerFollow;
            targetRot = rot;
            dispFromPlayer = displacement;
            UnSnap();

        }

        public void SetToPorterPipe(float zDist, bool freezeZ, Vector3 rot)
        {
            state = CamState.PorterPipe;
            zFreeze = freezeZ;
            targetRot = rot;
            camDistZ = zDist;
            UnSnap();
        }

        public void SetToCutScene(CutSceneTransition[] c)
        {
            cutSceneCoroutine = StartCoroutine(PlayCutScene(c));
            UnSnap();
        }

        
        public void RequestNewTarget()
        {
            RaycastHit[] hits = Physics.BoxCastAll(player.position, Vector3.one / 2, Vector3.up, Quaternion.identity, 1, int.MaxValue, QueryTriggerInteraction.Collide);
            foreach (var i in hits)
            {
                if (i.collider.tag == "CameraTransition")
                {
                    //GetTarget;
                    return;
                }
            }

        }


        private IEnumerator PlayCutScene(CutSceneTransition[] c)
        {
            oldState = state;
            state = CamState.CutScene;

            if (debug)
            {
                Debug.Log("Starting CutScene with " + c.Length + " Transitions.");
            }

            for (int i = 0; i < c.Length; i++)
            {
                Vector3 previousPosition;
                Vector3 previousRotation;
                float counter = Time.time;
                CutSceneTransition transition = c[i];

                if (i == 0)
                {
                    previousPosition = transform.position;
                    previousRotation = transform.eulerAngles;
                }
                else
                {
                    previousPosition = c[i - 1].GetPos();
                    previousRotation = c[i - 1].GetRot();
                }

                while (Time.time - counter < transition.TimeToComplete)
                {
                    transform.position = Vector3.Lerp(previousPosition, transition.GetPos(), (Time.time - counter) / transition.TimeToComplete);
                    transform.eulerAngles = GlobalMethods.LerpAngle(previousRotation, transition.GetRot(), (Time.time - counter) / transition.TimeToComplete);
                    yield return null;
                    if(debug)
                    Debug.Log("Transition #" + i + " TimeLeft: " + (transition.TimeToComplete - (Time.time - counter)));
                }
            }

            state = oldState;
            OnCutSceneEnd?.Invoke();

            if (debug)
            {
                Debug.Log("Ending Cutscene");
            }
        }

    }




    /////////////////////////////////////////////////
    /// Used to Create CutScenes played by the CameraBehavior.
    /////////////////////////////////////////////////
    [System.Serializable]
    public class CutSceneTransition
    {
        public State Type;
        public Vector3 Pos;
        public Vector3 Rot;
        public Transform FollowTarget;
        public float TimeToComplete;

        public enum State { Static, FollowTransform };


        public Vector3 GetPos()
        {
            if(Type == State.FollowTransform)
            {
                return FollowTarget.position + Pos;
            }
            else
            {
                return Pos;
            }
        }

        public Vector3 GetRot()
        {
            return Rot;
        }
    }
}
