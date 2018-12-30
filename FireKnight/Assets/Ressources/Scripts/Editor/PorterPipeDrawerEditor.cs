using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Dim.Interaction;

namespace Dim.Visualize
{
    /////////////////////////////////////////////////
    ///  Custom Editor for the PorterPipe Drawer
    /////////////////////////////////////////////////
    [CustomEditor(typeof(PorterPipeDrawer))]
    public class PorterPipeDrawerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            GUILayout.TextArea("The order in the hierarchy of all the Child Objects to this GameObject will be used to construct the Pipes." +
                "Once You have finished drawing the Pipe System by creating new children Objects press Build Pipes to create the System.");

            PorterPipeDrawer drawer = (PorterPipeDrawer)target;
            if (GUILayout.Button("Build Pipes"))
            {
                drawer.BuildPipes();
            }
        }
    }
}
