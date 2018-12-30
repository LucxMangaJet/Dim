using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Dim.Menu
{

    /////////////////////////////////////////////////
    /// Used to display Levels in the LevelSelection part of the MainMenu.
    /////////////////////////////////////////////////
    public class LevelDisplayLoader : MonoBehaviour
    {
        [SerializeField] Image image;
        [SerializeField] Text nameText;
        [SerializeField] Button playButton;
        string sceneName;
        
        
        public void Setup(Sprite sprite, string name)
        {
            image.sprite = sprite;
            nameText.text = name;
            sceneName = name;
            playButton.onClick.AddListener(delegate { LoadSceneFromPath(name); });
        }


        private void LoadSceneFromPath( string path)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        }
    }
}