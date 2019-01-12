using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dim
{

    /////////////////////////////////////////////////
    /// EnergyObject that is spawned both by the Player when Emitting freely and by Enemies on Death.
    /////////////////////////////////////////////////

    [RequireComponent(typeof(BoxCollider))]
    [RequireComponent(typeof(ParticleSystem))]
    public class EnergyArea : MonoBehaviour , IEnergyObject , Visualize.IExtraVisualization
    {

        public static float NOISE_MULTIPLYER = 1.5f;
        static float LIFETIME = 6;
        private float timestamp;

        public byte Energy = 0;
        public float Size = 0;

        private BoxCollider bC;
        private ParticleSystem pS;

        private void Awake()
        {
            bC = GetComponent<BoxCollider>();
            pS = GetComponent<ParticleSystem>();
            timestamp = Time.timeSinceLevelLoad;
        }

        private void Return()
        {
            LevelHandler.GetPlayer().GetComponent<Player.PlayerController>().EnergyAmount += Energy;
            EnergyHandler.RemoveEnergyObject(this);
            Debug.Log("Returning Energy to player");
            Destroy(gameObject);
        }

        public void UpdateField()
        {
            ResizeArea();
            UpdateParticleSystem();
            if (Energy <= 0)
            {
                EnergyHandler.RemoveEnergyObject(this);
                Destroy(gameObject);
            }
        }

        public void AddEnergy(bool returnToPlayer)
        {
            Energy++;

            if (returnToPlayer)
            {
                Invoke("Return", LIFETIME);
            }

            UpdateField();
        }

        public void RemoveEnergy()
        {
            Energy--;
            UpdateField();   
        }

        private void ResizeArea()
        {

           //size is now defined with noise -> affects the area the system moves within
           float noise = Math.Max(1, (float)Math.Log10(Energy) * NOISE_MULTIPLYER);
           var pSNM = pS.noise;
           pSNM.strength = noise * 0.1f;
           Size = noise;
          
        }

        private void UpdateParticleSystem()
        {
            var psm = pS.main;
            var pse = pS.emission;

            //psm.startSpeed = 0;
            //psm.startLifetime = 5 * Energy;
            //pse.rateOverTime = 10*Energy;
        }

        public float GetEnergyAmount()
        {
            return Energy;
        }

        public Transform GetTransform()
        {
            return transform;
        }

        public string[] GetExtraVisualizationElements()
        {
            return new string[]
            {
                "Energy: " + Energy,
                "TimeLeft:" + (LIFETIME - (Time.timeSinceLevelLoad-timestamp))
            };
        }
    }
}
