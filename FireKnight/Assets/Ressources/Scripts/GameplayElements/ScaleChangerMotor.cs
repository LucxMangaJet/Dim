using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dim.Interaction
{
    public class ScaleChangerMotor : InteractionBase
    {
        [SerializeField] byte activationMinEnergy;

        [SerializeField] float timeToToggle;
        [SerializeField] Vector3 SizeDifference;

        bool active;
        Coroutine coroutine;


        public override void OnEnergyChange(byte newEnergy)
        {
            

            if (newEnergy >= activationMinEnergy)
            {
                if (!active)
                {
                    if (Time.timeSinceLevelLoad < 2)
                    {
                        active = true;
                        return;
                    }

                    if (coroutine != null)
                    {
                        StopCoroutine(coroutine);
                    }
                    coroutine = StartCoroutine(ChangeSize(true));

                    active = true;
                }
            }
            else
            {
                if (active)
                {
                    if (coroutine != null)
                    {
                        StopCoroutine(coroutine);
                    }
                    coroutine = StartCoroutine(ChangeSize(false));
                }
                active = false;
            }
        }


        private IEnumerator ChangeSize(bool on)
        {
            
            Vector3 nowSize = transform.localScale;
            Vector3 targetSize = nowSize - SizeDifference;
            if (on)
            {
                targetSize = nowSize + SizeDifference;
            }

            float counter = 0;

            while (counter < timeToToggle)
            {
                transform.localScale = Vector3.Lerp(nowSize, targetSize, counter / timeToToggle);

                yield return null;
                counter += Time.deltaTime;
            }

            transform.localScale = targetSize;
            coroutine = null;
        }

    }

}
