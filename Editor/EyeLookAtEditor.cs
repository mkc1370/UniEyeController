using SimpleEyeController.View.Process;
using UnityEditor;

namespace SimpleEyeController.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(EyeLookAt))]
    public class EyeLookAtEditor : UnityEditor.Editor
    {
        private EyeLookAtStatusEditor _statusEditor;

        private void OnEnable()
        {
            _statusEditor = new EyeLookAtStatusEditor();
            _statusEditor.Init(serializedObject, nameof(EyeLookAt.status));

            var script = target as EyeLookAt;
            if (script == null) return;
        }

        private void OnDisable()
        {
            var script = target as EyeLookAt;
            if (script == null) return;
        }

        public override void OnInspectorGUI()
        {
            var script = target as EyeLookAt;
            if (script == null) return;

            serializedObject.Update();

            _statusEditor.Draw(false);

            serializedObject.ApplyModifiedProperties();
        }
    }
}