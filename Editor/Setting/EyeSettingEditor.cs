using System;
using UniEyeController.Core.Setting;
using UnityEditor;
using UnityEngine;

namespace UniEyeController.Editor.Setting
{
    public class EyeSettingEditor
    {
        private SerializedProperty _horizontalInside;
        private SerializedProperty _horizontalInsideMultiplier;
        private SerializedProperty _verticalUp;
        private SerializedProperty _verticalDown;
        
        public EyeSettingEditor(SerializedProperty property)
        {
            _horizontalInside = property.FindPropertyRelative(nameof(EyeSetting.horizontalOutside));
            _horizontalInsideMultiplier = property.FindPropertyRelative(nameof(EyeSetting.horizontalInsideMultiplier));
            _verticalUp = property.FindPropertyRelative(nameof(EyeSetting.verticalUp));
            _verticalDown = property.FindPropertyRelative(nameof(EyeSetting.verticalDown));
        }

        public void Draw()
        {
            EditorGUILayout.LabelField("目の可動域の設定");
            GUILayout.BeginVertical(GUI.skin.box);
            EditorGUI.indentLevel++;
            {
                EditorGUILayout.PropertyField(_horizontalInside, new GUIContent("水平（鼻の反対側） [オイラー角]"));
                EditorGUILayout.PropertyField(_horizontalInsideMultiplier, new GUIContent("水平（鼻側）※"));
                EditorGUILayout.PropertyField(_verticalUp, new GUIContent("上 [オイラー角]"));
                EditorGUILayout.PropertyField(_verticalDown, new GUIContent("下 [オイラー角]"));

                var message =
                    $"※水平（鼻側）は鼻の反対側への回転角の何倍回転するかを指定します。{Environment.NewLine}" +
                    $"この数値は0.85から1の範囲にするのがおすすめです。";
                EditorGUILayout.HelpBox(message, MessageType.Info);
            }
            EditorGUI.indentLevel--;
            GUILayout.EndVertical();
        }
    }
}