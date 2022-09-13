using UnityEditor;

namespace UniEyeController.Editor.Core.Process.Core
{
    public abstract class EyeStatusDrawerBase
    {
        public EyeStatusDrawerBase(SerializedProperty property)
        {
        }

        public virtual void Draw(bool isTimeline)
        {
        }
    }
}