using UniEyeController.Editor.Status;
using UniEyeController.Timeline.EyeLookAtTimeline;
using UnityEditor;

namespace UniEyeController.Editor.Timeline.EyeLookAtTimeline
{
    [CustomEditor(typeof(EyeLookAtClip))]
    public class EyeLookAtClipClipInspectorEditor : UnityEditor.Editor
    {
        private EyeLookAtStatusEditor _statusEditor;

        private void OnEnable()
        {
            var status = serializedObject.FindProperty(nameof(EyeLookAtClip.status));
            _statusEditor = new EyeLookAtStatusEditor(status);

            var script = target as EyeLookAtClip;
            if (script == null) return;
        }

        private void OnDisable()
        {
            var script = target as EyeLookAtClip;
            if (script == null) return;
        }

        public override void OnInspectorGUI()
        {
            var script = target as EyeLookAtClip;
            if (script == null) return;

            serializedObject.Update();

            _statusEditor.Draw(true);

            serializedObject.ApplyModifiedProperties();
        }
    }
}