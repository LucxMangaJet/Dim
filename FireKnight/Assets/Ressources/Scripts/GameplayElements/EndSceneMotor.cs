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
            Debug.Log("AAA");
            if (newEnergy == activationEnergy)
            {
                Debug.Log("V");
                StartCoroutine(endScene());

            }      
        }


        private IEnumerator endScene()
        {

            float counter = 0;

            while (counter < timeToToggle)
            {
                image.color = Color.Lerp( new Color(1,1,1,0),Color.white, counter / timeToToggle);

                yield return null;
                counter += Time.deltaTime;
            }

            LevelHandler.ShouldLoadFromSaveFile(null);
            GlobalMethods.LoadSceneAndSaveProgress(sceneToLoadIndex);

        }

    }
}