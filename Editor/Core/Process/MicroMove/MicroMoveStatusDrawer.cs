using UniEyeController.Core.Process.MicroMove;
using UniEyeController.Editor.Core.Process.Core;
using UnityEditor;
using UnityEngine;

namespace UniEyeController.Editor.Core.Process.MicroMove
{
    public class MicroMoveStatusDrawer : EyeStatusDrawerBase
    {
        private SerializedProperty _eyeMoveMultiplier;
        private SerializedProperty _eyeMoveStopTimeMin;
        private SerializedProperty _eyeMoveStopTimeMax;
        
        public MicroMoveStatusDrawer(SerializedProperty property) : base(property)
        {
            _eyeMoveMultiplier = property.FindPropertyRelative(nameof(MicroMoveStatus.eyeMoveMultiplier));
            _eyeMoveStopTimeMin = property.FindPropertyRelative(nameof(MicroMoveStatus.eyeMoveStopTimeMin));
            _eyeMoveStopTimeMax = property.FindPropertyRelative(nameof(MicroMoveStatus.eyeMoveStopTimeMax));
        }

        public override void Draw(bool isTimeline)
        {
            base.Draw(isTimeline);
            
            EditorGUILayout.LabelField("眼球微細運動の設定", EditorStyles.boldLabel);
            GUILayout.BeginVertical(GUI.skin.box);
            {
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