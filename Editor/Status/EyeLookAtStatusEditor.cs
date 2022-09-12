using System;
using System.Collections.Generic;
using UniEyeController.Core.Constants;
using UniEyeController.Core.EyeProcess.EyeStatus;
using UnityEditor;
using UnityEngine;

namespace UniEyeController.Editor.Status
{
    public class EyeLookAtStatusEditor
    {
        private SerializedProperty _weight;
        private SerializedProperty _method;
        private SerializedProperty _targetTransform;
        private SerializedProperty _targetTransformTimeline;
        private SerializedProperty _worldPosition;
        private SerializedProperty _normalizedYaw;
        private SerializedProperty _normalizedPitch;
        private SerializedProperty _direction;
        
        public EyeLookAtStatusEditor(SerializedProperty property)
        {
            _weight = property.FindPropertyRelative(nameof(EyeLookAtStatus.weight));
            _method = property.FindPropertyRelative(nameof(EyeLookAtStatus.method));
            _targetTransform = property.FindPropertyRelative(nameof(EyeLookAtStatus.targetTransform));
            _targetTransformTimeline = property.FindPropertyRelative(nameof(EyeLookAtStatus.targetTransformTimeline));
            _worldPosition = property.FindPropertyRelative(nameof(EyeLookAtStatus.worldPosition));
            _normalizedYaw = property.FindPropertyRelative(nameof(EyeLookAtStatus.normalizedYaw));
            _normalizedPitch = property.FindPropertyRelative(nameof(EyeLookAtStatus.normalizedPitch));
            _direction = property.FindPropertyRelative(nameof(EyeLookAtStatus.direction));
        }

        public void Draw(bool isTimeline)
        {
            EditorGUILayout.PropertyField(_weight, new GUIContent("適用度"));
            EditorGUILayout.Space();
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

                        BeginErrorColor(errorMessages.Count > 0);
                        EditorGUILayout.PropertyField(_targetTransformTimeline, new GUIContent("対象"));
                        EndErrorColor();
                        DrawErrorMessages(errorMessages);
                    }
                    else
                    {
                        var errorMessages = new List<string>();
                        if (_targetTransform.objectReferenceValue == null)
                        {
                            errorMessages.Add($"ターゲットが設定されていません");
                        }

                        BeginErrorColor(errorMessages.Count > 0);
                        EditorGUILayout.PropertyField(_targetTransform, new GUIContent("対象"));
                        EndErrorColor();
                        DrawErrorMessages(errorMessages);
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

        private void BeginErrorColor(bool isError)
        {
            if (isError)
            {
                GUI.color = Color.red;
            }
        }

        private void EndErrorColor()
        {
            GUI.color = Color.white;
        }

        private void DrawErrorMessages(List<string> errorMessages)
        {
            foreach (var errorMessage in errorMessages)
            {
                EditorGUILayout.HelpBox(errorMessage, MessageType.Error);
            }
        }
    }
}