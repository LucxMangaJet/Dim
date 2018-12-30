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

        void Start()
        {
            an = GetComponent<Animator>();
            controller = transform.parent.GetComponent<Dim.Player.PlayerController>();
            //transform.parent = null;
        }



        private void Update()
        {
            an.Play("1",0,Mathf.Clamp((controller.EnergyAmount/5),0,0.9999999f));
            transform.position = orient.position;
        }
    }
}