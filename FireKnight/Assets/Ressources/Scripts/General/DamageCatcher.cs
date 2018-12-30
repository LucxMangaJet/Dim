using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dim {

    /////////////////////////////////////////////////
    /// Checks Collision with damage dealing triggers, then calls damage handling script.
    /////////////////////////////////////////////////
    public class DamageCatcher : MonoBehaviour {

        IGameObjectDamageTaker control;

        private void Start()
        {
            control = GetComponent<IGameObjectDamageTaker>();
            if(control == null)
            {
                throw new System.Exception("This GameObject doesnt have a IGameObjectDamageTaker and can thus not recieve physical damage.");
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            
            if(other.tag == "PhysicalDamage")
            {
                if (other.GetComponent<PhysicalDamageDealer>().ShouldDealDamage(transform))
                {
                    Debug.Log("Apllying physical damage to " + gameObject.name);
                    control.TakeDamage();
                }
            }
        }
    }
}
