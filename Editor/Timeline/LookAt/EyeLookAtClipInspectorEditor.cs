using UniEyeController.Editor.Core.Process.LookAt;
using UniEyeController.Timeline.LookAt;
using UnityEditor;

namespace UniEyeController.Editor.Timeline.LookAt
{
    [CustomEditor(typeof(UniEyeLookAtClip))]
    public class UniEyeLookAtClipClipInspectorEditor : UnityEditor.Editor
    {
        private LookAtStatusDrawer _statusDrawer;

        private void OnEnable()
        {
            var status = serializedObject.FindProperty(nameof(UniEyeLookAtClip.processStatus));
            _statusDrawer = new LookAtStatusDrawer(status);

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

            _statusDrawer.Draw(true);

            serializedObject.ApplyModifiedProperties();
        }
    }
}