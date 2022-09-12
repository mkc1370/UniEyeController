using UnityEditor;

namespace UniEyeController.Editor.Core.Process.Core
{
    public abstract class EyeSettingDrawerBase
    {
        public EyeSettingDrawerBase(SerializedProperty property)
        {
        }

        public virtual void Draw()
        {
        }
    }
}