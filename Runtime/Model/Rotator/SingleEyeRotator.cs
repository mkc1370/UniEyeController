using SimpleEyeController.Constants;
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
        private EyeRangeSetting _setting;
    
        private Transform _eyeBone;
        private Quaternion _defaultRotation;
        private Quaternion _defaultLocalRotation;

        private Vector2 _currentEulerAngles;
        private EyeType _eyeType;

        public SingleEyeRotator(Transform eyeBone, EyeRangeSetting setting, EyeType eyeType)
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
            LookAt(worldPosition, GetLookAtWeight(worldPosition));
        }

        /// <summary>
        /// ターゲットの方向を向く
        /// </summary>
        /// <param name="worldPosition"></param>
        /// <param name="weight"></param>
        public void LookAt(Vector3 worldPosition, float weight)
        {
            var eulerAngles = GetEyeEulerAngles(worldPosition) * weight;

            Rotate(eulerAngles);
        }
        
        /// <summary>
        /// 目の向くべき角度を計算する
        /// ワールド座標から目の正面を+z・上を+yとする座標系に変換する
        /// </summary>
        /// <param name="worldPosition"></param>
        /// <returns></returns>
        private Vector2 GetEyeEulerAngles(Vector3 worldPosition)
        {
            var localPosition = GetLocalPosition(worldPosition);
            var yaw = Mathf.Atan2(localPosition.x, localPosition.z) * Mathf.Rad2Deg;
            var pitch = Mathf.Atan2(localPosition.y, localPosition.z) * Mathf.Rad2Deg;
            return new Vector2(yaw, pitch);
        }
        
        private Vector3 GetLocalPosition(Vector3 worldPosition)
        {
            // 目の正面方向を+zとするように回転させる
            var rotation =
                // 目のボーンの親の現在のrotation
                _eyeBone.rotation * Quaternion.Inverse(_eyeBone.localRotation) *
                // 目のボーンの親の初期状態のrotation
                Quaternion.Inverse(_defaultRotation * Quaternion.Inverse(_defaultLocalRotation));
            
            // 変換行列
            var matrix = Matrix4x4.TRS(_eyeBone.position, rotation, Vector3.one);
            
            // matrixの座標系での位置を取得する
            return matrix.inverse.MultiplyPoint(worldPosition);
        }

        /// <summary>
        /// ターゲットの方向を向いたときの視線の追従度合いを計算する
        /// ターゲットまでの距離や角度に応じて計算する
        /// これにより見るのをやめるときの動作をスムーズにできる
        /// </summary>
        /// <param name="worldPosition"></param>
        /// <returns></returns>
        public float GetLookAtWeight(Vector3 worldPosition)
        {
            // 向くのをやめはじめる距離
            // 向くのを完全にやめる距離
            var localPosition = GetLocalPosition(worldPosition);
            var distanceWeight = 1 - Mathf.InverseLerp(0.5f, 0f, localPosition.magnitude);

            // 角度が大きすぎる場合は向くのをやめる
            var eulerAngles = GetEyeEulerAngles(worldPosition);
            var limit = _setting.eulerAnglesLimit;
            var yawWeight = 1 - Mathf.InverseLerp(limit.x, 80f, Mathf.Abs(eulerAngles.x));
            var pitchWeight = 1 - Mathf.InverseLerp(limit.y, 80f, Mathf.Abs(eulerAngles.y));

            return distanceWeight * yawWeight * pitchWeight;
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