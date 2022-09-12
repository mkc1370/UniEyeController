using UniEyeController.Editor.Core.Process.Core;
using UnityEditor;

namespace UniEyeController.Editor.Core.Process.MicroMove
{
    public class MicroMoveStatusDrawer : EyeStatusDrawerBase
    {
        public MicroMoveStatusDrawer(SerializedProperty property) : base(property)
        {
        }

        public override void Draw(bool isTimeline)
        {
            base.Draw(isTimeline);
        }
    }
}