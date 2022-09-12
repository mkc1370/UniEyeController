using UniEyeController.Core.Process.MicroMove;
using UniEyeController.Editor.Core.Process.Core;
using UnityEditor;

namespace UniEyeController.Editor.Core.Process.MicroMove
{
    public class MicroMoveProcessEditor : EyeProcessEditorBase
    {
        public override string Title { get; protected set; } = "眼球微細運動";
        
        public MicroMoveProcessEditor(SerializedProperty property) : base(property)
        {
            var setting = property.FindPropertyRelative(nameof(MicroMoveProcess.setting));
            var status = property.FindPropertyRelative(nameof(MicroMoveProcess.serializedStatus));
            
            SettingDrawer = new MicroMoveSettingDrawer(setting);
            StatusDrawer = new MicroMoveStatusDrawer(status);
        }
    }
}