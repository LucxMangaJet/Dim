using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dim
{
    /////////////////////////////////////////////////
    /// Class that gets serialized into save files.
    /////////////////////////////////////////////////
    [System.Serializable]
    public class SaveFile
    {
        public string SceneName;
        public int CheckPointReached;

        public SaveFile(string sceneName, int checkPointReached)
        {
            SceneName = sceneName;
            CheckPointReached = checkPointReached;
        }

    }

    /////////////////////////////////////////////////
    /// Class that gets serialized to store User Data.
    /////////////////////////////////////////////////
    [System.Serializable]
    public class UserData
    {
        public bool CompletedGame;
        public int LevelsUnlocked;
    }


}
