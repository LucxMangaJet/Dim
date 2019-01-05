using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System;
using System.Reflection;

namespace Dim
{
    /////////////////////////////////////////////////
    /// Helper Class containing generally helpfull methods as well as Save and Load methods.
    /////////////////////////////////////////////////
    public static class GlobalMethods 
    {
        public static string SAVEFILE_PATH = Application.persistentDataPath + "/SaveFile";
        public static string USERDATA_PATH = Application.persistentDataPath + "/UserData";

        public static void QuitGame()
        {
            
            GameState.SetState(GameState.State.Quit);
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #endif
            Application.Quit();
        }

        public static void LoadMenu()
        {
            GameState.SetState(GameState.State.MainMenu);
            LoadSceneFromName("MainMenu~");
        }

        public static void LoadSceneFromName(string name)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(name);
        }

        public static string GetCurrentSceneName()
        {
            return UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        }

        public static bool SaveToSaveFile()
        {
            Level level = LevelHandler.GetCurrentLevel();

            if(level == null)
            {
                Debug.Log("Cannot save, you appear to not be in a level. (Did you add a levelcollector?)");
                return false;
            }

            if (!level.IsSavable)
            {
                Debug.Log("You cannot save in this level.");
                return false;
            }

            SaveFile save = new SaveFile(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name, LevelHandler.GetCurrentCheckPointIndex());

            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, save);
            byte[] bytz = ms.ToArray();
            FileStream file = File.Create(SAVEFILE_PATH); 
            file.Write(bytz, 0, bytz.Length);
            file.Close();
            Debug.Log("Saved successfully.");

            return true;
        }

        public static bool LoadFromSaveFile()
        {

            if (!SaveFileExists())
            {
                Debug.LogError("Failed loading: non existent Savefile");
                return false;
            }

            MemoryStream ms = new MemoryStream();
            BinaryFormatter bf = new BinaryFormatter();
            byte[] bytes = File.ReadAllBytes(SAVEFILE_PATH);
            ms.Write(bytes, 0, bytes.Length);
            ms.Seek(0, SeekOrigin.Begin);
            SaveFile save = (SaveFile)bf.Deserialize(ms);

            string name = save.SceneName;

            LevelHandler.ShouldLoadFromSaveFile(save);
            LoadSceneFromName(name);
            Debug.Log("Loaded successfully at CheckPoint: " + save.CheckPointReached);
            return true;
        }

        public static bool SaveFileExists()
        {
            return File.Exists(SAVEFILE_PATH);
        }


        public static void SaveUserData(UserData ud)
        {


            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, ud);
            byte[] bytz = ms.ToArray();
            FileStream file = File.Create(USERDATA_PATH);
            file.Write(bytz, 0, bytz.Length);
            file.Close();
            Debug.Log("User Data saved successfully.");
        }

        public static UserData LoadUserDataFromFile()
        {
            if (!UserDataExists())
            {
                Debug.LogError("UserData not found, creating Blank.");
                UserData ud = new UserData();
                ud.CompletedGame = false;
                ud.LevelsUnlocked = 1;
                SaveUserData(ud);
                
                return ud;
            }

            MemoryStream ms = new MemoryStream();
            BinaryFormatter bf = new BinaryFormatter();
            byte[] bytes = File.ReadAllBytes(USERDATA_PATH);
            ms.Write(bytes, 0, bytes.Length);
            ms.Seek(0, SeekOrigin.Begin);
            UserData data = (UserData)bf.Deserialize(ms);

            return data;
        }

        public static bool UserDataExists()
        {
            return File.Exists(USERDATA_PATH);
        }

        public static void ResetProgress()
        {
            if (UserDataExists())
            {
                File.Delete(USERDATA_PATH);
            }

            if (SaveFileExists())
            {
                File.Delete(SAVEFILE_PATH);
            }
        }


        public static Vector3 RoundVector3(Vector3 v)
        {
            float x = Mathf.Round(v.x);
            float y = Mathf.Round(v.y);
            float z = Mathf.Round(v.z);

            return new Vector3(x, y, z);
        }

        public static Vector3 LerpAngle(Vector3 start, Vector3 end, float t)
        {
            float x = Mathf.LerpAngle(start.x, end.x, t);
            float y = Mathf.LerpAngle(start.y, end.y, t);
            float z = Mathf.LerpAngle(start.z, end.z, t);
            return new Vector3(x, y, z);
        }

        public static bool IsTransformInsideCollider(Transform t,Vector3 center, Vector3 size)
        {
            Vector3 p = t.position;

            bool x = p.x > center.x - size.x / 2 && p.x < center.x + size.x / 2;
            bool y = p.y > center.y - size.y / 2 && p.y < center.y + size.y / 2;
            bool z = p.z > center.z - size.z / 2 && p.z < center.z + size.z / 2;

            return x && y && z;
        }


        

    } 

}
