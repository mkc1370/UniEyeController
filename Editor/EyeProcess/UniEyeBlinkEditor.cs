using UniEyeController.EyeProcess;
using UnityEditor;
using UnityEngine;

namespace UniEyeController.Editor.EyeProcess
{
    public class UniEyeBlinkEditor : UniEyeProcessBaseEditor
    {
        private SerializedProperty _weight;
        private SerializedProperty _eyeMoveMultiplier;
        private SerializedProperty _eyeBlinkStopTimeMin;
        private SerializedProperty _eyeBlinkStopTimeMax;
        private SerializedProperty _moveEyeWithBlink;
        private SerializedProperty _timeToCloseEyelid;
        private SerializedProperty _timeToOpenEyelid;

        public UniEyeBlinkEditor(SerializedProperty property) : base(property)
        {
        }

        protected override void GetProperties(SerializedProperty property)
        {
            _weight = property.FindPropertyRelative(nameof(UniEyeBlink.weight));
            _eyeMoveMultiplier = property.FindPropertyRelative(nameof(UniEyeBlink.eyeMoveMultiplier));
            _eyeBlinkStopTimeMin = property.FindPropertyRelative(nameof(UniEyeBlink.eyeBlinkStopTimeMin));
            _eyeBlinkStopTimeMax = property.FindPropertyRelative(nameof(UniEyeBlink.eyeBlinkStopTimeMax));
            _moveEyeWithBlink = property.FindPropertyRelative(nameof(UniEyeBlink.moveEyeWithBlink));
            _timeToCloseEyelid = property.FindPropertyRelative(nameof(UniEyeBlink.timeToCloseEyelid));
            _timeToOpenEyelid = property.FindPropertyRelative(nameof(UniEyeBlink.timeToOpenEyelid));
        }

        protected override void DrawProperties()
        {
            EditorGUILayout.LabelField("まばたきの設定", EditorStyles.boldLabel);
            GUILayout.BeginVertical(GUI.skin.box);
            {
                EditorGUILayout.PropertyField(_weight, new GUIContent("適用度"));
                EditorGUILayout.Space();
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