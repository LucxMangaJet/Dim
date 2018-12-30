using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dim {
    [RequireComponent(typeof(BoxCollider))]
    [RequireComponent(typeof(Rigidbody))]
    /////////////////////////////////////////////////
    /// [OUTDATED] Used to deal damage in the form of a Projectile.
    /////////////////////////////////////////////////
    public class EnergyBullet : MonoBehaviour {

        [SerializeField] float energyAmount;
        [SerializeField] float force;

        public void Start()
        {
            GetComponent<Rigidbody>().AddForce(force * transform.forward, ForceMode.Impulse);
        }


        private void OnCollisionEnter(Collision collision)
        {
            Destroy(gameObject);
    }
    }
}