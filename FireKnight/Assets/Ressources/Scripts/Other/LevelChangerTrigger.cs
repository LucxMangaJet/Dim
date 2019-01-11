using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Dim
{
    /////////////////////////////////////////////////
    /// Tells the LevelHandler to Update the Checkpoint reached when triggered by the Player.
    /////////////////////////////////////////////////
    public class LevelChangerTrigger : MonoBehaviour, Visualize.IExtraVisualization
    {
        [SerializeField] int  sceneToLoadIndex;

        public string[] GetExtraVisualizationElements()
        {
            return new string[]
            {
                "SCENE LOADER",
                "Loads: " + GlobalMethods.GetSceneNameFromBuildIndex(sceneToLoadIndex)
            };
        }

        //https://answers.unity.com/questions/1262342/how-to-get-scene-name-at-certain-buildindex.html
        

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                GameObject.FindGameObjectWithTag("LEVEL_COLLECTOR").GetComponent<FaderEffect>().FadeOut(Color.black, 3);
                Invoke("LoadScene", 3);
            }
        }

        private void LoadScene()
        {
            LevelHandler.ShouldLoadFromSaveFile(null);
            GlobalMethods.LoadSceneAndSaveProgress(sceneToLoadIndex);
        }
    }

   
}

