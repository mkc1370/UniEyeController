using UniEyeController.Core.EyeProcess;
using UnityEditor;
using UnityEngine;

namespace UniEyeController.Editor.EyeProcess
{
    public class EyeMicroMoveEditor : EyeProcessBaseEditor
    {
        private SerializedProperty _weight;
        private SerializedProperty _eyeMoveMultiplier;
        private SerializedProperty _eyeMoveStopTimeMin;
        private SerializedProperty _eyeMoveStopTimeMax;

        public EyeMicroMoveEditor(SerializedProperty property) : base(property)
        {
        }

        protected override void GetProperties(SerializedProperty property)
        {
            _weight = property.FindPropertyRelative(nameof(EyeMicroMove.weight));
            _eyeMoveMultiplier = property.FindPropertyRelative(nameof(EyeMicroMove.eyeMoveMultiplier));
            _eyeMoveStopTimeMin = property.FindPropertyRelative(nameof(EyeMicroMove.eyeMoveStopTimeMin));
            _eyeMoveStopTimeMax = property.FindPropertyRelative(nameof(EyeMicroMove.eyeMoveStopTimeMax));
        }

        protected override void DrawProperties()
        {
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
        }
    }
}