using System.Collections.Generic;
using UniEyeController.Core.Process.Core;
using UnityEditor;
using UnityEngine;

namespace UniEyeController.Editor.Core.Process.Core
{
    public abstract class EyeStatusDrawerBase
    {
        private SerializedProperty _weight;
        
        public EyeStatusDrawerBase(SerializedProperty property)
        {
            _weight = property.FindPropertyRelative(nameof(EyeStatusBase.weight));
        }

        public virtual void Draw(bool isTimeline)
        {
            EditorGUILayout.PropertyField(_weight, new GUIContent("適用度"));
            EditorGUILayout.Space();
        }

        protected void BeginErrorColor(bool isError)
        {
            if (isError)
            {
                GUI.color = Color.red;
            }
        }

        protected void EndErrorColor()
        {
            GUI.color = Color.white;
        }

        protected void DrawErrorMessages(List<string> errorMessages)
        {
            foreach (var errorMessage in errorMessages)
            {
                EditorGUILayout.HelpBox(errorMessage, MessageType.Error);
            }
        }
    }
}