using UniEyeController.Core.Process.MicroMove;
using UniEyeController.Editor.Core.Process.Core;
using UnityEditor;

namespace UniEyeController.Editor.Core.Process.MicroMove
{
    public class MicroMoveProcessEditor : EyeProcessEditorBase
    {
        public MicroMoveProcessEditor(SerializedProperty property) : base(property)
        {
        }

        protected override void GetProperties(SerializedProperty property)
        {
            var status = property.FindPropertyRelative(nameof(MicroMoveProcess.statusMonoBehaviour));
            StatusDrawer = new MicroMoveStatusDrawer(status);
        }
    }
}