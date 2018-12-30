using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dim.Player;

namespace Dim.Interaction
{

    /////////////////////////////////////////////////
    /// Deals with Transporting the Player through a PorterPipe System.
    /////////////////////////////////////////////////
    public class PorterPipeEntrance : InteractionBase, Dim.Visualize.IExtraVisualization
    {

        [Header("Porter Pipe")]
        public string portName;
        [HideInInspector] public Vector3 arrivingPosition;
        [SerializeField] float speed;
        [SerializeField] bool StartingMovingDirectionIsForward;

        [Header("'End' as element nr 2")]
        [Header("To go with energy level 2 to 'End' add ")]
        [SerializeField] string[] portNameConnections;

        [Header("Camera Settings")]
        [SerializeField] float camDistZ;
        [SerializeField] Vector3 camRotation;
        [SerializeField] bool camFreezeZTo0;
       

        bool playerInArea;
        bool isEnabled = true;
        Transform player;
        PorterPipeSection startingSection;

        public event System.Action<bool> OnStateChange;

        public  void Start()
        {
            player = LevelHandler.GetPlayer();

            arrivingPosition = new Vector3(transform.position.x, transform.position.y, 0);

            startingSection = GetComponent<PorterPipeSection>();

        }

        public override void OnEnergyChange(byte newEnergy)
        {
            OnStateChange?.Invoke(ShouldGoThroughPipe(newEnergy));
        }

        void Update()
        {
            if (playerInArea && isEnabled)
            {
                if (InputController.GetReleaseEnergy(InputStateType.PRESSED))
                {
                    if (ShouldGoThroughPipe(Storage.Energy))
                    {
                        StartCoroutine(TransitionThroughTube(Storage.Energy));
                        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraBehavior>().SetToPorterPipe(camDistZ, camFreezeZTo0, camRotation);
                        playerInArea = false;
                    } 
                }
            }
        }


        private bool ShouldGoThroughPipe(byte target)
        {
            if (target >= portNameConnections.Length)
            {
                Debug.LogError("This Pipes connection doesnt reach Energy Level " + target);
                return false;
            }

            if(portNameConnections[target] == "")
            {
                return false;
            }

            return true;
        }

        private IEnumerator TransitionThroughTube(byte target)
        {

            string targetName = portNameConnections[target];


            bool currentMovingDirectionIsForward = StartingMovingDirectionIsForward;
            PorterPipeSection currentSection = startingSection;
            PorterPipeSection nextSection;
            player.GetComponent<PlayerDeactivationHandler>().Dectivate();

            while(ShouldContinue(currentSection, targetName))
            {
                
                nextSection = currentSection.GetNextSection(currentMovingDirectionIsForward);

                if (nextSection.IsBlocked())
                {
                    currentMovingDirectionIsForward = !currentMovingDirectionIsForward;
                    nextSection = currentSection.GetNextSection(currentMovingDirectionIsForward);
                }


                float t = 0;
                while (t < 1)
                {
                    player.transform.position = Vector3.Lerp(currentSection.transform.position, nextSection.transform.position, t);
                    yield return null;
                    t += Time.deltaTime*speed;
                }

                currentSection = nextSection;
            }

            player.transform.position = currentSection.GetComponent<PorterPipeEntrance>().arrivingPosition;
            player.GetComponent<PlayerDeactivationHandler>().Activate();

        }

        private bool ShouldContinue(PorterPipeSection currentSection, string target)
        {
            if(currentSection.isEntrance)
            {
                if (currentSection.GetComponent<PorterPipeEntrance>().portName == target)
                {
                    return false;
                }
            }

            return true;
        }



        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                playerInArea = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag == "Player")
            {
                playerInArea = false;
            }
        }


   

        new public string[] GetExtraVisualizationElements()
        {
            return new string[]
            {
                (isEnabled)?"LocationName: "+ portName: "DISABLED",
                (playerInArea)?"Player in use Area":"",
                (Storage!=null)? "Linked to " + Storage.name: "UNLINKED"
            };
        }
    }
}
