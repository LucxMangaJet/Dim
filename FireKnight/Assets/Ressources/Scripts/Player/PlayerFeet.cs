using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dim.Player
{
    /////////////////////////////////////////////////
    /// Connects to the PlayerController, used to understand if the Player is colliding with the ground.
    /////////////////////////////////////////////////
    public class PlayerFeet : MonoBehaviour {

        private void OnTriggerStay(Collider other)
        {
            if (other.tag != "Player" && !other.isTrigger)
            {
                transform.parent.GetComponent<PlayerController>().OnGround =true;
            }
        }
    }
}