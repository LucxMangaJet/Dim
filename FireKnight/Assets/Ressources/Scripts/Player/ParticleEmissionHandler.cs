using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dim.Visualize 
{
    /////////////////////////////////////////////////
    /// Handles the Emission of Particles in the chest of the Player.
    /////////////////////////////////////////////////
    public class ParticleEmissionHandler : MonoBehaviour
    {

        Animator an;
        Dim.Player.PlayerController controller;
        [SerializeField] Transform orient;
        [Range(0,1)]
        [SerializeField] float intensity;

        [SerializeField] bool deactivate;
        void Start()
        {
            an = GetComponent<Animator>();
            controller = transform.parent.GetComponent<Dim.Player.PlayerController>();
            //transform.parent = null;
        }



        private void Update()
        {
            if (!deactivate)
            {
                an.Play("1", 0, intensity);
            }
            transform.position = orient.position;
        }
    }
}