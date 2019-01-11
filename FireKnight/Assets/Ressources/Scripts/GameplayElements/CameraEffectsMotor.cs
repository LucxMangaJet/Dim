using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dim.Interaction
{
    public class CameraEffectsMotor : InteractionBase
    {

        [SerializeField] byte activationMinEnergy;

        [SerializeField] bool cameraShakeOnIncrease, cameraShakeOnDecrease;
        [SerializeField] float shakeLength, shakeIntensity;

        bool active;
        Player.CameraBehavior cam;
        float startingIntensity;
        Coroutine coroutine;

        private void Start()
        {
            cam = LevelHandler.GetCamera().GetComponent<Player.CameraBehavior>();
        }

        public override void OnEnergyChange(byte newEnergy)
        {
            if (newEnergy >= activationMinEnergy)
            {
                if (!active)
                {
                    if (Time.time < 2)
                    {
                        active = true;
                        return;
                    }

                    if(cameraShakeOnIncrease)
                    ShakeCam();


                    active = true;
                }
            }
            else
            {
                if (active)
                {
                    if(cameraShakeOnDecrease)
                    ShakeCam();
                }
                active = false;
            }
        }


        private void ShakeCam()
        {
            cam.StartCameraShake(shakeLength, shakeIntensity);
        }

    }

}