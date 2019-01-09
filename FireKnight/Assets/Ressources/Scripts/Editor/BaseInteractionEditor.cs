using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Dim.Interaction;


namespace Dim.Visualize
{

    /////////////////////////////////////////////////
    /// Custom Editor for the BaseInteraction Script
    /////////////////////////////////////////////////
    [CustomEditor(typeof(InteractionBase),true)]
    [CanEditMultipleObjects]
    public class BaseInteractionEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            InteractionBase interaction = (InteractionBase)target;

            GUILayout.Label(interaction.Storage == null ? "Unlinked" : "Linked to " + interaction.Storage.name);

            if(GUILayout.Button("Link to closest Storage"))
            {
                Storage[] s = FindObjectsOfType<Storage>();

                int closest = -1;
                float dist = float.MaxValue;

                for (int i = 0; i < s.Length; i++)
                {
                    float localDist = Vector3.Distance(interaction.transform.position, s[i].transform.position);
                    if(localDist < dist)
                    {
                        closest = i;
                        dist = localDist;
                    }
                }

                if(closest > -1)
                {
                    interaction.Storage = s[closest];
                    UnityEditor.PrefabUtility.RecordPrefabInstancePropertyModifications(interaction);
                }
                else
                {
                    Debug.Log("No Storage found.");
                }
                
            }

            DrawDefaultInspector();
        }

    }
}
