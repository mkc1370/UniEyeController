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
        private SerializedProperty _timeToCloseEyelid;
        private SerializedProperty _timeToOpenEyelid;
        private SerializedProperty _moveEyeWithBlink;
        
        public BlinkSettingDrawer(SerializedProperty property) : base(property)
        {
            _eyeMoveMultiplier = property.FindPropertyRelative(nameof(BlinkSetting.eyeMoveMultiplier));
            _eyeBlinkStopTimeMin = property.FindPropertyRelative(nameof(BlinkSetting.eyeBlinkStopTimeMin));
            _eyeBlinkStopTimeMax = property.FindPropertyRelative(nameof(BlinkSetting.eyeBlinkStopTimeMax));
            _timeToCloseEyelid = property.FindPropertyRelative(nameof(BlinkSetting.timeToCloseEyelid));
            _timeToOpenEyelid = property.FindPropertyRelative(nameof(BlinkSetting.timeToOpenEyelid));
            _moveEyeWithBlink = property.FindPropertyRelative(nameof(BlinkSetting.moveEyeWithBlink));
        }

        public override void Draw()
        {
            base.Draw();

            EditorGUILayout.LabelField("まばたきを止める時間 [s]");
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(_eyeBlinkStopTimeMin, new GUIContent("最短"));
            EditorGUILayout.PropertyField(_eyeBlinkStopTimeMax, new GUIContent("最長"));
            EditorGUI.indentLevel--;
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("まばたきの時間 [s]");
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(_timeToCloseEyelid, new GUIContent("閉じる時間"));
            EditorGUILayout.PropertyField(_timeToOpenEyelid, new GUIContent("開く時間"));
            EditorGUI.indentLevel--;
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(_moveEyeWithBlink, new GUIContent("まばたきと同時に目を動かす"));
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(_eyeMoveMultiplier, new GUIContent("目の可動域の何倍の範囲で動かすか"));
            EditorGUI.indentLevel--;
        }
    }
}