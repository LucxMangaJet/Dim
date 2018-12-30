using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Dim.Visualize
{
    /////////////////////////////////////////////////
    /// Special GUI to activate Cheats for Debugging and Playtesting purposes.
    /////////////////////////////////////////////////
    public class CheatCodeGUI : MonoBehaviour
    {

        private new bool enabled = false;
        private static float ButtonWidth = 180, ButtonHeight = 40 , queueLimit = 20;
        
        string informationString = "";
        float informationStringSetTimeStamp = 0;
        

        //log code from https://answers.unity.com/questions/1020051/print-debuglog-to-screen-c.html
        string myLog;
        Queue myLogQueue = new Queue();
        bool shouldDebug;


        //audio
        AudioMixer mixer;
        float volume, playerVolume, ambienceVolume, stepsVolume, objectsVolume, enemiesVolume; 

        public void Toggle()
        {
            enabled = !enabled;
        }

        void OnEnable()
        {
            Application.logMessageReceived += HandleLog;



        }

        private void Start()
        {
            mixer = PrefabHolder.MainMixer();

            mixer.GetFloat("Volume", out volume);
            mixer.GetFloat("PlayerVolume", out playerVolume);
            mixer.GetFloat("AmbienceVolume", out ambienceVolume);
            mixer.GetFloat("StepsVolume", out stepsVolume);
            mixer.GetFloat("ObjectsVolume", out objectsVolume);
            mixer.GetFloat("EnemiesVolume", out enemiesVolume);
        }

        void OnDisable()
        {
            Application.logMessageReceived -= HandleLog;
        }

        private void OnGUI()
        {
            if (!enabled)
            {
                return;
            }

            GUI.Label(new Rect(10, 10, 200, 40), "CheatCodeGUI");
            GUI.Label(new Rect(10, 50, 100, 100),"FPS: " + (int)(1.0f / Time.smoothDeltaTime));
            if (GUI.Button(new Rect(10, 100, ButtonWidth, ButtonHeight), "Get Infinte Energy"))
            {
                Transform player = LevelHandler.GetPlayer();

                if (player == null)
                {
                    Setinfo("No Player Found. Are you inside a level?");
                }
                else
                {
                    Player.PlayerController controller = player.GetComponent<Player.PlayerController>();
                    controller.EnergyAmount = Byte.MaxValue;
                    Setinfo("Gave Player Infinite Energy.");
                }
            }

            if (GUI.Button(new Rect(10, 150, ButtonWidth, ButtonHeight), "Become Invulerable"))
            {
                Transform player = LevelHandler.GetPlayer();

                if (player == null)
                {
                    Setinfo("No Player Found. Are you inside a level?");
                }
                else
                {
                    Player.PlayerController controller = player.GetComponent<Player.PlayerController>();
                    controller.IsInvincible = true;
                    Setinfo("Made Player Invincible.");
                }
            }

            if (GUI.Button(new Rect(10, 200, ButtonWidth, ButtonHeight/2), "Jump To Next Checkpoint"))
            {
                if (LevelHandler.GetCurrentLevel() != null)
                {
                    LevelHandler.SetCurrentCheckPointIndex(LevelHandler.GetCurrentCheckPointIndex() + 1);
                    LevelHandler.Respawn();
                    Setinfo("Jumped to next Checkpoint.");
                }
                else
                {
                    Setinfo("No Level Found. Are you inside a level?");
                }
            }

            if (GUI.Button(new Rect(10, 220, ButtonWidth, ButtonHeight/2), "Jump To Previous Checkpoint"))
            {
                if (LevelHandler.GetCurrentLevel() != null)
                {
                    LevelHandler.SetCurrentCheckPointIndex(LevelHandler.GetCurrentCheckPointIndex() - 1);
                    LevelHandler.Respawn();
                    Setinfo("Jumped to previous Checkpoint.");
                }
                else
                {
                    Setinfo("No Level Found. Are you inside a level?");
                }
            }


            if (GUI.Button(new Rect(10, 250, ButtonWidth, ButtonHeight), "Skip Level"))
            {
                try
                {
                    UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1);
                    Setinfo("Trying to skip to next level");
                }
                catch
                {
                    Setinfo("Unable to load next Level");
                }
            }


            GUI.Label(new Rect(10, 300, ButtonWidth, ButtonHeight), "Checkpoint: " + LevelHandler.GetCurrentCheckPointIndex());
            GUI.Label(new Rect(10, 350, ButtonWidth, ButtonHeight), "GameState: " + GameState.GetState().ToString());
            if (GUI.Button(new Rect(10,400, ButtonWidth, ButtonHeight), "Change State"))
            {
                int val = (int)GameState.GetState() + 1;
                val = val % Enum.GetNames(typeof(GameState.State)).Length;
                GameState.SetState((GameState.State)val);
            }

            //mixerVolumes
            GUI.Label(new Rect(300, 150, 100, 30), "Audio Mixer");

            GUI.Label(new Rect(300, 180, 70, 30), "Master");
            volume = GUI.VerticalSlider(new Rect(300, 200, 30, 80),volume, 20,-80);
            GUI.Label(new Rect(370, 180, 70, 30), "Steps");
            stepsVolume = GUI.VerticalSlider(new Rect(370, 200, 30, 80), stepsVolume, 20, -80);
            GUI.Label(new Rect(440, 180, 70, 30), "Player");
            playerVolume = GUI.VerticalSlider(new Rect(440, 200, 30, 80), playerVolume, 20, -80);


            GUI.Label(new Rect(300, 320, 70, 30), "Enemies");
            enemiesVolume =  GUI.VerticalSlider(new Rect(300, 340, 30, 80), enemiesVolume, 20, -80);
            GUI.Label(new Rect(370, 320, 70, 30), "Ambience");
            ambienceVolume = GUI.VerticalSlider(new Rect(370, 340, 30, 80), ambienceVolume, 20, -80);
            GUI.Label(new Rect(440, 320, 70, 30), "Objects");
            objectsVolume = GUI.VerticalSlider(new Rect(440, 340, 30, 80), objectsVolume, 20, -80);


            mixer.SetFloat("Volume", volume);
            mixer.SetFloat("PlayerVolume", playerVolume);
            mixer.SetFloat("AmbienceVolume", ambienceVolume);
            mixer.SetFloat("StepsVolume",  stepsVolume);
            mixer.SetFloat("ObjectsVolume",  objectsVolume);
            mixer.SetFloat("EnemiesVolume",  enemiesVolume);
            

            //Info text
            if (Time.time - informationStringSetTimeStamp > 2)
            {
                informationString = "";
            }
            GUI.Label(new Rect(150, 10, 200, 40), informationString);



            shouldDebug = GUI.Toggle(new Rect(350, 10, 100, 30), shouldDebug, "Show Debug?");

            //Debug Log
            if (shouldDebug)
            {
                GUIStyle logStyle = new GUIStyle(GUI.skin.label);
                logStyle.richText = true;
                GUI.Label(new Rect(600, 10, 500, 1000), myLog, logStyle);
            }
            else
            {
                string s = "Sound Listeners: \n";

                foreach (var item in SoundMechanicHandler.GetListeners())
                {
                    s += item.name + "\n";
                }
                s += "\n Energy Sources: \n";

                foreach (var item in EnergyHandler.GetEnergyObjects())
                {
                    s += item.GetTransform().name + "\n";
                }

                GUI.Label(new Rect(600, 10, 500, 1000), s);
            }
            
            
        }


        private void Setinfo(string text)
        {
            informationStringSetTimeStamp = Time.time;
            informationString = text;
        }

        

        void HandleLog(string logString, string stackTrace, LogType type)
        {
            myLog = logString;

            string color = "<color=#ffffffff>";

            if(type == LogType.Error || type == LogType.Exception)
            {
                color = "<color=#ff0000ff>";
            }
            if (type == LogType.Warning || type == LogType.Assert)
            {
                color = "<color=#ffff00ff>";
            }


            string newString = "\n" + color +  "[" + type + "] : " + myLog  + " </color> ";
            myLogQueue.Enqueue(newString);
            if (type == LogType.Exception)
            {
                newString = "\n" + stackTrace;
                myLogQueue.Enqueue(newString);
            }

            while (myLogQueue.Count > queueLimit)
            {
                myLogQueue.Dequeue();
            }

            myLog = string.Empty;
            foreach (string log in myLogQueue)
            {
                myLog += log;
            }
        }


    }
}
