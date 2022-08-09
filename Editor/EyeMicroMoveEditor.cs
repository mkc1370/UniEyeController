using SimpleEyeController.View.Process;
using UnityEditor;
using UnityEngine;

namespace SimpleEyeController.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(EyeMicroMove))]
    public class EyeMicroMoveEditor : UnityEditor.Editor
    {
        private SerializedProperty _weight;
        private SerializedProperty _eyeMicroMoveMultiplier;
        private SerializedProperty _eyeMoveStopTimeMin;
        private SerializedProperty _eyeMoveStopTimeMax;
        
        private void OnEnable()
        {
            _weight = serializedObject.FindProperty(nameof(EyeMicroMove.weight));
            _eyeMicroMoveMultiplier = serializedObject.FindProperty(nameof(EyeMicroMove.eyeMicroMoveMultiplier));
            _eyeMoveStopTimeMin = serializedObject.FindProperty(nameof(EyeMicroMove.eyeMoveStopTimeMin));
            _eyeMoveStopTimeMax = serializedObject.FindProperty(nameof(EyeMicroMove.eyeMoveStopTimeMax));
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            
            EditorGUILayout.PropertyField(_weight, new GUIContent("眼球微細運動の適用度"));
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(_eyeMicroMoveMultiplier, new GUIContent("目の可動域の何倍の範囲で動かすか"));
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(_eyeMoveStopTimeMin, new GUIContent("動きを止める時間の最小値"));
            EditorGUILayout.PropertyField(_eyeMoveStopTimeMax, new GUIContent("動きを止める時間の最大値"));

            serializedObject.ApplyModifiedProperties();
        }
    }
}