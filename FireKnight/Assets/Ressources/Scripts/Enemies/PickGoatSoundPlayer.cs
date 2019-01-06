using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dim.Enemies
{

    /////////////////////////////////////////////////
    /// Attached to Pick and Feet of PickGoat, emits sounds on Collision.
    /////////////////////////////////////////////////
    public class PickGoatSoundPlayer : MonoBehaviour, Visualize.IExtraVisualization
    {

    //    [SerializeField] private float Loudness = 5;
    //    [SerializeField] private float Range = 10;
        private float timeStamp;
        private float cooldown = 0.1f;

        AudioSource source;

        private void Start()
        {
            source = GetComponent<AudioSource>();
        }



        private void OnTriggerEnter(Collider other)
        {

            if (other.tag == "Untagged" && Time.time - timeStamp > cooldown)
            {
                timeStamp = Time.time;
                // SoundMechanicHandler.PlaySound(transform, transform.position, Loudness, Range, false);
                source.Play();
                Debug.Log(other.name);
            }
        }

        public string[] GetExtraVisualizationElements()
        {
            return new string[]
            {
                Mathf.Max(Time.time-timeStamp,0).ToString()
            };
        }
    }

}