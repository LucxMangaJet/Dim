using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dim
{
    /////////////////////////////////////////////////
    /// SIngleton, stores Level Important informations such as Player, Camera and checkpoints.
    /////////////////////////////////////////////////
    
    public class LevelHandler
    {
        private static LevelHandler instance;
        private Level currentLevel;
        private Transform player;
        private GameObject camera;
        private int currentCheckPointIndex;

        private bool shouldLoadFromSavefile;
        SaveFile saveFile;

        public LevelHandler()
        {
            Erase();
        }

        private void Erase()
        {
            currentLevel = null;
            player = null;
            camera = null;
            currentCheckPointIndex = 0;

            shouldLoadFromSavefile = false;
            saveFile = null;
        }

        public static void Initiate()
        {
            instance = new LevelHandler();
        }

        public static void SetCurrentLevel(Level level, Transform player, GameObject camera)
        {
            instance.currentLevel = level;
            instance.player = player;
            instance.camera = camera;

        }

        public static Level GetCurrentLevel()
        {
            return instance.currentLevel;
        }

        public static void ShouldLoadFromSaveFile( SaveFile save)
        {
            if(save == null)
            {
                instance.saveFile = null;
                instance.shouldLoadFromSavefile = false;
            }
            else
            {
                instance.saveFile = save;
                instance.shouldLoadFromSavefile = true;
            }

        }

        public static bool IsLoadedFromSaveFile()
        {
            return instance.shouldLoadFromSavefile;
        }

        public static void Clear()
        {
            instance.Erase();
        }

        public static void Start()
        {
            if(instance.currentLevel != null)
            {
                if (instance.shouldLoadFromSavefile)
                {
                    if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == instance.saveFile.SceneName)
                    {
                        instance.player.position = instance.currentLevel.Checkpoints[instance.saveFile.CheckPointReached].transform.position;
                        instance.player.GetComponent<Player.PlayerController>().EnergyAmount = instance.currentLevel.Checkpoints[instance.saveFile.CheckPointReached].Energy;
                        ShouldLoadFromSaveFile(null);
                        return;
                    }
                }
                else
                {
                    instance.player.position = instance.currentLevel.StartPosition;
                    instance.currentCheckPointIndex = 0;
                    instance.player.GetComponent<Player.PlayerController>().EnergyAmount = instance.currentLevel.Checkpoints[0].Energy;
                }
            }
        }

       
        
        public static int GetCurrentCheckPointIndex()
        {
            return instance.currentCheckPointIndex;
        }
        public static void SetCurrentCheckPointIndex(int i)
        {
            if(GetCurrentLevel().Checkpoints.Length-1 < i || i<0)
            {
                return;
            }

            instance.currentCheckPointIndex = i;
        }

        public static Transform GetPlayer()
        {
            return instance.player;
        }

        public static void Respawn()
        {
            instance.player.position = instance.currentLevel.Checkpoints[instance.currentCheckPointIndex].transform.position;
            
            instance.player.GetComponent<Player.PlayerController>().ResetStats();
            instance.player.GetComponent<Player.PlayerController>().EnergyAmount = instance.currentLevel.Checkpoints[instance.currentCheckPointIndex].Energy;
        }

        public static GameObject GetCamera()
        {
            return instance.camera;
        }

        public static void ReloadLevelAtLastCheckpoint()
        {
            SaveFile save = new SaveFile(GlobalMethods.GetCurrentSceneName(), GetCurrentCheckPointIndex());
            ShouldLoadFromSaveFile(save);
            GlobalMethods.LoadSceneFromName(save.SceneName);
        }

    }

    public class Level
    {
        public readonly string Name;
        public readonly bool IsSavable;
        public readonly CheckPoint[] Checkpoints;
        public readonly Vector3 StartPosition;

        public Level(string name, CheckPoint[] checkpoints, bool isSavable)
        {
            Name = name;
            Checkpoints = checkpoints;
            StartPosition = Checkpoints[0].transform.position;
            IsSavable = isSavable;

        }
    }

 
}
