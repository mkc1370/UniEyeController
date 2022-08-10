using UniEyeController.EyeProcess;
using UnityEditor;
using UnityEngine;

namespace UniEyeController.Editor.EyeProcess
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(UniEyeMicroMove))]
    public class EyeMicroMoveEditor : EyeProcessBaseEditor
    {
        private SerializedProperty _weight;
        private SerializedProperty _eyeMicroMoveMultiplier;
        private SerializedProperty _eyeMoveStopTimeMin;
        private SerializedProperty _eyeMoveStopTimeMax;
        
        protected override void OnEnable()
        {
            base.OnEnable();
            
            _weight = serializedObject.FindProperty(nameof(UniEyeMicroMove.weight));
            _eyeMicroMoveMultiplier = serializedObject.FindProperty(nameof(UniEyeMicroMove.eyeMicroMoveMultiplier));
            _eyeMoveStopTimeMin = serializedObject.FindProperty(nameof(UniEyeMicroMove.eyeMoveStopTimeMin));
            _eyeMoveStopTimeMax = serializedObject.FindProperty(nameof(UniEyeMicroMove.eyeMoveStopTimeMax));
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
                EditorGUILayout.PropertyField(_eyeMicroMoveMultiplier, new GUIContent("目の可動域の何倍の範囲で動かすか"));
                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(_eyeMoveStopTimeMin, new GUIContent("動きを止める時間の最小値"));
                EditorGUILayout.PropertyField(_eyeMoveStopTimeMax, new GUIContent("動きを止める時間の最大値"));
            }
            GUILayout.EndVertical();

            serializedObject.ApplyModifiedProperties();
        }
    }
}