using System;
using System.Collections.Generic;
using SimpleEyeController.Constants;
using SimpleEyeController.Model.Status;
using UnityEditor;
using UnityEngine;

namespace SimpleEyeController.Editor
{
    public class EyeLookAtStatusEditor
    {
        private SerializedProperty _weight;
        private SerializedProperty _method;
        private SerializedProperty _target;
        private SerializedProperty _worldPosition;
        private SerializedProperty _normalizedYaw;
        private SerializedProperty _normalizedPitch;
        private SerializedProperty _direction;
        
        public void Init(SerializedObject serializedObject, string statusName)
        {
            _weight = serializedObject.FindProperty($"{statusName}.{nameof(EyeLookAtStatus.weight)}");
            _method = serializedObject.FindProperty($"{statusName}.{nameof(EyeLookAtStatus.method)}");
            _target = serializedObject.FindProperty($"{statusName}.{nameof(EyeLookAtStatus.target)}");
            _worldPosition = serializedObject.FindProperty($"{statusName}.{nameof(EyeLookAtStatus.worldPosition)}");
            _normalizedYaw = serializedObject.FindProperty($"{statusName}.{nameof(EyeLookAtStatus.normalizedYaw)}");
            _normalizedPitch = serializedObject.FindProperty($"{statusName}.{nameof(EyeLookAtStatus.normalizedPitch)}");
            _direction = serializedObject.FindProperty($"{statusName}.{nameof(EyeLookAtStatus.direction)}");
        }

        public void Draw()
        {
            EditorGUILayout.PropertyField(_weight, new GUIContent("視線制御の適用度"));
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(_method, new GUIContent("見る対象"));
            EditorGUILayout.Space();
            switch ((LookAtMethod)_method.enumValueIndex)
            {
                case LookAtMethod.Direction:
                    EditorGUILayout.PropertyField(_direction, new GUIContent("見る方向"));
                    break;
                case LookAtMethod.Transform:
                    var errorMessages = new List<string>();
                    if (_target.objectReferenceValue == null)
                    {
                        errorMessages.Add($"ターゲットが設定されていません");
                    }
                    BeginErrorColor(errorMessages.Count > 0);
                    EditorGUILayout.PropertyField(_target);
                    EndErrorColor();
                    DrawErrorMessages(errorMessages);
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