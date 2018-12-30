using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using Dim.Player;

namespace Dim.Visualize
{
    /////////////////////////////////////////////////
    /// Custom Editor for the CameraCutSceneTrigger. 
    /////////////////////////////////////////////////
    [CustomEditor(typeof(CameraCutSceneTrigger))]
    public class CutSceneEditor : Editor
    {


        public override void OnInspectorGUI()
        {
            CameraCutSceneTrigger trigger = (CameraCutSceneTrigger)target;

            DisplayCutScenes(trigger);
            

            if (trigger.inEditing)
            {
                DisplayInEditing(trigger);
            }
            else
            {
                DisplayAddRemoveButtons(trigger);
                DisplayStartEditingButton(trigger);
            }

            DrawDefaultInspector();

        }

        private void DisplayStartEditingButton(CameraCutSceneTrigger trigger)
        {
            if(GUILayout.Button("Start Editing"))
            {
                trigger.inEditing = true;
                for (int i = 0; i < trigger.cutScene.Length; i++)
                {

                    SpawnCameraForTransition(trigger.cutScene[i],i,trigger);
                }
            }
        }

        private void DisplayInEditing(CameraCutSceneTrigger trigger)
        {
            for (int i = 0; i < trigger.cutScene.Length; i++)
            {
                var tr = trigger.cutScene[i];
                GUILayout.Label("Transition #" + i );
                tr.TimeToComplete = EditorGUILayout.FloatField("Time To Completion", tr.TimeToComplete);
            }

            if (GUILayout.Button("Done Editing"))
            {
                if(trigger.editorTempObjects.Count != trigger.cutScene.Length)
                {
                    Debug.LogError("An Error has Occured. Editing failed.");
                    return;
                }

                for (int i = 0; i < trigger.cutScene.Length; i++)
                {
                    SaveInfoToTransition(trigger,i);
                    DestroyImmediate(trigger.editorTempObjects[i]);
                }

                trigger.editorTempObjects.Clear();
                trigger.inEditing = false;
                PrefabUtility.RecordPrefabInstancePropertyModifications(trigger);
            }
        }

        private void SpawnCameraForTransition(CutSceneTransition c , int index, CameraCutSceneTrigger trigger)
        {
            GameObject g = new GameObject("TEMPCAM#" + index);
            g.AddComponent<Camera>();
            if(c.Type == CutSceneTransition.State.FollowTransform)
            {
                g.transform.parent = c.FollowTarget;
            }
            g.transform.localPosition = c.Pos;
            g.transform.eulerAngles = c.Rot;

            trigger.editorTempObjects.Add(g);
          
        }

        private void SaveInfoToTransition(CameraCutSceneTrigger trigger, int index)
        {
            CutSceneTransition current = trigger.cutScene[index];
            GameObject g = trigger.editorTempObjects[index];

            if(g.transform.parent == null)
            {
                current.Type = CutSceneTransition.State.Static;
                current.FollowTarget = null;
            }
            else
            {
                current.Type = CutSceneTransition.State.FollowTransform;
                current.FollowTarget = g.transform.parent;
            }

            current.Rot = g.transform.eulerAngles;
            current.Pos = g.transform.localPosition;
        }

        private void DisplayCutScenes(CameraCutSceneTrigger trigger)
        {
            GUILayout.Label("Transitions : " + trigger.cutScene.Length);

        }

        private void DisplayAddRemoveButtons(CameraCutSceneTrigger trigger)
        {
            if(GUILayout.Button("Add Transition"))
            {
                List<CutSceneTransition> temp = new List<CutSceneTransition>(trigger.cutScene);
                temp.Add(new CutSceneTransition());
                trigger.cutScene = temp.ToArray();
            }

            if(GUILayout.Button("Remove Transition"))
            {
                List<CutSceneTransition> temp = new List<CutSceneTransition>(trigger.cutScene);
                temp.RemoveAt(temp.Count - 1);
                trigger.cutScene = temp.ToArray();
            }
        }
    }
}
