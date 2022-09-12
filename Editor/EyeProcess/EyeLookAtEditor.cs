using UniEyeController.Core.EyeProcess;
using UniEyeController.Editor.Status;
using UnityEditor;
using UnityEngine;

namespace UniEyeController.Editor.EyeProcess
{
    public class EyeLookAtEditor : EyeProcessBaseEditor
    {
        private EyeLookAtStatusEditor _statusEditor;

        protected override void GetProperties(SerializedProperty property)
        {
            var status = property.FindPropertyRelative(nameof(EyeLookAt.statusMonoBehaviour));
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

        public EyeLookAtEditor(SerializedProperty property) : base(property)
        {
        }
    }
}