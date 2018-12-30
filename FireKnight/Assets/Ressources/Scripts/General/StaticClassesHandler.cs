using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

namespace Dim
{

    /////////////////////////////////////////////////
    /// Deals with instanciating and updating important Singletons.
    /////////////////////////////////////////////////
    public class StaticClassesHandler : MonoBehaviour
    {
        
        [SerializeField] bool debug;
        [SerializeField] GameState.State startingState;
        [Header("Prefabs")]
        [SerializeField] GameObject heatArea;
        [SerializeField] GameObject soundElement;
        [SerializeField] GameObject energyBullet;
        [SerializeField] GameObject energyLaser;
        [SerializeField] GameObject energyDetectionVisualization;
        [SerializeField] AudioMixer mainMixer;

        private void Awake()
        {
            if (GameObject.FindGameObjectsWithTag("GAME_HANDLER").Length > 1)
            {
                if (debug)
                {
                    Debug.Log("There is more then one Game Handler. Removing excessive.");
                }
                Destroy(gameObject);
            }
            else
            {
                DontDestroyOnLoad(gameObject);

                GameState.Initiate(startingState);
                InputController.Initiate();
                EnergyHandler.Initiate();
                SoundMechanicHandler.Initiate();
                LevelHandler.Initiate();
                PrefabHolder.Initiate(
                    heatArea,
                    soundElement,
                    energyBullet,
                    energyLaser, 
                    energyDetectionVisualization,
                    mainMixer
                    );

                SceneManager.sceneLoaded += CollectLevelProperties;
            }
            
        }

        private void CollectLevelProperties(Scene arg0, LoadSceneMode arg1)
        {

            GameObject levelCollector = GameObject.FindGameObjectWithTag("LEVEL_COLLECTOR");

            if (levelCollector == null)
            {
                if (debug)
                {
                    Debug.Log("No LevelCollector found");
                }
                //Clear the LevelHandler to avoid Leftover information from old level remaining;
                LevelHandler.Clear();
                return;
            }

            if (debug)
            {
                Debug.Log("Collecting level properties from: " + levelCollector.name);
            }

            levelCollector.SendMessage("Collect");
            LevelHandler.Start();
            SoundMechanicHandler.Initiate();
        }

        private void LateUpdate()
        {
            InputController.Update();
        }
    }
}
