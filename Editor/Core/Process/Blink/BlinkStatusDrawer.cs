using UniEyeController.Editor.Core.Process.Core;
using UnityEditor;

namespace UniEyeController.Editor.Core.Process.Blink
{
    public class BlinkStatusDrawer : EyeStatusDrawerBase
    {
        public BlinkStatusDrawer(SerializedProperty property) : base(property)
        {
        }

        public override void Draw(bool isTimeline)
        {
            base.Draw(isTimeline);
        }
    }
}