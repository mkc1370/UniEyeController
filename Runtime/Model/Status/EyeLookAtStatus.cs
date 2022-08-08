using System;
using SimpleEyeController.Constants;
using UnityEngine;

namespace SimpleEyeController.Model.Status
{
    [Serializable]
    public struct EyeLookAtStatus
    {
        [Header("視線制御の適用度")]
        [Range(0f, 1f)] public float weight;

        [Header("指定方法")]
        public LookAtMethod method;
    
        [Header("見る対象（Transform）")]
        public Transform target;
        
        [Header("見る対象（ワールド座標）")]
        public Vector3 worldPosition;

        [Header("目の角度（左右） [-1, 1]")]
        [Range(-1f, 1f)]
        public float normalizedYaw;
        
        [Header("目の角度（上下） [-1, 1]")]
        [Range(-1f, 1f)]
        public float normalizedPitch;
        
        [Header("目の方向")]
        public EyeLookAtDirection direction;

        public static EyeLookAtStatus Default =>
            new EyeLookAtStatus()
            {
                weight = 1f,
                method = LookAtMethod.Rotation,
                target = null,
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
                    if (target != null)
                    {
                        return $"T : {target.name}";
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
                        if (target == null)
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