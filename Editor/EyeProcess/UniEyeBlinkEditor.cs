using UniEyeController.EyeProcess;
using UnityEditor;
using UnityEngine;

namespace UniEyeController.Editor.EyeProcess
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(UniEyeBlink))]
    public class UniEyeBlinkEditor : UniEyeProcessBaseEditor
    {
        private SerializedProperty _weight;
        private SerializedProperty _eyeMoveMultiplier;
        private SerializedProperty _eyeBlinkStopTimeMin;
        private SerializedProperty _eyeBlinkStopTimeMax;
        private SerializedProperty _moveEyeWithBlink;
        private SerializedProperty _timeToCloseEyelid;
        private SerializedProperty _timeToOpenEyelid;

        protected override void OnEnable()
        {
            base.OnEnable();
            
            _weight = serializedObject.FindProperty(nameof(UniEyeBlink.weight));
            _eyeMoveMultiplier = serializedObject.FindProperty(nameof(UniEyeBlink.eyeMoveMultiplier));
            _eyeBlinkStopTimeMin = serializedObject.FindProperty(nameof(UniEyeBlink.eyeBlinkStopTimeMin));
            _eyeBlinkStopTimeMax = serializedObject.FindProperty(nameof(UniEyeBlink.eyeBlinkStopTimeMax));
            _moveEyeWithBlink = serializedObject.FindProperty(nameof(UniEyeBlink.moveEyeWithBlink));
            _timeToCloseEyelid = serializedObject.FindProperty(nameof(UniEyeBlink.timeToCloseEyelid));
            _timeToOpenEyelid = serializedObject.FindProperty(nameof(UniEyeBlink.timeToOpenEyelid));
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

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

            serializedObject.ApplyModifiedProperties();
        }
    }
}