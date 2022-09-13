using UniEyeController.Core.Process.Core;
using UnityEditor;
using UnityEngine;

namespace UniEyeController.Editor.Core.Process.Core
{
    public abstract class EyeSettingDrawerBase
    {
        private SerializedProperty _weight;
        
        public EyeSettingDrawerBase(SerializedProperty property)
        {
            _weight = property.FindPropertyRelative(nameof(EyeSettingBase.weight));
        }

        public virtual void Draw()
        {
            EditorGUILayout.PropertyField(_weight, new GUIContent("適用度"));
            EditorGUILayout.Space();
        }
    }
}