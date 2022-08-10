using System;
using UniEyeController.Core.Constants;
using UniEyeController.Core.Setting;
using UniEyeController.EyeProcess;
using UnityEditor;
using UnityEngine;

namespace UniEyeController.Editor
{
    public class EyelidSettingEditor
    {
        private SerializedProperty _eyelidType;
        private SerializedProperty _blendShapeMesh;
        private SerializedProperty _blendShapeIndexes;
        private SerializedProperty _blendShapeNames;
        
        public EyelidSettingEditor(SerializedProperty property)
        {
            _eyelidType = property.FindPropertyRelative(nameof(EyelidSetting.eyelidType));
            _blendShapeMesh = property.FindPropertyRelative(nameof(EyelidSetting.blendShapeMesh));
            _blendShapeIndexes = property.FindPropertyRelative(nameof(EyelidSetting.blendShapeIndexes));
            _blendShapeNames = property.FindPropertyRelative(nameof(EyelidSetting.blendShapeNames));
        }

        public void Draw()
        {
            EditorGUILayout.PropertyField(_eyelidType, new GUIContent("まぶたの指定方法"));
            switch ((EyelidType)_eyelidType.enumValueIndex)
            {
                case EyelidType.BlendShapeIndex:
                    EditorGUILayout.PropertyField(_blendShapeMesh, new GUIContent("顔"));
                    EditorGUI.indentLevel++;
                    EditorGUILayout.PropertyField(_blendShapeIndexes, new GUIContent("まぶたのBlendShapeの番号"));
                    EditorGUI.indentLevel--;
                    break;
                case EyelidType.BlendShapeName:
                    EditorGUILayout.PropertyField(_blendShapeMesh, new GUIContent("顔"));
                    EditorGUI.indentLevel++;
                    EditorGUILayout.PropertyField(_blendShapeNames, new GUIContent("まぶたのBlendShapeの名前"));
                    EditorGUI.indentLevel--;
                    break;
                case EyelidType.Manual:
                    EditorGUILayout.HelpBox($"スクリプトから{nameof(UniEyeBlink)}.{nameof(UniEyeBlink.OnBlink)}にイベントを登録してください。", MessageType.Info);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}