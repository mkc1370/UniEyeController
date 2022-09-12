using UniEyeController.Core.Process.LookAt;
using UniEyeController.Editor.Core.Process.Core;
using UnityEditor;

namespace UniEyeController.Editor.Core.Process.LookAt
{
    public class LookAtProcessEditor : EyeProcessEditorBase
    {
        public LookAtProcessEditor(SerializedProperty property) : base(property)
        {
        }
        
        protected override void GetProperties(SerializedProperty property)
        {
            var status = property.FindPropertyRelative(nameof(LookAtProcess.statusMonoBehaviour));
            StatusDrawer = new LookAtStatusDrawer(status);
        }
    }
}