using SimpleEyeController.Timeline.EyeLookAtTimeline;
using UnityEditor;

namespace SimpleEyeController.Editor.Timeline.EyeLookAtTimeline
{
    [CustomEditor(typeof(EyeLookAtClip))]
    public class EyeLookAtClipClipInspectorEditor : UnityEditor.Editor
    {
        private EyeLookAtStatusEditor _statusEditor;

        private void OnEnable()
        {
            _statusEditor = new EyeLookAtStatusEditor();
            _statusEditor.Init(serializedObject, nameof(EyeLookAtClip.status));

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

            _statusEditor.Draw();

            serializedObject.ApplyModifiedProperties();
        }
    }
}