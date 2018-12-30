using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Dim.Interaction;

namespace Dim.Visualize
{
    /////////////////////////////////////////////////
    /// Custom Editor for the Platform Class
    /////////////////////////////////////////////////
    [CustomEditor(typeof(Platform))]
    public class PlatformEditor : BaseInteractionEditor
    {
        int value = 0;

        public override void OnInspectorGUI()
        {
            Platform platform = (Platform)target;
            if (platform.Storage != null)
            {
                if (platform.platformEnergyPositions == null)
                {
                    GUILayout.Label("No States defined");
                }
                else
                {
                    GUILayout.Label("States:");
                    for (int i = 0; i < platform.platformEnergyPositions.Length; i++)
                    {
                        if (platform.platformEnergyPositions[i] != Vector3.zero)
                        {
                            GUILayout.Label(i + ": " + platform.platformEnergyPositions[i]);
                        }
                       
                    }

                }


                GUILayout.TextArea("Use the following button to set the current position of the object as the state the Platform at the given Energy value");
                GUILayout.BeginHorizontal();
               
                if (GUILayout.Button("Set Current Position To"))
                {
                    if(platform.platformEnergyPositions == null || platform.platformEnergyPositions.Length ==0)
                    {
                        platform.platformEnergyPositions = new Vector3[platform.Storage.MaxEnergy+1];
                    }

                    platform.platformEnergyPositions[value] = platform.transform.position;
                    
                    PrefabUtility.RecordPrefabInstancePropertyModifications(platform);
                }

                value = Mathf.Clamp(EditorGUILayout.IntField(value),0,platform.Storage.MaxEnergy);

                GUILayout.EndHorizontal();
            
            }

            base.OnInspectorGUI();
        }
    }
}
