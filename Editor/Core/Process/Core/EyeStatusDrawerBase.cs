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
            _weight = property.FindPropertyRelative(nameof(EyeProcessStatusBase.weight));
        }

        public virtual void Draw(bool isTimeline)
        {
            EditorGUILayout.PropertyField(_weight, new GUIContent("適用度"));
            EditorGUILayout.Space();
        }
    }
}