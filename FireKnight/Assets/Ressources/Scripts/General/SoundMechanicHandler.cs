using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dim.Visualize;

namespace Dim
{

    /////////////////////////////////////////////////
    /// Singleton, deals with the Sound Detection mechanic: detection and casting.
    /////////////////////////////////////////////////
    public class SoundMechanicHandler
    {
        private static SoundMechanicHandler instance;

        List<Transform> soundsListeners;

        private SoundMechanicHandler()
        {
            soundsListeners = new List<Transform>();
        }

        public static void Initiate()
        {
            instance = new SoundMechanicHandler();
        }

        public static List<Transform> GetListeners()
        {
            return instance.soundsListeners;
        }

        private static void CallHearingMethodsOnListeners(Transform source, Vector3 origin, float loudness, float range)
        {

                foreach (var listener in instance.soundsListeners)
                {
                if (listener != source)
                {
                    CheckAndCallIfHearing(origin, loudness, range, listener);
                }
                }
            
        }

        private static void CheckAndCallIfHearing(Vector3 origin, float loudness, float range, Transform listener)
        {
            float dist = Vector3.Distance(origin, listener.position);

            if (dist <= range)
            {
                listener.GetComponent<ISoundMechanicTaker>().RegisterSoundHeard( new SoundHeard(loudness , origin));

            }
        }


        public static void PlaySound(Transform source, Vector3 origin, float loudness, float range, bool visulize)
        {
            if (visulize)
            {
                PlayVisualSound(origin, loudness, range);
            }

            CallHearingMethodsOnListeners(source, origin, loudness, range);
        }

        public static void PlayVisualSound(Vector3 origin, float loudness, float range)
        {
            GameObject s = GameObject.Instantiate(PrefabHolder.SoundElement(), origin, Quaternion.identity);
            SoundElement sE = s.GetComponent<SoundElement>();
            sE.Setup(loudness, range);
        }

        [System.Obsolete("Method should be dropped for limided effectiveness.")]
        public static void PlayVisualSound(Vector3 origin, float loudness)
        {
            GameObject s = GameObject.Instantiate(PrefabHolder.SoundElement(), origin, Quaternion.identity);
            SoundElement sE = s.GetComponent<SoundElement>();
            sE.Setup(loudness);

        }

        public static void AddListener(Transform t)
        {

            if (!instance.soundsListeners.Contains(t))
            {
                instance.soundsListeners.Add(t);
            }
            
        }

        public static void RemoveListener(Transform t)
        {
            instance.soundsListeners.Remove(t);
        }

    }
}


