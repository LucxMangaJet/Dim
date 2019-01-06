﻿using System.Collections;
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
    /// Handles the EndGame Menu.
    /////////////////////////////////////////////////
    public class EndMenuController : MonoBehaviour
    {

        //quit
        public void Quit()
        {
            GlobalMethods.QuitGame();
        }


        public void NewGame()
        {
            GlobalMethods.LoadSceneFromName("MainMenu~");
        }

 
    }
}