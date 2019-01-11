using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dim.Interaction;

namespace Dim.Visualize
{
    public class TeaserScript : MonoBehaviour
    {
        [SerializeField] AudioSource audioSource;
        [SerializeField] List<TeaserTimedSoundClip> clips;

        [SerializeField] Storage storage;
        [SerializeField] List<float> addEnergyTimeStamp;




        void Update()
        {
            for (int i = 0; i < clips.Count; i++)
            {
                var item = clips[i];

                if (Time.time > item.TimeStamp)
                {
                    audioSource.clip = item.Clip;
                    audioSource.volume = item.Volume;
                    audioSource.loop = item.Loop;
                    audioSource.Play();

                    clips.RemoveAt(i);
                    i--;
                }
            }

            for (int i = 0; i < addEnergyTimeStamp.Count; i++)
            {
                var item = addEnergyTimeStamp[i];

                if (Time.time > item)
                {
                    storage.RemoveEnergy();
                    addEnergyTimeStamp.RemoveAt(i);
                    i--;
                }
            }
        }
    }

    [System.Serializable]
    public class TeaserTimedSoundClip
    {
        public float TimeStamp;
        public AudioClip Clip;
        public bool Loop;

        [Range(0,1)]
        public float Volume;
    }

}