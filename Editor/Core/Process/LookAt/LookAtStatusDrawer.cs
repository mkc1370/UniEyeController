using System;
using System.Collections.Generic;
using UniEyeController.Core.Process.LookAt;
using UniEyeController.Editor.Core.Extensions;
using UniEyeController.Editor.Core.Process.Core;
using UnityEditor;
using UnityEngine;

namespace UniEyeController.Editor.Core.Process.LookAt
{
    public class LookAtStatusDrawer : EyeStatusDrawerBase
    {
        private SerializedProperty _method;
        private SerializedProperty _targetTransform;
        private SerializedProperty _targetTransformTimeline;
        private SerializedProperty _worldPosition;
        private SerializedProperty _normalizedYaw;
        private SerializedProperty _normalizedPitch;
        private SerializedProperty _direction;
        
        public LookAtStatusDrawer(SerializedProperty property) : base(property)
        {
            _method = property.FindPropertyRelative(nameof(LookAtProcessStatus.method));
            _targetTransform = property.FindPropertyRelative(nameof(LookAtProcessStatus.targetTransform));
            _targetTransformTimeline = property.FindPropertyRelative(nameof(LookAtProcessStatus.targetTransformTimeline));
            _worldPosition = property.FindPropertyRelative(nameof(LookAtProcessStatus.worldPosition));
            _normalizedYaw = property.FindPropertyRelative(nameof(LookAtProcessStatus.normalizedYaw));
            _normalizedPitch = property.FindPropertyRelative(nameof(LookAtProcessStatus.normalizedPitch));
            _direction = property.FindPropertyRelative(nameof(LookAtProcessStatus.direction));
        }

        public override void Draw(bool isTimeline)
        {
            base.Draw(isTimeline);
            
            EditorGUILayout.PropertyField(_method, new GUIContent("注視点の指定方法"));
            EditorGUI.indentLevel++;
            switch ((LookAtMethod)_method.enumValueIndex)
            {
                case LookAtMethod.Direction:
                    EditorGUILayout.PropertyField(_direction, new GUIContent("方向"));
                    break;
                case LookAtMethod.Transform:
                    if (isTimeline)
                    {
                        var errorMessages = new List<string>();
                        if (_targetTransformTimeline.exposedReferenceValue == null)
                        {
                            errorMessages.Add($"ターゲットが設定されていません");
                        }

                        EditorExtensions.BeginErrorColor(errorMessages.Count > 0);
                        EditorGUILayout.PropertyField(_targetTransformTimeline, new GUIContent("対象"));
                        EditorExtensions.EndErrorColor();
                        EditorExtensions.DrawErrorMessages(errorMessages);
                    }
                    else
                    {
                        var errorMessages = new List<string>();
                        if (_targetTransform.objectReferenceValue == null)
                        {
                            errorMessages.Add($"ターゲットが設定されていません");
                        }

                        EditorExtensions.BeginErrorColor(errorMessages.Count > 0);
                        EditorGUILayout.PropertyField(_targetTransform, new GUIContent("対象"));
                        EditorExtensions.EndErrorColor();
                        EditorExtensions.DrawErrorMessages(errorMessages);
                    }

                    break;
                case LookAtMethod.MainCamera:
                    break;
                case LookAtMethod.WorldPosition:
                    EditorGUILayout.PropertyField(_worldPosition, new GUIContent("見る位置（ワールド座標）"));
                    break;
                case LookAtMethod.Rotation:
                    EditorGUILayout.PropertyField(_normalizedYaw, new GUIContent("目の角度（左右） [-1, 1]"));
                    EditorGUILayout.PropertyField(_normalizedPitch, new GUIContent("目の角度（上下） [-1, 1]"));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            EditorGUI.indentLevel--;
        }
    }
}