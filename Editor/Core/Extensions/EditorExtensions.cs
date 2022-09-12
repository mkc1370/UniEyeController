using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UniEyeController.Editor.Core.Extensions
{
    public static class EditorExtensions
    {
        public static void BeginErrorColor(bool isError)
        {
            if (isError)
            {
                GUI.color = Color.red;
            }
        }

        public static void EndErrorColor()
        {
            GUI.color = Color.white;
        }

        public static void DrawErrorMessages(List<string> errorMessages)
        {
            foreach (var errorMessage in errorMessages)
            {
                EditorGUILayout.HelpBox(errorMessage, MessageType.Error);
            }
        }
    }
}