using UniEyeController.Core.Process.Blink;
using UniEyeController.Editor.Core.Process.Core;
using UnityEditor;
using UnityEngine;

namespace UniEyeController.Editor.Core.Process.Blink
{
    public class BlinkSettingDrawer : EyeSettingDrawerBase
    {
        private SerializedProperty _eyeMoveMultiplier;
        private SerializedProperty _eyeBlinkStopTimeMin;
        private SerializedProperty _eyeBlinkStopTimeMax;
        private SerializedProperty _timeToCloseEyelidMin;
        private SerializedProperty _timeToCloseEyelidMax;
        private SerializedProperty _timeToOpenEyelidMin;
        private SerializedProperty _timeToOpenEyelidMax;
        private SerializedProperty _moveEyeWithBlink;
        
        public BlinkSettingDrawer(SerializedProperty property) : base(property)
        {
            _eyeMoveMultiplier = property.FindPropertyRelative(nameof(BlinkSetting.eyeMoveMultiplier));
            _eyeBlinkStopTimeMin = property.FindPropertyRelative(nameof(BlinkSetting.eyeBlinkStopTimeMin));
            _eyeBlinkStopTimeMax = property.FindPropertyRelative(nameof(BlinkSetting.eyeBlinkStopTimeMax));
            _timeToCloseEyelidMin = property.FindPropertyRelative(nameof(BlinkSetting.timeToCloseEyelidMin));
            _timeToCloseEyelidMax = property.FindPropertyRelative(nameof(BlinkSetting.timeToCloseEyelidMax));
            _timeToOpenEyelidMin = property.FindPropertyRelative(nameof(BlinkSetting.timeToOpenEyelidMin));
            _timeToOpenEyelidMax = property.FindPropertyRelative(nameof(BlinkSetting.timeToOpenEyelidMax));
            _moveEyeWithBlink = property.FindPropertyRelative(nameof(BlinkSetting.moveEyeWithBlink));
        }

        public override void Draw()
        {
            base.Draw();

            EditorGUILayout.LabelField("次のまばたきまでの時間 [s]");
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(_eyeBlinkStopTimeMin, new GUIContent("最短"));
                EditorGUILayout.PropertyField(_eyeBlinkStopTimeMax, new GUIContent("最長"));
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("目を閉じる時間 [s]");
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(_timeToCloseEyelidMin, new GUIContent("最短"));
                EditorGUILayout.PropertyField(_timeToCloseEyelidMax, new GUIContent("最長"));
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("目を開く時間 [s]");
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(_timeToOpenEyelidMin, new GUIContent("最短"));
                EditorGUILayout.PropertyField(_timeToOpenEyelidMax, new GUIContent("最長"));
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(_moveEyeWithBlink, new GUIContent("まばたきと同時に目を動かす"));
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(_eyeMoveMultiplier, new GUIContent("目の可動域の何倍の範囲で動かすか"));
                EditorGUI.indentLevel--;
            }
        }
    }
}