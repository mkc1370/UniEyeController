using System;
using System.Collections.Generic;
using UniEyeController.Core.Main.Constants;
using UniEyeController.Editor.Core.Controller.Eye;
using UniEyeController.Editor.Core.Controller.Eyelid;
using UniEyeController.Editor.Core.Extensions;
using UniEyeController.Editor.Core.Process.Blink;
using UniEyeController.Editor.Core.Process.LookAt;
using UniEyeController.Editor.Core.Process.MicroMove;
using UnityEditor;
using UnityEngine;


namespace UniEyeController.Editor.Core.Main
{
    using  UniEyeController = UniEyeController.Core.Main.UniEyeController;
    
    [CanEditMultipleObjects]
    [CustomEditor(typeof(UniEyeController))]
    public class UniEyeControllerEditor : UnityEditor.Editor
    {
        private SerializedProperty _updateMethod;
        private SerializedProperty _executeAlways;
        
        private SerializedProperty _assignMethod;
        private SerializedProperty _animator;
        private SerializedProperty _prefabForGenericAvatar;
        private SerializedProperty _manualEyeL;
        private SerializedProperty _manualEyeR;

        private EyeSettingEditor _eyeSettingEditor;
        private EyelidSettingEditor _eyelidSettingEditor;
        
        private bool _debugFoldout;

        private LookAtProcessEditor _lookAtProcessEditor;
        private MicroMoveProcessEditor _microMoveProcessEditor;
        private BlinkProcessEditor _blinkProcessEditor;

        private void OnEnable()
        {
            _updateMethod = serializedObject.FindProperty(nameof(UniEyeController.updateMethod));
            _executeAlways = serializedObject.FindProperty(nameof(UniEyeController.executeAlways));
            
            _assignMethod = serializedObject.FindProperty(nameof(UniEyeController.assignMethod));
            _animator = serializedObject.FindProperty(nameof(UniEyeController.animator));
            _prefabForGenericAvatar = serializedObject.FindProperty(nameof(UniEyeController.prefabForGenericAvatar));
            _manualEyeL = serializedObject.FindProperty(nameof(UniEyeController.manualEyeL));
            _manualEyeR = serializedObject.FindProperty(nameof(UniEyeController.manualEyeR));
            
            var eyeSetting = serializedObject.FindProperty(nameof(UniEyeController.setting));
            _eyeSettingEditor = new EyeSettingEditor(eyeSetting);
            
            var eyelidSetting = serializedObject.FindProperty(nameof(UniEyeController.eyelidSetting));
            _eyelidSettingEditor = new EyelidSettingEditor(eyelidSetting);
            
            var lookAt = serializedObject.FindProperty(nameof(UniEyeController.lookAtProcess));
            var microMove = serializedObject.FindProperty(nameof(UniEyeController.microMoveProcess));
            var blink = serializedObject.FindProperty(nameof(UniEyeController.blinkProcess));
            
            _lookAtProcessEditor = new LookAtProcessEditor(lookAt);
            _blinkProcessEditor = new BlinkProcessEditor(blink);
            _microMoveProcessEditor = new MicroMoveProcessEditor(microMove);
            
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

            EditorGUILayout.LabelField("実行設定", EditorStyles.boldLabel);
            GUILayout.BeginVertical(GUI.skin.box);
            {
                EditorGUILayout.PropertyField(_updateMethod, new GUIContent("実行タイミング"));
                EditorGUILayout.PropertyField(_executeAlways, new GUIContent("Playしていない状態でも実行する"));
                EditorGUILayout.HelpBox("Timeline再生時には設定に関係なく実行されます", MessageType.Info);
            }
            GUILayout.EndVertical();

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("目のボーンの設定", EditorStyles.boldLabel);
            EditorGUI.BeginDisabledGroup(Application.isPlaying);
            GUILayout.BeginVertical(GUI.skin.box);
            {
                var errorMessages = new List<string>();
                EditorGUI.BeginChangeCheck();
                
                EditorGUILayout.PropertyField(_assignMethod, new GUIContent("キャラクターの種類"));
                EditorGUILayout.Space();

                switch ((EyeAssignMethod)_assignMethod.enumValueIndex)
                {
                    case EyeAssignMethod.Humanoid:
                        var animator = (Animator)_animator.objectReferenceValue;
                        EditorExtensions.BeginErrorColor(animator == null || !animator.isHuman);
                        EditorGUILayout.PropertyField(_animator);
                        EditorExtensions.EndErrorColor();
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
                        EditorExtensions.BeginErrorColor(_prefabForGenericAvatar.objectReferenceValue == null);
                        EditorGUILayout.PropertyField(_prefabForGenericAvatar, new GUIContent("基準となるPrefab"));
                        EditorExtensions.EndErrorColor();
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

                        EditorExtensions.BeginErrorColor(_manualEyeL.objectReferenceValue == null);
                        EditorGUILayout.PropertyField(_manualEyeL, new GUIContent("左目"));
                        EditorExtensions.EndErrorColor();
                        if (_manualEyeL.objectReferenceValue == null)
                        {
                            errorMessages.Add("左目のTransformが設定されていません");
                        }

                        EditorExtensions.BeginErrorColor(_manualEyeR.objectReferenceValue == null);
                        EditorGUILayout.PropertyField(_manualEyeR, new GUIContent("右目"));
                        EditorExtensions.EndErrorColor();
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
                
                EditorExtensions.DrawErrorMessages(errorMessages);

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
            GUILayout.EndVertical();
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.Space();
            
            serializedObject.Update();
            
            EditorGUI.BeginChangeCheck();

            // 目の設定
            _eyeSettingEditor.Draw();
            EditorGUILayout.Space();
            
            // まぶたの設定
            _eyelidSettingEditor.Draw();
            EditorGUILayout.Space();

            _lookAtProcessEditor.Draw();
            EditorGUILayout.Space();
            _microMoveProcessEditor.Draw();
            EditorGUILayout.Space();
            _blinkProcessEditor.Draw();
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

            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
            }
        }
    }
}