using UniEyeController.EyeProcess;
using UnityEditor;
using UnityEngine;

namespace UniEyeController.Editor.EyeProcess
{
    [CustomEditor(typeof(EyeProcessBase))]
    public abstract class EyeProcessBaseEditor : UnityEditor.Editor
    {
        private SerializedProperty _executeAlways;
        private SerializedProperty _updateMethod;
        private SerializedProperty _executionOrder;

        private bool _footerFoldout;

        protected virtual void OnEnable()
        {
            _executeAlways = serializedObject.FindProperty(nameof(EyeProcessBase.executeAlways));
            _updateMethod = serializedObject.FindProperty(nameof(EyeProcessBase.updateMethod));
            _executionOrder = serializedObject.FindProperty(nameof(EyeProcessBase.executionOrder));
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.LabelField("実行タイミング", EditorStyles.boldLabel);
            GUILayout.BeginVertical(GUI.skin.box);
            {
                EditorGUILayout.PropertyField(_updateMethod, new GUIContent("視線の更新タイミング"));

                EditorGUI.indentLevel++;
                _footerFoldout = EditorGUILayout.Foldout(_footerFoldout, "詳細設定");
                EditorGUI.indentLevel++;
                if (_footerFoldout)
                {
                    EditorGUILayout.PropertyField(_executeAlways, new GUIContent("Playしていない状態でも実行する"));
                    EditorGUILayout.PropertyField(_executionOrder, new GUIContent("実行順（小さい順に実行）"));
                    EditorGUILayout.HelpBox("Timeline再生時には設定に関係なく実行されます", MessageType.Info);
                }

                EditorGUI.indentLevel--;
                EditorGUI.indentLevel--;
            }
            GUILayout.EndVertical();
            EditorGUILayout.Space();

            serializedObject.ApplyModifiedProperties();
        }
    }
}