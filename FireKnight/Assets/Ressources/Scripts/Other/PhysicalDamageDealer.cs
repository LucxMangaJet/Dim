using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Dim
{

    /////////////////////////////////////////////////
    /// Used to deal damage.
    /////////////////////////////////////////////////
    public class PhysicalDamageDealer : MonoBehaviour, Visualize.IExtraVisualization
    {

        [SerializeField] bool alwaysDealDamage;

        [SerializeField] bool hasRigidBody;
        [SerializeField] bool verticalMovementOnly;
        [SerializeField] float requiredForce;

        Rigidbody rb;
        private void Start()
        {
            if (hasRigidBody)
            {
                rb = GetComponent<Rigidbody>();
            }
        }

        public bool ShouldDealDamage(Transform target)
        {
            if (target == transform)
            {
                return false;
            }

            if (alwaysDealDamage)
            {
                return true;
            }
            if (verticalMovementOnly)
            {
                if (Mathf.Abs(rb.velocity.y) > requiredForce)
                {
                    return true;
                }
            }
            else
            {
                if (rb.velocity.magnitude > requiredForce)
                {
                    return true;
                }
            }

            return false;

        }

        public string[] GetExtraVisualizationElements()
        {
            if (hasRigidBody)
            {
                return new string[]
                {
                    "Damage Force: " + ((verticalMovementOnly)?Mathf.Abs(rb.velocity.y):rb.velocity.magnitude)
                };
            }
            else if (alwaysDealDamage)
            {
                return new string[]
                {
                    "Touch Damage"
                };
            }
            return null;

        }
    }
}
