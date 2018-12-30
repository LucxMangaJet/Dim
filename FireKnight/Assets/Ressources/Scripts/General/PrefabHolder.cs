using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Dim
{
    /////////////////////////////////////////////////
    /// Stores Prefabs used multiple times.
    /////////////////////////////////////////////////
    public class PrefabHolder
    {

        private static PrefabHolder instance;

        private readonly GameObject heatArea;
        private readonly GameObject soundElement;
        private readonly GameObject energyBullet;
        private readonly GameObject energyLaser;
        private readonly GameObject energyDetectionVisualizer;
        private readonly AudioMixer mainMixer;


        public PrefabHolder(GameObject _heatArea, GameObject _soundElement, GameObject _energyBullet, GameObject _energyLaser, GameObject _energyDetectionVisualizer, AudioMixer _mainMixer)
        {
            heatArea = _heatArea;
            soundElement = _soundElement;
            energyLaser = _energyLaser;
            energyBullet = _energyBullet;
            energyDetectionVisualizer = _energyDetectionVisualizer;
            mainMixer = _mainMixer;
        }

        public static void Initiate(GameObject _heatArea, GameObject _soundElement, GameObject _energyBullet, GameObject _energyLaser, GameObject _energyDetectionVisualizer, AudioMixer _mainMixer)
        {
            instance = new PrefabHolder(_heatArea, _soundElement, _energyBullet, _energyLaser, _energyDetectionVisualizer, _mainMixer);
        }

        public static GameObject EnergyArea()
        {
            return instance.heatArea;
        }

        public static GameObject SoundElement()
        {
            return instance.soundElement;
        }

        public static GameObject EnergyBullet()
        {
            return instance.energyBullet;
        }

        public static GameObject EnergyLaser()
        {
            return instance.energyLaser;
        }

        public static GameObject EnergyDetectionVisualizer()
        {
            return instance.energyDetectionVisualizer;
        }

        public static AudioMixer MainMixer()
        {
            return instance.mainMixer;
        }
    }
}