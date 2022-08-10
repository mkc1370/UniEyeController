using System;
using System.Collections.Generic;
using UniEyeController.Core.Constants;
using UnityEditor;
using UnityEngine;

namespace UniEyeController.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(UniEyeController))]
    public class UniEyeControllerBehaviourEditor : UnityEditor.Editor
    {
        private SerializedProperty _executeAlways;
        private SerializedProperty _updateMethod;
        private SerializedProperty _assignMethod;
        private SerializedProperty _animator;
        private SerializedProperty _prefabForGenericAvatar;
        private SerializedProperty _manualEyeL;
        private SerializedProperty _manualEyeR;
        private SerializedProperty _rangeSetting;
        
        private bool _debugFoldout;

        private void OnEnable()
        {
            _executeAlways = serializedObject.FindProperty(nameof(UniEyeController.executeAlways));
            _updateMethod = serializedObject.FindProperty(nameof(UniEyeController.updateMethod));
            _assignMethod = serializedObject.FindProperty(nameof(UniEyeController.assignMethod));
            _animator = serializedObject.FindProperty(nameof(UniEyeController.animator));
            _prefabForGenericAvatar = serializedObject.FindProperty(nameof(UniEyeController.prefabForGenericAvatar));
            _manualEyeL = serializedObject.FindProperty(nameof(UniEyeController.manualEyeL));
            _manualEyeR = serializedObject.FindProperty(nameof(UniEyeController.manualEyeR));
            _rangeSetting = serializedObject.FindProperty(
                $"{nameof(UniEyeController.rangeSetting)}");
            
            var script = target as UniEyeController;
            if (script == null) return;
        }

        private void OnDisable()
        {
            var script = target as UniEyeController;
            if (script == null) return;
        }

        public override void OnInspectorGUI()
        {
            var script = target as UniEyeController;
            if (script == null) return;
            
            serializedObject.Update();
            
            EditorGUI.BeginChangeCheck();
            
            EditorGUILayout.LabelField("一般設定");
            GUILayout.BeginVertical(GUI.skin.box);
            EditorGUI.indentLevel++;
            {
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(_executeAlways, new GUIContent("Playしていない状態でも実行する（Timeline再生時には設定に関係なく実行されます）"));
                if (EditorGUI.EndChangeCheck())
                {
                    script.Init();
                }
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
                    case EyeAssignMethod.Humanoid:
                        var animator = (Animator)_animator.objectReferenceValue;
                        BeginErrorColor(animator == null || !animator.isHuman);
                        EditorGUILayout.PropertyField(_animator);
                        EndErrorColor();
                        if (animator == null)
                        {
                            errorMessages.Add("Animatorが設定されていません");
                        }
                        if(animator != null && !animator.isHuman)
                        {
                            errorMessages.Add("AnimatorがHumanoidではありません");
                        }

                        EditorGUILayout.HelpBox("全てのmuscleが0の状態を基準として目の初期状態を設定します。", MessageType.Info);

                        break;
                    case EyeAssignMethod.Generic:
                        BeginErrorColor(_prefabForGenericAvatar.objectReferenceValue == null);
                        EditorGUILayout.PropertyField(_prefabForGenericAvatar, new GUIContent("基準となるPrefab"));
                        EndErrorColor();
                        if (_prefabForGenericAvatar.objectReferenceValue == null)
                        {
                            errorMessages.Add("基準となるPrefabが指定されていません");
                        }

                        GUILayout.BeginHorizontal();
                        GUILayout.Space(EditorGUIUtility.singleLineHeight * EditorGUI.indentLevel);
                        if (GUILayout.Button("自動で検索する"))
                        {
                            var prefab = PrefabUtility.GetCorrespondingObjectFromSource(script.gameObject);
                            _prefabForGenericAvatar.objectReferenceValue = prefab;
                        }
                        GUILayout.EndHorizontal();

                        EditorGUILayout.Space();

                        BeginErrorColor(_manualEyeL.objectReferenceValue == null);
                        EditorGUILayout.PropertyField(_manualEyeL, new GUIContent("左目"));
                        EndErrorColor();
                        if (_manualEyeL.objectReferenceValue == null)
                        {
                            errorMessages.Add("左目のTransformが設定されていません");
                        }

                        BeginErrorColor(_manualEyeR.objectReferenceValue == null);
                        EditorGUILayout.PropertyField(_manualEyeR, new GUIContent("右目"));
                        EndErrorColor();
                        if (_manualEyeR.objectReferenceValue == null)
                        {
                            errorMessages.Add("右目のTransformが設定されていません");
                        }

                        var message =
                            $"基準となるPrefabを基準として目の初期状態を決定します。{Environment.NewLine}" +
                            $"目までのパス（GameObject名）が異なっていると正しく動作しません。";
                        EditorGUILayout.HelpBox(message, MessageType.Info);

                        break;
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