using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Dim.Visualize
{
    /////////////////////////////////////////////////
    /// Basic Behavior to visualize the players energy carges.
    /////////////////////////////////////////////////
    public class ParticleChargeLookRotation : MonoBehaviour
    {

        [SerializeField] Vector3 rotation;
        float speedMultiplier;
        void Start()
        {
            speedMultiplier = Random.Range(0.8f, 1.2f);
            transform.localPosition = Vector3.up * Random.Range(-0.25f, 0.25f);
        }


        // Update is called once per frame
        void Update()
        {
            transform.Rotate(rotation * speedMultiplier*Time.deltaTime*60);
        }
    }
}