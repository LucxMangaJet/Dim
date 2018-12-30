using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Dim.Collector
{
    /////////////////////////////////////////////////
    /// Sets up LevelHandler with new informations at the start of a new Level.
    /////////////////////////////////////////////////
    public class LevelCollector : MonoBehaviour
    {
        public string levelName;
        public bool canSaveInLevel;

        private void Collect()
        {
            
            

            List<CheckPoint> checkpoints = (from a in GameObject.FindGameObjectsWithTag("CHECKPOINT") select a.GetComponent<CheckPoint>()).ToList();
            checkpoints.Sort((emp1, emp2) => emp1.checkPointIndex.CompareTo(emp2.checkPointIndex));


            Transform player = GameObject.FindGameObjectWithTag("Player").transform;
            GameObject cam = GameObject.FindGameObjectWithTag("MainCamera");

            LevelHandler.SetCurrentLevel(new Level(levelName, checkpoints.ToArray(), canSaveInLevel),player, cam);
        }

        
    }
}
