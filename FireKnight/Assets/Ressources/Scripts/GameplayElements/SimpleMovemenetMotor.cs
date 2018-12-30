using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

//OUTDATED
namespace Dim.Interaction{

    /////////////////////////////////////////////////
    /// [OUTDATED] Used to move Visual Assets based on Storage Energy Levels
    /////////////////////////////////////////////////
    public class SimpleMovemenetMotor : MonoBehaviour ,Dim.Visualize.IExtraVisualization {

        [SerializeField] bool doesMove, doesRotate, doesLoop;
        [SerializeField] bool useVectorsOverTransforms;

        [Header("If Does Loop")]
        [SerializeField] MotorLoopingType loopType;
        [SerializeField] int howManyLoops;

        [Header("If Use Transforms")]
        [SerializeField] Transform startingTransform;
        [SerializeField] Transform endingTransform;

        [Header("If Use Vectors")]
        [SerializeField] bool startIsScenePosition;
        [SerializeField] Vector3 startingPos;
        [SerializeField] Vector3 endingPos;
        [SerializeField] Vector3 startingRot, endingRot;

        [Space(20)]
        [SerializeField] bool alwaysActive;
        [SerializeField] GameObject storageModuleObject;
        [SerializeField] float activationThreshold;
        [SerializeField] float speedInPercentPerSec;
        [SerializeField] bool requiresConstantEnergy;
        [Header("If Requires Constant Energy")]
        [SerializeField] float energyDrainedPerSec;
        [Header("If One Time Activation")]
        [SerializeField] float oneTimeEnergyCost;

        [Header("Sounds")]
        [SerializeField] bool playSoundOnStart;
        [SerializeField] bool playSoundOnEnd, playSoundWhileMoving;
        [SerializeField] AudioClip startSound, endSound, movingSound;


        float currentCompletionPercentage=0;
        float directionMultiplyer = 1;
        int currentLoops=0;
        AudioSource audioSource;

        private void Start()
        {
            audioSource = GetComponent<AudioSource>();

            if (startIsScenePosition)
            {
                startingPos = transform.position;
                startingRot = transform.localEulerAngles;
            }

            if (alwaysActive)
            {
               
                requiresConstantEnergy = false;
                StartCoroutine(ActivateAndFunctionMotor());
            }
            else
            {

            }
        }


        private void Enable()
        {
            if (!requiresConstantEnergy)
            {
                
            }

            StartCoroutine(ActivateAndFunctionMotor());

        }

        private IEnumerator ActivateAndFunctionMotor()
        {
            if (playSoundOnStart)
            {
                audioSource.clip = startSound;
                audioSource.loop = false;
                audioSource.Play();
            }

            bool repeat = true;
            while (repeat)
            {
                

                while (currentCompletionPercentage <= 1 && currentCompletionPercentage >=0)
                {
                    if (playSoundWhileMoving && !audioSource.isPlaying)
                    {
                        audioSource.clip = movingSound;
                        audioSource.loop = true;
                        audioSource.Play();
                    }

                    if (doesMove)
                    {
                        if (useVectorsOverTransforms)
                        {
                            transform.position = Vector3.Lerp(startingPos, endingPos, currentCompletionPercentage);
                        }
                        else
                        {
                            transform.position = Vector3.Lerp(startingTransform.position, endingTransform.position, currentCompletionPercentage);
                        }
                    }

                    if (doesRotate)
                    {
                        if (useVectorsOverTransforms)
                        {
                            transform.localEulerAngles = GlobalMethods.LerpAngle(startingRot, endingRot, currentCompletionPercentage);
                        }
                        else
                        {
                            transform.localEulerAngles = GlobalMethods.LerpAngle(startingTransform.localEulerAngles, endingTransform.localEulerAngles, currentCompletionPercentage);
                        }
                    }

                    yield return null;

                    currentCompletionPercentage += speedInPercentPerSec * directionMultiplyer * Time.deltaTime;

                    if (requiresConstantEnergy)
                    {
                        float energyToDrain = energyDrainedPerSec * Time.deltaTime;

                    }
                }

                repeat = IsRepetitionRequired();
            }

            if (playSoundOnEnd)
            {
                audioSource.clip = endSound;
                audioSource.loop = false;
                audioSource.Play();
            }

        }


        private bool IsRepetitionRequired()
        {
            if (doesLoop && (currentLoops <= howManyLoops || howManyLoops<=0))
            {
                currentLoops++;

                if (loopType == MotorLoopingType.PingPong)
                {
                    if(directionMultiplyer > 0)
                    {
                        currentCompletionPercentage = 1;
                    }
                    else
                    {
                        currentCompletionPercentage = 0;
                    }
                    directionMultiplyer *= -1;
                }
                else
                {
                    currentCompletionPercentage = 0;
                }

                return true;
            }

            return false;
        }

        public string[] GetExtraVisualizationElements()
        {
            return new string[]
            {
              "Movement Percent: " +  currentCompletionPercentage,
               (doesLoop)?"Loops: "+currentLoops.ToString():""
            };
        }

        public enum MotorLoopingType
        {
            PingPong,
            Repeat
        }

    }
}
