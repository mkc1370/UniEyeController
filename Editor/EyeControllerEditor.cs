using System.Collections.Generic;
using SimpleEyeController.Constants;
using SimpleEyeController.View;
using UnityEditor;
using UnityEngine;

namespace SimpleEyeController.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(EyeController))]
    public class EyeControllerEditor : UnityEditor.Editor
    {
        private SerializedProperty _executeAlways;
        private SerializedProperty _updateMethod;
        private SerializedProperty _assignMethod;
        private SerializedProperty _animator;
        private SerializedProperty _manualEyeL;
        private SerializedProperty _manualEyeR;
        private SerializedProperty _rangeSetting;
        
        private bool _debugFoldout;

        private void OnEnable()
        {
            _executeAlways = serializedObject.FindProperty(nameof(EyeController.executeAlways));
            _updateMethod = serializedObject.FindProperty(nameof(EyeController.updateMethod));
            _assignMethod = serializedObject.FindProperty(nameof(EyeController.assignMethod));
            _animator = serializedObject.FindProperty(nameof(EyeController.animator));
            _manualEyeL = serializedObject.FindProperty(nameof(EyeController.manualEyeL));
            _manualEyeR = serializedObject.FindProperty(nameof(EyeController.manualEyeR));
            _rangeSetting = serializedObject.FindProperty(
                $"{nameof(EyeController.rangeSetting)}");
            
            var script = target as EyeController;
            if (script == null) return;
        }

        private void OnDisable()
        {
            var script = target as EyeController;
            if (script == null) return;
        }

        public override void OnInspectorGUI()
        {
            var script = target as EyeController;
            if (script == null) return;
            
            serializedObject.Update();
            
            EditorGUI.BeginChangeCheck();
            
            EditorGUILayout.LabelField("一般設定");
            GUILayout.BeginVertical(GUI.skin.box);
            EditorGUI.indentLevel++;
            {
                EditorGUILayout.PropertyField(_executeAlways, new GUIContent("Playしていない状態でも実行する"));
                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(_updateMethod, new GUIContent("視線の更新タイミング"));
            }
            EditorGUI.indentLevel--;
            GUILayout.EndVertical();
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("目のボーンの設定");
            EditorGUI.BeginDisabledGroup(Application.isPlaying);
            GUILayout.BeginVertical(GUI.skin.box);
            EditorGUI.indentLevel++;
            {
                var errorMessages = new List<string>();
                EditorGUI.BeginChangeCheck();
                
                EditorGUILayout.PropertyField(_assignMethod, new GUIContent("指定方法"));
                EditorGUILayout.Space();

                switch ((EyeAssignMethod)_assignMethod.enumValueIndex)
                {
                    case EyeAssignMethod.Animator:
                        BeingErrorColor(_animator.objectReferenceValue == null);
                        EditorGUILayout.PropertyField(_animator);
                        EndErrorColor();
                        if (_animator.objectReferenceValue == null)
                        {
                            errorMessages.Add("Animatorが設定されていません");
                        }

                        break;
                    // case EyeAssignMethod.Transform:
                    //     BeingErrorColor(_manualEyeL.objectReferenceValue == null);
                    //     EditorGUILayout.PropertyField(_manualEyeL, new GUIContent("左目"));
                    //     EndErrorColor();
                    //     if (_manualEyeL.objectReferenceValue == null)
                    //     {
                    //         errorMessages.Add("左目のTransformが設定されていません");
                    //     }
                    //
                    //     BeingErrorColor(_manualEyeR.objectReferenceValue == null);
                    //     EditorGUILayout.PropertyField(_manualEyeR, new GUIContent("右目"));
                    //     EndErrorColor();
                    //     if (_manualEyeR.objectReferenceValue == null)
                    //     {
                    //         errorMessages.Add("右目のTransformが設定されていません");
                    //     }
                    //
                    //     break;
                }

                EditorGUILayout.Space();
                
                DrawErrorMessages(errorMessages);

                if (EditorGUI.EndChangeCheck())
                {
                    // ここで更新しないと変更された内容が反映されない
                    serializedObject.ApplyModifiedProperties();
                    
                    if (errorMessages.Count == 0)
                    {
                        script.ChangeEyeBones();
                    }
                }
            }
            EditorGUI.indentLevel--;
            GUILayout.EndVertical();
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("可動域の設定");
            GUILayout.BeginVertical(GUI.skin.box);
            EditorGUI.indentLevel++;
            {
                EditorGUILayout.PropertyField(_rangeSetting);
            }
            EditorGUI.indentLevel--;
            GUILayout.EndVertical();
            EditorGUILayout.Space();
            

            _debugFoldout = EditorGUILayout.Foldout(_debugFoldout, "デバッグ用");
            if (_debugFoldout)
            {
                EditorGUI.BeginDisabledGroup(true);
                EditorGUI.indentLevel++;
                {
                    EditorGUILayout.ObjectField("左目", script.CurrentEyeL, typeof(Transform), true);
                    EditorGUILayout.ObjectField("右目", script.CurrentEyeR, typeof(Transform), true);
                }
                EditorGUI.indentLevel--;
                EditorGUI.EndDisabledGroup();
            }

            serializedObject.ApplyModifiedProperties();
        }
        
        private void BeingErrorColor(bool isError)
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