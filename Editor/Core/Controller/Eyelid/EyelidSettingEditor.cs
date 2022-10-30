using System;
using UniEyeController.Core.Controller.Eyelid;
using UniEyeController.Core.Controller.Eyelid.Constants;
using UniEyeController.Core.Process.Blink;
using UnityEditor;
using UnityEngine;

namespace UniEyeController.Editor.Core.Controller.Eyelid
{
    public partial class EyelidSettingEditor
    {
        private SerializedProperty _eyelidType;
        private SerializedProperty _blendShapeMesh;
        private SerializedProperty _blendShapeIndexes;
        private SerializedProperty _blendShapeNames;
        
        partial void OnEnableVRM(SerializedProperty property);
        partial void DrawAssignMethodVrm();
        
        public EyelidSettingEditor(SerializedProperty property)
        {
            OnEnableVRM(property);
            
            _eyelidType = property.FindPropertyRelative(nameof(EyelidSetting.eyelidType));
            _blendShapeMesh = property.FindPropertyRelative(nameof(EyelidSetting.blendShapeMesh));
            _blendShapeIndexes = property.FindPropertyRelative(nameof(EyelidSetting.blendShapeIndexes));
            _blendShapeNames = property.FindPropertyRelative(nameof(EyelidSetting.blendShapeNames));
        }

        public void Draw()
        {
            EditorGUILayout.LabelField("まぶたの設定", EditorStyles.boldLabel);
            GUILayout.BeginVertical(GUI.skin.box);
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
                        EditorGUILayout.HelpBox(
                            $"スクリプトから{nameof(BlinkProcess)}.{nameof(BlinkProcess.OnBlink)}にイベントを登録してください。",
                            MessageType.Info);
                        break;
                    case EyelidType.VRM1:
                        DrawAssignMethodVrm();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            GUILayout.EndVertical();
        }
    }
}