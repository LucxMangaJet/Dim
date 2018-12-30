using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dim.Interaction
{
    /////////////////////////////////////////////////
    /// Base Component to form PorterPipe Systems.
    /// Essentially gets connected to the Previous and Next PorterPipeSections by the PorterPipeDrawer.
    /////////////////////////////////////////////////
    public class PorterPipeSection : MonoBehaviour, Dim.Visualize.IExtraVisualization
    {

        public PorterPipeSection nextSection, previousSection;
        public bool isEntrance;
        public bool isCurve;
        public bool isBlocked;

        public bool IsBlocked()
        {
            return isBlocked;
        }

        public PorterPipeSection GetNextSection(bool forward)
        {
            if (forward)
            {
                return nextSection;
            }
            else
            {
                return previousSection;
            }
        }


        public void SetNextSection(bool forward, PorterPipeSection p)
        {
            if (forward)
            {
                nextSection = p;
            }
            else
            {
                previousSection = p;
            }
        }

        public string[] GetExtraVisualizationElements()
        {
            return new string[]
            {
            (isBlocked)?"BLOCKED":""
            };
        }
    }
}