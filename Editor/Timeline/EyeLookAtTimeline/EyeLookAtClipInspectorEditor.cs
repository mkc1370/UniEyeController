using UniEyeController.Editor.Status;
using UniEyeController.Timeline.UniEyeLookAtTimeline;
using UnityEditor;

namespace UniEyeController.Editor.Timeline.EyeLookAtTimeline
{
    [CustomEditor(typeof(UniEyeLookAtClip))]
    public class EyeLookAtClipClipInspectorEditor : UnityEditor.Editor
    {
        private EyeLookAtStatusEditor _statusEditor;

        private void OnEnable()
        {
            var status = serializedObject.FindProperty(nameof(UniEyeLookAtClip.status));
            _statusEditor = new EyeLookAtStatusEditor(status);

            var script = target as UniEyeLookAtClip;
            if (script == null) return;
        }

        private void OnDisable()
        {
            var script = target as UniEyeLookAtClip;
            if (script == null) return;
        }

        public override void OnInspectorGUI()
        {
            var script = target as UniEyeLookAtClip;
            if (script == null) return;

            serializedObject.Update();

            _statusEditor.Draw(true);

            serializedObject.ApplyModifiedProperties();
        }
    }
}