using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Dim.Interaction;

namespace Dim.Visualize
{

    /////////////////////////////////////////////////
    ///  Custom Editor for the PressurePlate
    /////////////////////////////////////////////////
    [CustomEditor(typeof(PressurePlate))]
    public class PressurePlateEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            PressurePlate plate = (PressurePlate)target;

            GUILayout.Label(plate.Interaction == null ? "Unlinked" : "Linked to " + plate.Interaction.name);

            if (GUILayout.Button("Link to closest Interaction Object"))
            {
                InteractionBase[] s = FindObjectsOfType<InteractionBase>();

                int closest = -1;
                float dist = float.MaxValue;

                for (int i = 0; i < s.Length; i++)
                {
                    float localDist = Vector3.Distance(plate.transform.position, s[i].transform.position);
                    if (localDist < dist)
                    {
                        closest = i;
                        dist = localDist;
                    }
                }

                if (closest > -1)
                {
                    plate.Interaction = s[closest];
                    UnityEditor.PrefabUtility.RecordPrefabInstancePropertyModifications(plate);
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