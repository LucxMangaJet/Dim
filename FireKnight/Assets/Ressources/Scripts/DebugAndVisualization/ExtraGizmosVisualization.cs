using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Dim.Visualize {

    /////////////////////////////////////////////////
    /// Gets every IExtraVisualization from the gameObject and visualizes it with Gizmos.
    /////////////////////////////////////////////////
    public class ExtraGizmosVisualization : MonoBehaviour {

        IExtraVisualization v;
        [SerializeField] Vector3 displacement;


        private void Awake()
        {
            v = GetComponent<IExtraVisualization>();
            if (v == null)
            {
                Debug.Log("The GameObject " + gameObject.name + " has an ExtraGizmosVisualization Component but no ExtraVisualization. Please remove it if unecessary.");
                Destroy(this);
            }
        }


        private void OnDrawGizmos()
        {
            if (v == null)
            {
                return;
            }

            string[] elements = v.GetExtraVisualizationElements();

            for (int i = 0; i < elements.Length; i++)
            {
                GizmosUtils.DrawText(GUI.skin, elements[i], transform.position + displacement , Color.magenta, 9, i*10);
            }
            
        }
    }

    /////////////////////////////////////////////////
    /// Interface used to debug information visually using Gizmos.
    /// Used in pair with the ExtraGizmosVisualization.
    /////////////////////////////////////////////////
    interface IExtraVisualization
    {
        string[] GetExtraVisualizationElements();
    }
}
