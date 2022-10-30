#if USE_VRM1
using UniEyeController.Core.Controller.Eyelid;
using UniEyeController.Editor.Core.Extensions;
using UnityEditor;
using UniVRM10;

namespace UniEyeController.Editor.Core.Controller.Eyelid
{
    public partial class EyelidSettingEditor
    {
        private SerializedProperty _vrm10Instance;

        partial void OnEnableVRM(SerializedProperty property)
        {
            _vrm10Instance = property.FindPropertyRelative(nameof(EyelidSetting.vrm10Instance));
        }

        partial void DrawAssignMethodVrm()
        {
            var vrm10Instance = (Vrm10Instance)_vrm10Instance.objectReferenceValue;
            EditorExtensions.BeginErrorColor(vrm10Instance == null);
            EditorGUILayout.PropertyField(_vrm10Instance);
            EditorExtensions.EndErrorColor();
            if (vrm10Instance == null)
            {
                EditorGUILayout.HelpBox($"{nameof(Vrm10Instance)}が設定されていません", MessageType.Error);
            }
        }
    }
}
#endif