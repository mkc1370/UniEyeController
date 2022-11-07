#if USE_VRM1
using UniEyeController.Editor.Core.Extensions;
using UnityEditor;
using UniVRM10;

namespace UniEyeController.Editor
{
    public partial class UniEyeControllerEditor
    {
        private SerializedProperty _vrm10Instance;

        partial void OnEnableVRM()
        {
            _vrm10Instance = serializedObject.FindProperty(nameof(UniEyeController.vrm10Instance));
        }

        partial void DrawVrm()
        {
            var vrm10Instance = (Vrm10Instance)_vrm10Instance.objectReferenceValue;
            EditorExtensions.BeginErrorColor(vrm10Instance == null);
            EditorGUILayout.PropertyField(_vrm10Instance);
            EditorExtensions.EndErrorColor();
            if (vrm10Instance == null)
            {
                _errorMessages.Add($"{nameof(Vrm10Instance)}が設定されていません");
            }
        }
    }
}
#endif
