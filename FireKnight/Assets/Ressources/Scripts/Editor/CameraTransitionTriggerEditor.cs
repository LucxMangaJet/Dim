using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using Dim.Player;

namespace Dim.Visualize
{
    /////////////////////////////////////////////////
    /// Custom Editor for the CameraTransition Script
    /////////////////////////////////////////////////
    [CustomEditor(typeof(CameraTransitionTrigger))]
    public class CameraTransitionTriggerEditor : Editor
    {


        public override void OnInspectorGUI()
        {
            CameraTransitionTrigger transition = (CameraTransitionTrigger)target;
            if (transition.InEditing)
            {
                DrawInEditing(transition);
            }
            else if (transition.IsSetup)
            {
                DrawAlreadySetup(transition);
            }
            else
            {
                DrawNotSetup(transition);
            }

            if (transition.debug)
            {
                DrawDefaultInspector();
            }

        }

        private void DrawNotSetup(CameraTransitionTrigger tr)
        {

            if (GUILayout.Button("Setup Static Camera"))
            {
                SpawnCameraAt(tr.transform.position, tr);
                tr.FollowPlayer = false;
                tr.InEditing = true;
            }
            else
            if (GUILayout.Button("Setup Player following Camera"))
            {
                SpawnTestPlayerWithCamAt(tr.transform.position, tr.transform.position - new Vector3(0, 0, 7), tr);
                tr.FollowPlayer = true;
                tr.InEditing = true;
            }
        }

        private void DrawInEditing(CameraTransitionTrigger tr)
        {


            if (GUILayout.Button("Confirm"))
            {
                //set positions
                if (tr.FollowPlayer)
                {
                    tr.FollowRelativePos = tr.editingObjects[1].transform.localPosition;
                    tr.Rotation = tr.editingObjects[1].transform.eulerAngles;

                }
                else
                {
                    tr.StaticPos = tr.editingObjects[0].transform.position;
                    tr.Rotation = tr.editingObjects[0].transform.eulerAngles;
                }

                foreach (var ob in tr.editingObjects)
                {
                    DestroyImmediate(ob);
                }
                tr.editingObjects.Clear();

                tr.InEditing = false;
                tr.IsSetup = true;

                UnityEditor.PrefabUtility.RecordPrefabInstancePropertyModifications(tr);
            }
        }

        private void DrawAlreadySetup(CameraTransitionTrigger tr)
        {
            if (tr.FollowPlayer)
            {
                GUILayout.Label("Relative Position: " + tr.FollowRelativePos);

            }
            else
            {
                GUILayout.Label("Position: " + tr.StaticPos);
            }

            GUILayout.Label("Rotation: " + tr.Rotation);

            if (GUILayout.Button("Edit"))
            {
                if (tr.FollowPlayer)
                {
                    GameObject g = SpawnTestPlayerWithCamAt(tr.transform.position, tr.FollowRelativePos, tr);
                    g.transform.eulerAngles = tr.Rotation;
                }
                else
                {
                    GameObject g = SpawnCameraAt(tr.StaticPos, tr);
                    g.transform.eulerAngles = tr.Rotation;
                }

                tr.IsSetup = false;
                tr.InEditing = true;
            }
            else
            if (GUILayout.Button("Change Type"))
            {
                tr.IsSetup = false;
                tr.InEditing = false;
            }
        }

        private GameObject SpawnTestPlayerWithCamAt(Vector3 PlayerPos, Vector3 CamPos, CameraTransitionTrigger tr)
        {
            GameObject g = SpawnPlayerSimulationAt(PlayerPos, tr);
            GameObject c = SpawnCameraAt(CamPos, tr);
            c.transform.parent = g.transform;

            return c;
        }

        private GameObject SpawnPlayerSimulationAt(Vector3 pos, CameraTransitionTrigger tr)
        {
            GameObject g = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            g.name = "TEMPORARY PLAYER FOR EDITING";
            g.transform.position = pos;
            tr.editingObjects.Add(g);
            return g;
        }

        private GameObject SpawnCameraAt(Vector3 pos, CameraTransitionTrigger tr)
        {
            GameObject g = new GameObject("TEMPORARY CAM FOR EDITING");
            g.AddComponent<Camera>();
            g.transform.position = pos;
            tr.editingObjects.Add(g);
            return g;
        }



    }
}