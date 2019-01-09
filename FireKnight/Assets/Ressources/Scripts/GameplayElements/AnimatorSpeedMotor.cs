using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dim.Interaction { 

public class AnimatorSpeedMotor : InteractionBase {

    [SerializeField] byte activationMinEnergy;

    [SerializeField] float timeToToggle;
    [SerializeField] float highSpeed;
    [SerializeField] float lowSpeed;

    bool active;
    Animator animator;
    Coroutine coroutine;

    private void Start()
    {
            animator = GetComponent<Animator>();
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
                coroutine = StartCoroutine(ToggleSpeed(true));

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
                coroutine = StartCoroutine(ToggleSpeed(false));
            }
            active = false;
        }
    }


    private IEnumerator ToggleSpeed(bool on)
    {
        float nowSpeed =animator.speed;
        float targetSpeed = lowSpeed;
        if (on)
        {
            targetSpeed = highSpeed;
        }
        
        float counter = 0;

        while (counter < timeToToggle)
        {
            animator.speed = Mathf.Lerp(nowSpeed, targetSpeed, counter / timeToToggle);

            yield return null;
            counter += Time.deltaTime;
        }

            animator.speed = targetSpeed;
        coroutine = null;
    }

}
}

