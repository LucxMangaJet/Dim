using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dim.Visualize {

    /////////////////////////////////////////////////
    ///  Displays a simple given text over the Object in combination with ExtraGizmosVisualization.
    /////////////////////////////////////////////////
    public class GizmosStaticTextDisplayer : MonoBehaviour, IExtraVisualization {

        public string textToDisplay;

        public string[] GetExtraVisualizationElements()
        {
            return new string[] { textToDisplay };
        }

       
    }
}