using UniEyeController.Core.Process.Core;
using UniEyeController.Core.Process.LookAt;
using UnityEditor;
using UnityEngine;

namespace UniEyeController.Editor.Core.Process.Core
{
    public abstract class EyeProcessEditorBase
    {
        public abstract string Title { get; protected set; }
        
        protected EyeSettingDrawerBase SettingDrawer;
        protected EyeStatusDrawerBase StatusDrawer;
        
        private SerializedProperty _enabled;
        
        public EyeProcessEditorBase(SerializedProperty property)
        {
            // TODO : あまりきれいではないので直す
            _enabled = property.FindPropertyRelative(nameof(EyeProcessBase<LookAtSetting, LookAtStatus>.enabled));
        }

        public void Draw()
        {
            EditorGUILayout.PropertyField(_enabled, new GUIContent(Title));

            EditorGUILayout.LabelField("設定");
            EditorGUI.indentLevel++;
            GUILayout.BeginVertical(GUI.skin.box);
            {
                StatusDrawer.Draw(false);
            }
            GUILayout.EndVertical();
            EditorGUI.indentLevel--;

            EditorGUI.indentLevel++;
            GUILayout.BeginVertical(GUI.skin.box);
            {
                SettingDrawer.Draw();
            }
            GUILayout.EndVertical();
            EditorGUI.indentLevel--;
        }
    }
}