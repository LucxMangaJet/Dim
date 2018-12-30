using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dim.Interaction;

namespace Dim.Visualize {

    /////////////////////////////////////////////////
    /// View for PorterPipe Entrances, turns a light on/off to visualize operability.
    /////////////////////////////////////////////////
    public class PorterPipeEntranceLook : MonoBehaviour {

        Light light;
        float intensityGoal;
        float intensity;
        PorterPipeEntrance pipeEntrance;

        private void Awake()
        {
            pipeEntrance = GetComponent<PorterPipeEntrance>();
            pipeEntrance.OnStateChange += CatchTargetChange;
        }

        private void CatchTargetChange(bool active)
        {
            if (active)
            {
                Acivate();
            }
            else
            {
                Deactive();
            }
        }

        void Start() {
            light = GetComponent<Light>();
        }

        void FixedUpdate()
        {
            intensity = light.intensity;
            if (intensity != intensityGoal)
            {
                if (intensity < intensityGoal)
                {
                    intensity++;
                    if (intensity > intensityGoal)
                    {
                        intensity = intensityGoal;
                    }
                }
                else
                {
                    intensity--;
                    if (intensity < intensityGoal)
                    {
                        intensity = intensityGoal;
                    }
                }

                light.intensity = intensity;
            }
        }

        public void Acivate()
        {
            intensityGoal = 30;
        }

        public void Deactive()
        {
            intensityGoal = 0;
        }
    }
}