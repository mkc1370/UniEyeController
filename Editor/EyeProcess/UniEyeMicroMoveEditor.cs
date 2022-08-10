using UniEyeController.EyeProcess;
using UnityEditor;
using UnityEngine;

namespace UniEyeController.Editor.EyeProcess
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(UniUniEyeMicroMove))]
    public class UniEyeMicroMoveEditor : UniEyeProcessBaseEditor
    {
        private SerializedProperty _weight;
        private SerializedProperty _eyeMoveMultiplier;
        private SerializedProperty _eyeMoveStopTimeMin;
        private SerializedProperty _eyeMoveStopTimeMax;
        
        protected override void OnEnable()
        {
            base.OnEnable();
            
            _weight = serializedObject.FindProperty(nameof(UniUniEyeMicroMove.weight));
            _eyeMoveMultiplier = serializedObject.FindProperty(nameof(UniUniEyeMicroMove.eyeMoveMultiplier));
            _eyeMoveStopTimeMin = serializedObject.FindProperty(nameof(UniUniEyeMicroMove.eyeMoveStopTimeMin));
            _eyeMoveStopTimeMax = serializedObject.FindProperty(nameof(UniUniEyeMicroMove.eyeMoveStopTimeMax));
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            serializedObject.Update();
            
            EditorGUILayout.LabelField("眼球微細運動の設定", EditorStyles.boldLabel);
            GUILayout.BeginVertical(GUI.skin.box);
            {
                EditorGUILayout.PropertyField(_weight, new GUIContent("適用度"));
                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(_eyeMoveMultiplier, new GUIContent("目の可動域の何倍の範囲で動かすか"));
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("眼球の動きを止める時間 [s]");
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(_eyeMoveStopTimeMin, new GUIContent("最短"));
                EditorGUILayout.PropertyField(_eyeMoveStopTimeMax, new GUIContent("最長"));
                EditorGUI.indentLevel--;
            }
            GUILayout.EndVertical();

            serializedObject.ApplyModifiedProperties();
        }
    }
}