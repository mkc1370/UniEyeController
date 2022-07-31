using SimpleEyeController.Model.Setting;
using UnityEngine;

namespace SimpleEyeController.Model.Rotator
{
    /// <summary>
    /// 片目の視線制御
    /// 初期化時に顔が正面に向いている必要があります(TスタンスやAスタンスであれば大丈夫です)
    /// </summary>
    public class SingleEyeRotator
    {
        private EyeControllerSetting _setting;
    
        private Transform _eyeBone;
        private Quaternion _defaultRotation;
        private Quaternion _defaultLocalRotation;

        private Vector2 _currentEulerAngles;
        private EyeType _eyeType;

        public SingleEyeRotator(Transform eyeBone, EyeControllerSetting setting, EyeType eyeType)
        {
            _eyeBone = eyeBone;
            _defaultRotation = eyeBone.rotation;
            _defaultLocalRotation = eyeBone.localRotation;
        
            _setting = setting;
            _eyeType = eyeType;
        }

        /// <summary>
        /// ターゲットの方向を向く
        /// </summary>
        /// <param name="worldPosition"></param>
        public void LookAt(Vector3 worldPosition)
        {
            // 一度Y-Upに直してから方向を計算する
            _eyeBone.localRotation = _defaultLocalRotation * Quaternion.Inverse(_defaultRotation);
            var localPosition = _eyeBone.InverseTransformPoint(worldPosition);

            // どれだけターゲットの方向を見るかの重み(これにより見るのをやめるときの動作をスムーズにできる)
            var yaw = Mathf.Atan2(localPosition.x, localPosition.z) * Mathf.Rad2Deg;
            var pitch = Mathf.Atan2(localPosition.y, localPosition.z) * Mathf.Rad2Deg;

            // 向くのをやめはじめる距離
            // 向くのを完全にやめる距離
            var distanceWeight = 1 - Mathf.InverseLerp(0.5f, 0f, localPosition.magnitude);

            // 角度が大きすぎる場合は向くのをやめる
            var limit = _setting.eulerAnglesLimit;
            var yawWeight = 1 - Mathf.InverseLerp(limit.x, 80f, Mathf.Abs(yaw));
            var pitchWeight = 1 - Mathf.InverseLerp(limit.y, 80f, Mathf.Abs(pitch));
            
            var weight = distanceWeight * yawWeight * pitchWeight;
            var eulerAngles = new Vector2(yaw, pitch) * weight;

            Rotate(eulerAngles);
        }

        /// <summary>
        /// 目を回転させる
        /// </summary>
        public void Rotate(Vector2 eulerAngles)
        {
            _currentEulerAngles = eulerAngles;
            Apply();
        }

        public void AppendRotate(Vector2 eulerAngles)
        {
            _currentEulerAngles += eulerAngles;
            Apply();
        }

        public void AppendNormalizedRotate(Vector2 normalizedEulerAngles)
        {
            AppendRotate(Vector2.Scale(_setting.eulerAnglesLimit, normalizedEulerAngles));
        }

        public void NormalizedRotate(Vector2 normalizedEulerAngles)
        {
            Rotate(Vector2.Scale(_setting.eulerAnglesLimit, normalizedEulerAngles));
        }

        private void Apply()
        {
            var limit = _setting.eulerAnglesLimit;
            var adjustedYaw = Mathf.Clamp(_currentEulerAngles.x, -limit.x, limit.x);
            var adjustedPitch = Mathf.Clamp(_currentEulerAngles.y, -limit.y, limit.y);
            
            _eyeBone.localRotation =
                _defaultLocalRotation *
                Quaternion.Inverse(_defaultRotation) *
                Quaternion.AngleAxis(adjustedYaw, Vector3.up) *
                Quaternion.AngleAxis(adjustedPitch, Vector3.left) *
                _defaultRotation;
        }
    }
}