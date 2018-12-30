using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace Dim {

    /////////////////////////////////////////////////
    /// Creates inEditor shortcuts for Level Creation.
    /////////////////////////////////////////////////
    public static class CustomEditorMenus
    {

        [MenuItem("Dim/SnapRotation &y")]
        static void SnapRotation()
        {
            Selection.activeTransform.eulerAngles = RoundRotation(Selection.activeTransform.eulerAngles);
        }

        [MenuItem("Dim/SnapRotation &y", true)]
        static bool ValidateSnapRotation()
        {
            // Return false if no transform is selected.
            return Selection.activeTransform != null;
        }

        [MenuItem("Dim/SnapGrid &x")]
        static void SnapToGrid()
        {
            Selection.activeTransform.position = GlobalMethods.RoundVector3(Selection.activeTransform.position);
        }

        [MenuItem("Dim/SnapGrid &x", true)]
        static bool ValidateSnapToGrid()
        {
            // Return false if no transform is selected.
            return Selection.activeTransform != null;
        }

        [MenuItem("Dim/SnapZto0 &c")]
        static void SnapZTo0()
        {
            Vector3 v = Selection.activeTransform.position;
            Selection.activeTransform.position = new Vector3(v.x, v.y, 0);
        }

        [MenuItem("Dim/SnapZto0 &c", true)]
        static bool ValidateSnapZTo0()
        {
            // Return false if no transform is selected.
            return Selection.activeTransform != null;
        }


        private static Vector3 RoundRotation(Vector3 euler)
        {
            Vector3 vec = euler;
            vec.x = Mathf.Round(vec.x / 90) * 90;
            vec.y = Mathf.Round(vec.y / 90) * 90;
            vec.z = Mathf.Round(vec.z / 90) * 90;
            return vec;
        }



    }
}
