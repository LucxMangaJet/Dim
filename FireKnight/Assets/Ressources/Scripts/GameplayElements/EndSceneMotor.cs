using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Dim.Interaction
{
    public class EndSceneMotor : InteractionBase
    {

        [SerializeField] byte activationEnergy;

        [SerializeField] float timeToToggle;
        [SerializeField] int sceneToLoadIndex;

        [SerializeField] Image image;


        public override void OnEnergyChange(byte newEnergy)
        {

            if (newEnergy == activationEnergy)
            {
                GameObject.FindGameObjectWithTag("LEVEL_COLLECTOR").GetComponent<FaderEffect>().FadeOut(Color.white, timeToToggle);
                Invoke("Load", timeToToggle);
            }      
        }

        private void Load()
        {
            LevelHandler.ShouldLoadFromSaveFile(null);
            GlobalMethods.LoadSceneAndSaveProgress(sceneToLoadIndex);
        } 



    }
}