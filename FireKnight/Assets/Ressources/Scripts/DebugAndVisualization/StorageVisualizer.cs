using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dim.Interaction;

namespace Dim.Visualize
{
    /////////////////////////////////////////////////
    /// Visualizes Linked Storages using a ParticleSystem
    /////////////////////////////////////////////////
    public class StorageVisualizer : MonoBehaviour
    {
        [SerializeField] GameObject[] lights;
        Vector3 visualCenterOffset;
        [SerializeField] GameObject linkedStorageVisualizer;
        Storage storage;

        private void Awake()
        {
            storage = GetComponent<Storage>();

            storage.OnEnergyChange += SetLights;
        }

        private void Start()
        {
            visualCenterOffset = Vector3.up;
            if (storage.LinkedStorages  != null)
            {

                foreach (Storage s in storage.LinkedStorages)
                {
                    GameObject g = Instantiate(linkedStorageVisualizer, transform.position + visualCenterOffset, Quaternion.identity);
                    ParticleSystem ps = g.GetComponent<ParticleSystem>();

                    float distance = Vector3.Distance(transform.position, s.transform.position);

                    var emissionModule = ps.emission;

                    Debug.DrawLine(transform.position + visualCenterOffset, s.transform.position + visualCenterOffset);

                    //get the shape module and store it in a variable for later use
                    var shapeModule = ps.shape;
                    //shapeModule.position = transform.position ;

                    //calculate Directional Vector
                    Vector3 particleDirection = (s.transform.position - transform.position);

                    //use the direction of the emission shape to give the particles speed in a certain direction
                    shapeModule.rotation = Quaternion.LookRotation(particleDirection).eulerAngles;
                    var mainModule = ps.main;

                    //ensure that the partcicles always reach their goal and the die.
                    mainModule.startLifetime = distance / mainModule.startSpeed.constant;
                }
            }

        }

        private void SetLights(byte NewEnergy)
        {
            for (int i = 0; i < lights.Length; i++)
            {
                if (NewEnergy >= i + 1)
                {
                    lights[i].SetActive(true);
                }
                else
                {
                    lights[i].SetActive(false);
                }
            }
        }
    }
}