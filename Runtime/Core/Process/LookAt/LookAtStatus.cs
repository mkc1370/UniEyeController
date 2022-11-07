using System;
using UniEyeController.Core.Process.Core;
using UniEyeController.Core.Process.LookAt.Constants;
using UnityEngine;

namespace UniEyeController.Core.Process.LookAt
{
    [Serializable]
    public struct LookAtStatus : IEyeStatusBase
    {
        [Range(0f, 1f)]
        public float weight;
        public LookAtDirection direction;
        public LookAtMethod method;
        public Transform targetTransform;
        public ExposedReference<Transform> targetTransformTimeline;
        
        public Vector3 worldPosition;
        
        [Range(-1f, 1f)]
        public float normalizedYaw;
        
        [Range(-1f, 1f)]
        public float normalizedPitch;

        public static LookAtStatus Default
        {
            get
            {
                var status = new LookAtStatus();
                status.method = LookAtMethod.Direction;
                status.direction = LookAtDirection.Forward;
                status.weight = 1f;
                return status;
            }
        }

        public static LookAtStatus LookForward
        {
            get
            {
                var status = new LookAtStatus();
                status.weight = 1f;
                status.method = LookAtMethod.Direction;
                status.direction = LookAtDirection.Forward;
                return status;
            }
        }

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