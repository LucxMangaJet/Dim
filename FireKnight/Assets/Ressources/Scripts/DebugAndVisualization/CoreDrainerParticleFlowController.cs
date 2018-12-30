using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dim.Visualize
{

    /////////////////////////////////////////////////
    /// Visualizes the Draining of the Player by the CoreDrainer.
    /////////////////////////////////////////////////
    public class CoreDrainerParticleFlowController : MonoBehaviour
    {

        ParticleSystem ownPS;
        Transform playerTransform;
        Transform drainerTransform;
        float particleForce;
        public bool Active = false;

        // Use this for initialization
        void Start()
        {
            ownPS = GetComponent<ParticleSystem>();
            playerTransform = GameObject.FindWithTag("Player").transform;
            drainerTransform = transform.parent.transform;
            drainerTransform.GetComponent<Enemies.CoreDrainerController>().DrainFlow = this;
            transform.parent = null;
            transform.position = Vector3.zero;
            transform.rotation = Quaternion.identity;
            transform.localScale = Vector3.one;
            
        }

        // Update is called once per frame
        void Update()
        {

            float distance = Vector3.Distance(drainerTransform.position, playerTransform.position);

            var emissionModule = ownPS.emission;

            if (distance < 5 && Active)
            {
                Debug.DrawLine(playerTransform.position, drainerTransform.position + new Vector3(0,1,0));

                //get the shape module and store it in a variable for later use
                var shapeModule = ownPS.shape;

                //position the point of emission right were the player should be
                shapeModule.position = playerTransform.position;

                //calculate Directional Vector
                Vector3 particleDirection = (drainerTransform.position + new Vector3(0, 1, 0) - playerTransform.position);

                //use the direction of the emission shape to give the particles speed in a certain direction
                shapeModule.rotation = Quaternion.LookRotation(particleDirection).eulerAngles;
                var mainModule = ownPS.main;

                //ensure that the partcicles always reach their goal and the die.
                mainModule.startLifetime = distance / mainModule.startSpeed.constant;

                //use force over Lifetime to push the particles to a certain direction
                /*
                particleForce = 100;
                particleDirection = particleDirection.normalized * particleForce;

                var forceOverLifetimeModule = ownPS.forceOverLifetime;
                forceOverLifetimeModule.x = particleDirection.x;
                forceOverLifetimeModule.y = particleDirection.y;
                forceOverLifetimeModule.z = particleDirection.z;
                */

                emissionModule.rateOverTime = 10;
            }
            else
            {
                emissionModule.rateOverTime = 0;
            }
        }
    }
}