﻿using UniEyeController.EyeProcess;
using UnityEditor;
using UnityEngine;

namespace UniEyeController.Editor.EyeProcess
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(UniUniEyeLookAt))]
    public class UniUniEyeLookAtEditor : UniEyeProcessBaseEditor
    {
        private EyeLookAtStatusEditor _statusEditor;

        protected override void OnEnable()
        {
            base.OnEnable();
            
            _statusEditor = new EyeLookAtStatusEditor();
            _statusEditor.Init(serializedObject, nameof(UniUniEyeLookAt.status));

            var script = target as UniUniEyeLookAt;
            if (script == null) return;
        }

        private void OnDisable()
        {
            var script = target as UniUniEyeLookAt;
            if (script == null) return;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            var script = target as UniUniEyeLookAt;
            if (script == null) return;

            serializedObject.Update();

            EditorGUILayout.LabelField("注視点の設定", EditorStyles.boldLabel);
            GUILayout.BeginVertical(GUI.skin.box);
            {
                _statusEditor.Draw(false);
            }
            GUILayout.EndVertical();

            serializedObject.ApplyModifiedProperties();
        }
    }
}