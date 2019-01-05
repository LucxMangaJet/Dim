using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.IO;
using System;

namespace Dim.Menu
{

    /////////////////////////////////////////////////
    /// Handles the complete MainMenu: interactions and display.
    /////////////////////////////////////////////////
    public class MainMenuController : MonoBehaviour
    {

        [SerializeField] GameObject volumeText;
        [SerializeField] GameObject levelDisplayObject, levelSelectionObject , continueObject;
        //relsolutions
        [SerializeField] Dropdown resDropdown;
        Resolution[] resolutions;

        string[] levelScenesNames;
        UserData userData;

        void Start()
        {
            //CheckIfInRightMenu
            userData = GlobalMethods.LoadUserDataFromFile();
            if (userData.CompletedGame)
            {
                GlobalMethods.LoadSceneFromName("ENdindMEnu");
            }

            //resDropdown
            resolutions = Screen.resolutions;
            resDropdown.ClearOptions();
            List<string> options = new List<string>();
            int myResIndx = 0;

            foreach (Resolution r in resolutions)
            {
                if (r.width == Screen.currentResolution.width && r.height == Screen.currentResolution.height)
                    myResIndx = options.Count;


                options.Add(r.width + " x " + r.height);
            }
            resDropdown.AddOptions(options);
            resDropdown.value = myResIndx;
            resDropdown.RefreshShownValue();

            //continue
            if (!GlobalMethods.SaveFileExists())
            {
                continueObject.SetActive(false);
            }



            //LevelSelection
            FindAviableLevels();
            DisplayAviableLevles();

        }



        //changing Menus
        public void EnterSettings()
        {
            GameState.SetState(GameState.State.SettingsMenu);
        }

        public void ReturnToMainMenu()
        {
            GameState.SetState(GameState.State.MainMenu);
        }

        public void EnterLevelSelection()
        {
            GameState.SetState(GameState.State.LevelSelection);
        }


        //Quality
        public void SetQuality(int qualityIndx)
        {
            QualitySettings.SetQualityLevel(qualityIndx);
            Debug.Log("Set Quality to: " + QualitySettings.names[qualityIndx]);
        }

        //Resolutions 
        public void SetResolution(int resIndx)
        {
            Resolution res = resolutions[resIndx];
            Screen.SetResolution(res.width, res.height, Screen.fullScreen);
            Debug.Log("Resolution set to: " + res.width + " x " + res.height);
        }

        public void SetFullscreen(bool isFullscreen)
        {
            Screen.fullScreen = isFullscreen;
            Debug.Log("Fullscreen set to: " + isFullscreen);
        }

        //Audio
        public void SetVolume(float volume)
        {

            PrefabHolder.MainMixer().SetFloat("Volume", volume);
            volumeText.GetComponent<Text>().text = (int)((volume + 80) * (1 / 0.8)) + "%";
        }

        //quit
        public void Quit()
        {
            GlobalMethods.QuitGame();
        }

        //continue 
        public void Continue()
        {
            GlobalMethods.LoadFromSaveFile();
        }


        private void FindAviableLevels()
        {
            List<string> scenes = new List<string>();

            for (int i = 0; i <= userData.LevelsUnlocked; i++)
            {
                string s = System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i));
                    
                if(!s.Contains("~"))
                    scenes.Add(s);
            }

            levelScenesNames = scenes.ToArray();
        }

        private void DisplayAviableLevles()
        {
            for (int i = 0; i < levelScenesNames.Length; i++)
            {
                GameObject g = Instantiate(levelDisplayObject, levelSelectionObject.transform);
                g.GetComponent<LevelDisplayLoader>().Setup(null, (levelScenesNames[i]));
                g.GetComponent<RectTransform>().anchoredPosition = new Vector2(-300 + 110 * i, 100);
                g.SetActive(false);
            }

        }

 
    }
}