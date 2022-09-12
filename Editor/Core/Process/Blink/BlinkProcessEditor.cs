using UniEyeController.Core.Process.Blink;
using UniEyeController.Editor.Core.Process.Core;
using UnityEditor;

namespace UniEyeController.Editor.Core.Process.Blink
{
    public class BlinkProcessEditor : EyeProcessEditorBase
    {
        public override string Title { get; protected set; } = "まばたき";
        
        public BlinkProcessEditor(SerializedProperty property) : base(property)
        {
            var setting = property.FindPropertyRelative(nameof(BlinkProcess.setting));
            var status = property.FindPropertyRelative(nameof(BlinkProcess.serializedStatus));

            SettingDrawer = new BlinkSettingDrawer(setting);
            StatusDrawer = new BlinkStatusDrawer(status);
        }
    }
}