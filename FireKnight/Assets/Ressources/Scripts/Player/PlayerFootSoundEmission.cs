using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Dim.Player
{
    /////////////////////////////////////////////////
    /// Attached to both of the Players Feet Joints, emits a step sound on collision.
    /////////////////////////////////////////////////
    public class PlayerFootSoundEmission : MonoBehaviour, Visualize.IExtraVisualization
    {
        [HideInInspector]  public float Loudness = 5;
        [HideInInspector]  public float Range = 10;
        private float timeStamp;
        private float cooldown = 0.5f;

        AudioSource source;

        private void Start()
        {
            source = GetComponent<AudioSource>();
        }

        public enum StepType {Sneak=3, Walk=5, Sprint=7, Jump = 10}

        public void SetLoudness(StepType type)
        {
            Loudness = (int)type;
        }

        private void OnTriggerEnter(Collider other)
        {
           
            if (other.tag == "Untagged" && Time.time-timeStamp >cooldown)
            {
                timeStamp = Time.time;
                SoundMechanicHandler.PlaySound(transform, transform.position, Loudness, Range);
                source.volume = Loudness / 10;
                source.Play();
            }
        }

        public string[] GetExtraVisualizationElements()
        {
            return new string[]
            {
                ""+(StepType)Loudness
            };
        }
    }
}