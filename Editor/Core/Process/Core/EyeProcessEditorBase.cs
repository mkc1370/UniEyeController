using UniEyeController.Core.Process.Core;
using UnityEditor;
using UnityEngine;

namespace UniEyeController.Editor.Core.Process.Core
{
    public abstract class EyeProcessEditorBase
    {
        protected EyeStatusDrawerBase StatusDrawer;
        
        private SerializedProperty _enabled;
        
        public EyeProcessEditorBase(SerializedProperty property)
        {
            _enabled = property.FindPropertyRelative(nameof(EyeProcessBase.enabled));
            
            GetProperties(property);
        }

        public void Draw()
        {
            EditorGUILayout.PropertyField(_enabled, new GUIContent("機能の有効化"));
            
            DrawProperties();
        }

        protected abstract void GetProperties(SerializedProperty property);

        protected void DrawProperties()
        {
            EditorGUILayout.LabelField("注視点の設定", EditorStyles.boldLabel);
            GUILayout.BeginVertical(GUI.skin.box);
            {
                StatusDrawer.Draw(false);
            }
            GUILayout.EndVertical();
        }
    }
}