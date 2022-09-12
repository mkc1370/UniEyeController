using UniEyeController.Core.Process.LookAt;
using UniEyeController.Editor.Core.Process.Core;
using UnityEditor;

namespace UniEyeController.Editor.Core.Process.LookAt
{
    public class LookAtProcessEditor : EyeProcessEditorBase
    {
        public override string Title { get; protected set; } = "注視点";
        
        public LookAtProcessEditor(SerializedProperty property) : base(property)
        {
            var setting = property.FindPropertyRelative(nameof(LookAtProcess.setting));
            var status = property.FindPropertyRelative(nameof(LookAtProcess.serializedStatus));

            SettingDrawer = new LookAtSettingDrawer(setting);
            StatusDrawer = new LookAtStatusDrawer(status);
        }
    }
}