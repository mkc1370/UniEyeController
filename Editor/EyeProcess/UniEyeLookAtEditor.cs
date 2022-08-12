using UniEyeController.Editor.Status;
using UniEyeController.EyeProcess;
using UnityEditor;
using UnityEngine;

namespace UniEyeController.Editor.EyeProcess
{
    public class UniEyeLookAtEditor : UniEyeProcessBaseEditor
    {
        private EyeLookAtStatusEditor _statusEditor;

        protected override void GetProperties(SerializedProperty property)
        {
            var status = property.FindPropertyRelative(nameof(UniEyeLookAt.status));
            _statusEditor = new EyeLookAtStatusEditor(status);
        }

        protected override void DrawProperties()
        {
            EditorGUILayout.LabelField("注視点の設定", EditorStyles.boldLabel);
            GUILayout.BeginVertical(GUI.skin.box);
            {
                _statusEditor.Draw(false);
            }
            GUILayout.EndVertical();
        }

        public UniEyeLookAtEditor(SerializedProperty property) : base(property)
        {
        }
    }
}