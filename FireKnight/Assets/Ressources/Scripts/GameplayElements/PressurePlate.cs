using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dim.Interaction
{

    /////////////////////////////////////////////////
    /// Behavior Script for Pressure Plate, simulates being a Storage to activate interactions.
    /////////////////////////////////////////////////
    public class PressurePlate : MonoBehaviour
    {

        public InteractionBase Interaction;

        [SerializeField] byte StepOnSimulatedEnergy;
        [SerializeField] byte StepOffSimulatedEnergy;
        [SerializeField] float UnPressTimeDelay;

        Animator anim;
        Material mat;

        void Start ()
        {
            anim = GetComponent<Animator>();
            mat = GetComponent<Renderer>().material;
            mat.DisableKeyword("_EMISSION");

        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.GetComponent<IEnergyObject>() !=null)
            {
                Interaction.OnEnergyChange(StepOnSimulatedEnergy);
                StopAllCoroutines();
                anim.SetBool("active",true);
                mat.EnableKeyword("_EMISSION");
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<IEnergyObject>() != null)
            {
                StartCoroutine(Deactivate());
            }
        }

        private IEnumerator Deactivate()
        {
            yield return new WaitForSeconds(UnPressTimeDelay);
            Interaction.OnEnergyChange(StepOffSimulatedEnergy);
            anim.SetBool("active", false);
            mat.DisableKeyword ("_EMISSION");
        }

    }
}
