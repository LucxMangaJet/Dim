using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//OUTDATED
namespace Dim.Interaction {

    /////////////////////////////////////////////////
    /// [OUTDATED] Used to created gating withing PorterPipes.
    /////////////////////////////////////////////////
    public class PorterPipeBlocker : InteractionBase {

        [SerializeField] Vector3 unBlockedRotation;
        [SerializeField] float speedPerSec;
        [SerializeField] float threshhold;

        Vector3 startRot;

        public void Start()
        {
            startRot = transform.eulerAngles;

        }

        public override void OnEnergyChange(byte newEnergy)
        {
            if(newEnergy>= threshhold)
            {
                StartCoroutine(MoveToUnBlocked());
            }
        }

        private IEnumerator MoveToUnBlocked()
        {
            float t = 0;

            while (t <= 1)
            {
                transform.eulerAngles = Dim.GlobalMethods.LerpAngle(startRot, unBlockedRotation, t);
                yield return null;
                t += Time.deltaTime * speedPerSec;
            }

            transform.eulerAngles = unBlockedRotation;
            GetComponent<PorterPipeSection>().isBlocked = false;
        }


    }
}