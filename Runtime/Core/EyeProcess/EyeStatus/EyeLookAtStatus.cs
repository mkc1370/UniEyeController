using System;
using UniEyeController.Core.Constants;
using UnityEngine;

namespace UniEyeController.Core.EyeProcess.EyeStatus
{
    [Serializable]
    public struct EyeLookAtStatus : IEyeStatus
    {
        [Range(0f, 1f)]
        public float weight;
        
        public EyeLookAtDirection direction;
        public LookAtMethod method;
        public Transform targetTransform;
        public ExposedReference<Transform> targetTransformTimeline;
        public Vector3 worldPosition;
        [Range(-1f, 1f)]
        public float normalizedYaw;
        [Range(-1f, 1f)]
        public float normalizedPitch;

        public static EyeLookAtStatus Default =>
            new EyeLookAtStatus()
            {
                weight = 1f,
                method = LookAtMethod.Rotation,
                targetTransform = null,
                worldPosition = Vector3.zero,
                normalizedYaw = 0f,
                normalizedPitch = 0f
            };

        public override string ToString()
        {
            switch (method)
            {
                case LookAtMethod.Direction:
                    return $"D : {direction}";
                case LookAtMethod.Transform:
                    if (targetTransform != null)
                    {
                        return $"T : {targetTransform.name}";
                    }
                    else
                    {
                        return $"T : null";
                    }
                case LookAtMethod.MainCamera:
                    return $"Main Camera";
                case LookAtMethod.WorldPosition:
                    return $"P : {worldPosition}";
                case LookAtMethod.Rotation:
                    return $"R : ({normalizedYaw}, {normalizedPitch})";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public string ErrorMessage
        {
            get
            {
                switch (method)
                {
                    case LookAtMethod.Transform:
                        if (targetTransformTimeline.exposedName == null)
                        {
                            return "Target Transform is null";
                        }

                        return null;
                    default:
                        return null;
                }
            }
        }
    }
}