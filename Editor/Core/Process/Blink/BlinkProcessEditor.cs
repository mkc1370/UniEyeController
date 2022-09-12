using UniEyeController.Core.Process.Blink;
using UniEyeController.Editor.Core.Process.Core;
using UnityEditor;

namespace UniEyeController.Editor.Core.Process.Blink
{
    public class BlinkProcessEditor : EyeProcessEditorBase
    {
        public BlinkProcessEditor(SerializedProperty property) : base(property)
        {
        }

        protected override void GetProperties(SerializedProperty property)
        {
            var status = property.FindPropertyRelative(nameof(BlinkProcess.statusMonoBehaviour));
            StatusDrawer = new BlinkStatusDrawer(status);
        }
    }
}