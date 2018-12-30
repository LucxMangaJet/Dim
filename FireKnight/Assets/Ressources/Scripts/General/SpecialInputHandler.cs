using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dim.Visualize;

namespace Dim
{

    /////////////////////////////////////////////////
    /// Checks for special Keyboard input, like quicksaving.
    /////////////////////////////////////////////////
    public class SpecialInputHandler : MonoBehaviour
    {


        void Update()
        {
            if(InputController.GetEscape(InputStateType.JUST_PRESSED))
            {
                GlobalMethods.LoadMenu();
            }

            if (Input.GetKeyDown(KeyCode.F7))
            {
                GlobalMethods.SaveToSaveFile();
            }

            if (Input.GetKeyDown(KeyCode.F8))
            {
                GlobalMethods.LoadFromSaveFile();
            }

            if (Input.GetKeyDown(KeyCode.F12))
            {
                CheatCodeGUI gui = GetComponent<CheatCodeGUI>();
                if (gui != null)
                {
                    gui.Toggle();
                }
            }
            
        }
    }
}
