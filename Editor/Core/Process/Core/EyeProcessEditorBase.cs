using System;
using UniEyeController.Core.Process.Core;
using UniEyeController.Core.Process.LookAt;
using UniEyeController.Editor.Extensions;
using UnityEditor;
using UnityEngine;

namespace UniEyeController.Editor.Core.Process.Core
{
    public abstract class EyeProcessEditorBase
    {
        public abstract string Title { get; protected set; }

        protected EyeSettingDrawerBase SettingDrawer;
        protected EyeStatusDrawerBase StatusDrawer;
        
        private SerializedProperty _updateMethod;
        private SerializedProperty _executeAlways;
        private SerializedProperty _enabled;

        private bool _isFoldout;

        public EyeProcessEditorBase(SerializedProperty property)
        {
            _enabled = property.FindPropertyRelative(nameof(EyeProcessBase.enabled));
            _updateMethod = property.FindPropertyRelative(nameof(EyeProcessBase.updateMethod));
            _executeAlways = property.FindPropertyRelative(nameof(EyeProcessBase.executeAlways));
        }

        public void Draw()
        {
            var enabled = _enabled.boolValue;
            CustomUI.FoldoutWithToggle(Title, ref _isFoldout, ref enabled);
            _enabled.boolValue = enabled;
            
            EditorGUI.indentLevel++;
            if (_isFoldout)
            {
                EditorGUILayout.LabelField("実行設定", EditorStyles.boldLabel);
                GUILayout.BeginVertical(GUI.skin.box);
                {
                    EditorGUILayout.PropertyField(_updateMethod, new GUIContent("実行タイミング"));
                    EditorGUILayout.PropertyField(_executeAlways, new GUIContent("Playしていない状態でも実行する（試験的）"));
                }
                GUILayout.EndVertical();

                EditorGUILayout.LabelField("設定", EditorStyles.boldLabel);
                EditorGUI.indentLevel++;
                GUILayout.BeginVertical(GUI.skin.box);
                {
                    SettingDrawer.Draw();
                }
                GUILayout.EndVertical();
                EditorGUI.indentLevel--;

                EditorGUI.indentLevel++;
                GUILayout.BeginVertical(GUI.skin.box);
                {
                    StatusDrawer.Draw(false);
                }
                GUILayout.EndVertical();
                EditorGUI.indentLevel--;
            }

            EditorGUI.indentLevel--;
        }
    }
}