using UniEyeController.EyeProcess;
using UnityEditor;
using UnityEngine;

namespace UniEyeController.Editor.EyeProcess
{
    [CustomEditor(typeof(UniEyeProcessBase))]
    public abstract class UniEyeProcessBaseEditor : UnityEditor.Editor
    {
        private SerializedProperty _executeAlways;
        private SerializedProperty _updateMethod;
        private SerializedProperty _executionOrder;

        private bool _footerFoldout;
        private bool _footerFoldoutDetail;

        protected virtual void OnEnable()
        {
            _executeAlways = serializedObject.FindProperty(nameof(UniEyeProcessBase.executeAlways));
            _updateMethod = serializedObject.FindProperty(nameof(UniEyeProcessBase.updateMethod));
            _executionOrder = serializedObject.FindProperty(nameof(UniEyeProcessBase.executionOrder));
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            _footerFoldout = EditorGUILayout.Foldout(_footerFoldout, "実行方法");
            if (_footerFoldout)
            {
                GUILayout.BeginVertical(GUI.skin.box);
                EditorGUILayout.PropertyField(_updateMethod, new GUIContent("実行タイミング"));
                EditorGUILayout.PropertyField(_executeAlways, new GUIContent("Playしていない状態でも実行する"));
                EditorGUILayout.PropertyField(_executionOrder, new GUIContent("実行順（小さい順に実行）"));
                EditorGUILayout.HelpBox("Timeline再生時には設定に関係なく実行されます", MessageType.Info);
                GUILayout.EndVertical();
            }

            EditorGUILayout.Space();

            serializedObject.ApplyModifiedProperties();
        }
    }
}