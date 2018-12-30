using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dim.Enemies
{

    ///////////////////////////
    /// Used for the CoreDrainer, absorbs EnergyAreas into the CoreDrainer.
    //////////////////////////
    public class CoreDrainerAbsorbtionAreaHandler : MonoBehaviour
    {

        CoreDrainerController controller;

        void Start()
        {
            controller = transform.parent.GetComponent<CoreDrainerController>();
        }


        private void OnTriggerEnter(Collider other)
        {
            if(other.tag == "HeatArea")
            {
                other.GetComponent<EnergyArea>().RemoveEnergy();
                controller.Energy += 1;
            }
        }

    }

}