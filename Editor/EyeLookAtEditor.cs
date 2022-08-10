using UniEyeController.EyeProcess;
using UnityEditor;

namespace UniEyeController.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(UniEyeLookAt))]
    public class EyeLookAtEditor : UnityEditor.Editor
    {
        private EyeLookAtStatusEditor _statusEditor;

        private void OnEnable()
        {
            _statusEditor = new EyeLookAtStatusEditor();
            _statusEditor.Init(serializedObject, nameof(UniEyeLookAt.status));

            var script = target as UniEyeLookAt;
            if (script == null) return;
        }

        private void OnDisable()
        {
            var script = target as UniEyeLookAt;
            if (script == null) return;
        }

        public override void OnInspectorGUI()
        {
            var script = target as UniEyeLookAt;
            if (script == null) return;

            serializedObject.Update();

            _statusEditor.Draw(false);

            serializedObject.ApplyModifiedProperties();
        }
    }
}