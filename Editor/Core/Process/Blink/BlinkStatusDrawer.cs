using UniEyeController.Core.Process.Blink;
using UniEyeController.Editor.Core.Process.Core;
using UnityEditor;
using UnityEngine;

namespace UniEyeController.Editor.Core.Process.Blink
{
    public class BlinkStatusDrawer : EyeStatusDrawerBase
    {
        private SerializedProperty _eyeMoveMultiplier;
        private SerializedProperty _eyeBlinkStopTimeMin;
        private SerializedProperty _eyeBlinkStopTimeMax;
        private SerializedProperty _timeToCloseEyelid;
        private SerializedProperty _timeToOpenEyelid;
        private SerializedProperty _moveEyeWithBlink;
        
        public BlinkStatusDrawer(SerializedProperty property) : base(property)
        {
            _eyeMoveMultiplier = property.FindPropertyRelative(nameof(BlinkStatus.eyeMoveMultiplier));
            _eyeBlinkStopTimeMin = property.FindPropertyRelative(nameof(BlinkStatus.eyeBlinkStopTimeMin));
            _eyeBlinkStopTimeMax = property.FindPropertyRelative(nameof(BlinkStatus.eyeBlinkStopTimeMax));
            _timeToCloseEyelid = property.FindPropertyRelative(nameof(BlinkStatus.timeToCloseEyelid));
            _timeToOpenEyelid = property.FindPropertyRelative(nameof(BlinkStatus.timeToOpenEyelid));
            _moveEyeWithBlink = property.FindPropertyRelative(nameof(BlinkStatus.moveEyeWithBlink));
        }

        public override void Draw(bool isTimeline)
        {
            base.Draw(isTimeline);
            
            EditorGUILayout.LabelField("まばたきの設定", EditorStyles.boldLabel);
            GUILayout.BeginVertical(GUI.skin.box);
            {
                EditorGUILayout.PropertyField(_eyeMoveMultiplier, new GUIContent("目の可動域の何倍の範囲で動かすか"));
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("まばたきを止める時間 [s]");
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(_eyeBlinkStopTimeMin, new GUIContent("最短"));
                EditorGUILayout.PropertyField(_eyeBlinkStopTimeMax, new GUIContent("最長"));
                EditorGUI.indentLevel--;
                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(_moveEyeWithBlink, new GUIContent("まばたきと同時に目を動かす"));
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("まばたきの時間 [s]");
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(_timeToCloseEyelid, new GUIContent("閉じる時間"));
                EditorGUILayout.PropertyField(_timeToOpenEyelid, new GUIContent("開く時間"));
                EditorGUI.indentLevel--;
            }
            GUILayout.EndVertical();
        }
    }
}