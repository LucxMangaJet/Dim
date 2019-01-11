using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dim {

    /////////////////////////////////////////////////
    /// Tells the LevelHandler to Update the Checkpoint reached when triggered by the Player.
    /////////////////////////////////////////////////
    public class CheckPoint : MonoBehaviour, Visualize.IExtraVisualization
    {

        public int checkPointIndex;
        public byte Energy;

        public string[] GetExtraVisualizationElements()
        {
            return new string[]
            {
                "CHECKPOINT #" + (checkPointIndex),
                "Energy: " + Energy
            };
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                

                if (LevelHandler.GetCurrentCheckPointIndex() <= checkPointIndex)
                {
                    LevelHandler.SetCurrentCheckPointIndex(checkPointIndex);
                    GlobalMethods.SaveToSaveFile();
                }

                if (checkPointIndex == 0)
                {
                    GameObject.FindGameObjectWithTag("LEVEL_COLLECTOR").GetComponent<FaderEffect>().FadeIn(Color.black, 3);
                }
            }
    }
    }
}
