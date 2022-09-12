using UniEyeController.Core.Constants;
using UnityEngine;

namespace UniEyeController.Core.Controller.Eye
{
    /// <summary>
    /// TやAスタンスでの目の状態
    /// </summary>
    public class SingleEyeDefaultStatus
    {
        public EyeType EyeType;
        public Transform Bone;
        public Quaternion Rotation;
        public Quaternion LocalRotation;
        
        public SingleEyeDefaultStatus(Transform bone, Transform boneForDefaultRotation, EyeType eyeType)
        {
            EyeType = eyeType;
            Bone = bone;
            Rotation = boneForDefaultRotation.rotation;
            LocalRotation = boneForDefaultRotation.localRotation;
        }
    }
}