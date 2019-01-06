using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Dim
{
    /////////////////////////////////////////////////
    /// Responsible for Playing the death animation (spawning a destructable version of the Player, PickGoat etc).
    /////////////////////////////////////////////////
    public class VisualDestructionHandler : MonoBehaviour
    {
        public static Vector3 extraForce;
        public static float extraForceTimeStamp;
        public static GameObject extraForceTarget;

        [SerializeField] GameObject destroyedObject;
        [SerializeField] float scaleMultiplication;

        public void Destroy(Vector3 velocity)
        {
            GameObject g = Instantiate(destroyedObject, transform.position + new Vector3(0, -0.75f, 0), transform.rotation);
            g.transform.localScale = transform.lossyScale * scaleMultiplication;

            if(gameObject == extraForceTarget && Time.time-extraForceTimeStamp < 1)
            {
                velocity += extraForce;
            }

            foreach (var rb in g.GetComponentsInChildren<Rigidbody>())
            {
                rb.velocity = velocity;
            }

        }
    }
}