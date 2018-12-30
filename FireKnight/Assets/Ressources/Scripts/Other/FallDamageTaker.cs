using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dim {
    public class FallDamageTaker : MonoBehaviour {

        [SerializeField] float requiredVerticalSpeed;
        IGameObjectDamageTaker damageTaker;

        private void Start()
        {
            damageTaker = GetComponent<IGameObjectDamageTaker>();
            if(damageTaker == null)
            {
                Debug.LogError("FallDamageTaker is on a GameObject that cannot take damage.");
                Destroy(this);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (Mathf.Abs(collision.relativeVelocity.y) > requiredVerticalSpeed)
            {
                if(tag == "Player")
                {
                    Player.PlayerDestructionHandler.extraForceTimeStamp = Time.time;
                    Player.PlayerDestructionHandler.extraForce = -collision.relativeVelocity;
                }

                GetComponent<IGameObjectDamageTaker>().TakeDamage();
            }
        }
    }
}