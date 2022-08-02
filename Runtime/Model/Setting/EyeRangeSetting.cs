using System;
using SimpleEyeController.Constants;
using UnityEngine;

namespace SimpleEyeController.Model.Setting
{
    /// <summary>
    /// 目の可動域の設定
    /// </summary>
    [Serializable]
    public class EyeRangeSetting
    {
        private const float MinEyeEulerAngles = 0.0f;
        private const float MaxEyeEulerAngles = 90.0f;
        
        /// <summary>
        /// 水平方向の鼻に近い側 [オイラー角]
        /// </summary>
        [Range(MinEyeEulerAngles, MaxEyeEulerAngles)]
        public float horizontalInside = 16f;
        
        /// <summary>
        /// 水平方向の鼻から遠い側 [オイラー角]
        /// </summary>
        [Range(MinEyeEulerAngles, MaxEyeEulerAngles)]
        public float horizontalOutside = 20f;
        
        /// <summary>
        /// 上方向 [オイラー角]
        /// </summary>
        [Range(MinEyeEulerAngles, MaxEyeEulerAngles)]
        public float verticalUp = 10f;
        
        /// <summary>
        /// 下方向 [オイラー角]
        /// </summary>
        [Range(MinEyeEulerAngles, MaxEyeEulerAngles)]
        public float verticalDown = 8f;

        public float GetYawLimitMax()
        {
            return Mathf.Max(horizontalInside, horizontalOutside);
        }

        public Vector2 GetYawLimit(EyeType eyeType)
        {
            var yawLimit = new Vector2();
            switch (eyeType)
            {
                case EyeType.Left:
                    yawLimit.x = -horizontalOutside;
                    yawLimit.y = horizontalInside;
                    break;
                case EyeType.Right:
                    yawLimit.x = -horizontalInside;
                    yawLimit.y = horizontalOutside;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(eyeType), eyeType, null);
            }

            return yawLimit;
        }

        public Vector2 PitchLimit()
        {
            return new Vector2(-verticalDown, verticalUp);
        }

        private float GetClampedYaw(EyeType eyeType, float yaw)
        {
            var maxYaw = Mathf.Max(horizontalInside, horizontalOutside);
            var normalizedYaw = Mathf.Clamp(yaw, -maxYaw, maxYaw) / maxYaw;
            return GetYawFromNormalized(eyeType, normalizedYaw);
        }
        
        private float GetClampedPitch(float pitch)
        {
            var pitchLimit = PitchLimit();
            var clampedPitch =
                Mathf.Clamp(pitch, pitchLimit.x, 0) +
                Mathf.Clamp(pitch, 0, pitchLimit.y);
            return clampedPitch;
        }

        public Vector2 GetClampedEulerAngles(EyeType eyeType ,Vector2 eulerAngles)
        {
            return new Vector2(
                GetClampedYaw(eyeType, eulerAngles.x),
                GetClampedPitch(eulerAngles.y)
            );
        }

        public float GetYawFromNormalized(EyeType eyeType, float normalizedYaw)
        {
            var yawLimit = GetYawLimit(eyeType);
            var yaw =
                Mathf.Lerp(0, yawLimit.x, -normalizedYaw) +
                Mathf.Lerp(0, yawLimit.y, normalizedYaw);
            return yaw;
        }
        
        public float GetPitchFromNormalized(float normalizedPitch)
        {
            var pitchLimit = PitchLimit();
            var pitch =
                Mathf.Lerp(0, pitchLimit.x, -normalizedPitch) +
                Mathf.Lerp(0, pitchLimit.y, normalizedPitch);
            return pitch;
        }

        public Vector2 GetEulerAnglesFromNormalized(EyeType eyeType, Vector2 normalizedEulerAngles)
        {
            var yaw = GetYawFromNormalized(eyeType, normalizedEulerAngles.x);
            var pitch = GetPitchFromNormalized(normalizedEulerAngles.y);
            return new Vector2(yaw, pitch);
        }
    }
}