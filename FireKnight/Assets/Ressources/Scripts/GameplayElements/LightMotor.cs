using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Dim.Interaction
{

    public class LightMotor : InteractionBase
    {
        [SerializeField] byte activationMinEnergy;

        [SerializeField] float timeToToggle;

        bool active;
        Light light;
        float startingIntensity;
        Coroutine coroutine;

        private void Start()
        {
            light = GetComponent<Light>();
            startingIntensity = GetComponent<Light>().intensity;
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
                    if (coroutine != null)
                    {
                        StopCoroutine(coroutine);
                    }
                    coroutine = StartCoroutine(ToggleLight(true));    
                    
                    active = true;
                }
            }
            else
            {
                if (active)
                {
                    if (coroutine!= null)
                    {
                        StopCoroutine(coroutine);
                    }
                    coroutine = StartCoroutine(ToggleLight(false));
                }
                active = false;
            }
        }


        private IEnumerator ToggleLight(bool on)
        {
            float targetIntensity = 0;
            float nowIntensity = GetComponent<Light>().intensity;

            if (on)
            {
                targetIntensity = startingIntensity;
            }

            float counter =0;

            while(counter < timeToToggle)
            {
                GetComponent<Light>().intensity = Mathf.Lerp(nowIntensity, targetIntensity, counter / timeToToggle);

                yield return null;
                counter += Time.deltaTime;
            }

            GetComponent<Light>().intensity = targetIntensity;
            coroutine = null;
        }

    }
}

