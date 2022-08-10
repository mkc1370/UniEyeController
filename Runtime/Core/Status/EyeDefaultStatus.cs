using UniEyeController.Core.Constants;
using UnityEngine;

namespace UniEyeController.Core.Status
{
    /// <summary>
    /// TやAスタンスでの目の状態
    /// </summary>
    public class EyeDefaultStatus
    {
        public EyeType EyeType;
        public Transform Bone;
        public Quaternion Rotation;
        public Quaternion LocalRotation;
        
        public EyeDefaultStatus(Transform bone, Transform boneForDefaultRotation, EyeType eyeType)
        {
            EyeType = eyeType;
            Bone = bone;
            Rotation = boneForDefaultRotation.rotation;
            LocalRotation = boneForDefaultRotation.localRotation;
        }
    }
}