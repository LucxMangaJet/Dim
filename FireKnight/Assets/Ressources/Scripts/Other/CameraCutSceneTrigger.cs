using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Dim.Player
{

    /////////////////////////////////////////////////
    /// Used to make the Camera  play a CutScene.
    /////////////////////////////////////////////////
    public class CameraCutSceneTrigger : MonoBehaviour
    {
        public CutSceneTransition[] cutScene;

        //forEditor
        [HideInInspector] public bool inEditing=false;
        [HideInInspector] public List<GameObject> editorTempObjects = new List<GameObject>();

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                CameraBehavior camB = Dim.LevelHandler.GetCamera().GetComponent<CameraBehavior>();

                if(cutScene != null)
                {
                    camB.OnCutSceneEnd += DisableCutSceneAndReenablePlayer;
                    camB.SetToCutScene(cutScene);
                    LevelHandler.GetPlayer().GetComponent<Player.PlayerController>().LockControls = true;
                  
                }
                
            }
        }


        private void DisableCutSceneAndReenablePlayer()
        {
            LevelHandler.GetPlayer().GetComponent<Player.PlayerController>().LockControls = false;
            CameraBehavior camB = Dim.LevelHandler.GetCamera().GetComponent<CameraBehavior>();
            camB.OnCutSceneEnd -= DisableCutSceneAndReenablePlayer;
            Destroy(gameObject);
        }



    }
}
