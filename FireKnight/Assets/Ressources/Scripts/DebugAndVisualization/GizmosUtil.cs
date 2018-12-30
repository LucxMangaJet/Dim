using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//https://unionassets.com/blog/unity3d-editor-tips-and-tricks-431
namespace Dim.Visualize
{

    /////////////////////////////////////////////////
    /// Helper Class to utilize the GIZMOS Unity feature.
    /////////////////////////////////////////////////
    public static class GizmosUtils
    {
        public static void DrawText(GUISkin guiSkin, string text, Vector3 position, Color? color = null, int fontSize = 0, float yOffset = 0)
        {
#if UNITY_EDITOR
            var prevSkin = GUI.skin;
            if (guiSkin == null)
                Debug.LogWarning("editor warning: guiSkin parameter is null");
            else
                GUI.skin = guiSkin;

            GUIContent textContent = new GUIContent(text);

            GUIStyle style = (guiSkin != null) ? new GUIStyle(guiSkin.GetStyle("Label")) : new GUIStyle();
            if (color != null)
                style.normal.textColor = (Color)color;
            if (fontSize > 0)
                style.fontSize = fontSize;

            Vector2 textSize = style.CalcSize(textContent);
            Vector3 screenPoint;
            try { 
            screenPoint = Camera.current.WorldToScreenPoint(position);
            }
            catch
            {
                return;
            }
            if (screenPoint.z > 0) // checks necessary to the text is not visible when the camera is pointed in the opposite direction relative to the object
            {
                var worldPosition = Camera.current.ScreenToWorldPoint(new Vector3(screenPoint.x - textSize.x * 0.5f, screenPoint.y + textSize.y * 0.5f + yOffset, screenPoint.z));
                UnityEditor.Handles.Label(worldPosition, textContent, style);
            }
            GUI.skin = prevSkin;
            #endif
        }

        public static void DrawArrow(GUISkin guiSkin, Vector3 start, Vector3 end , float lineSize, Color? color = null)
        {
            #if UNITY_EDITOR

                UnityEditor.Handles.color = (Color)color;
                UnityEditor.Handles.DrawDottedLine(start, end, lineSize);
            #endif
        }

        public static void DrawCone(Vector3 origin, Vector3 dir, float range, float baseRadius)
        {
            Vector3 basePos = origin + dir.normalized * range;
            Vector3 pos1 = basePos + Vector3.Cross(dir, Vector3.back).normalized * baseRadius;
            Vector3 pos2 = basePos + Vector3.Cross(dir, Vector3.forward).normalized * baseRadius;

            Gizmos.DrawLine(pos1,pos2);
            Gizmos.DrawLine(origin, pos2);
            Gizmos.DrawLine(origin, pos1);
        }
    }
}