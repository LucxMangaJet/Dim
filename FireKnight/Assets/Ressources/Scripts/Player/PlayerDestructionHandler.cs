using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Dim.Player
{
    /////////////////////////////////////////////////
    /// Responsible for Playing the death animation (spawning a destructable version of the Player).
    /////////////////////////////////////////////////
    public class PlayerDestructionHandler : MonoBehaviour
    {
        public static Vector3 extraForce;
        public static float extraForceTimeStamp;

        [SerializeField] GameObject DestroyedPlayer;

        public void DestroyPlayer(Vector3 velocity)
        {

            GameObject g = Instantiate(DestroyedPlayer, transform.position + new Vector3(0, -0.75f, 0), transform.rotation);
            g.transform.localScale = transform.lossyScale * 3;

            if(Time.time-extraForceTimeStamp < 1)
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