using System;
using UniEyeController.Core.Constants;
using UniEyeController.Core.Setting;
using UniEyeController.Core.Status;
using UnityEngine;

namespace UniEyeController.Core.Rotator
{
    /// <summary>
    /// 片目の視線制御
    /// 初期化時に顔が正面に向いている必要があります(TスタンスやAスタンスであれば大丈夫です)
    /// </summary>
    public class SingleEyeController
    {
        private EyeSetting _setting;
    
        private Transform _eyeBone;
        private Quaternion _defaultRotation;
        private Quaternion _defaultLocalRotation;

        private Vector2 _currentEulerAngles;
        private EyeType _eyeType;

        public SingleEyeController(SingleEyeDefaultStatus defaultStatus, EyeSetting setting)
        {
            _eyeBone = defaultStatus.Bone;
            _defaultRotation = defaultStatus.Rotation;
            _defaultLocalRotation = defaultStatus.LocalRotation;
        
            _setting = setting;
            _eyeType = defaultStatus.EyeType;
        }

        /// <summary>
        /// ターゲットの方向を向く
        /// </summary>
        /// <param name="worldPosition"></param>
        /// <param name="weight"></param>
        /// <param name="method"></param>
        public void LookAt(Vector3 worldPosition, float weight, RotationApplyMethod method)
        {
            var eulerAngles = GetEyeEulerAngles(worldPosition) * GetLookAtWeight(worldPosition);

            Rotate(eulerAngles, weight, method);
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
            var distanceWeight = 1 - Mathf.InverseLerp(0.2f, 0f, localPosition.magnitude);

            // 角度が大きすぎる場合は向くのをやめる
            var eulerAngles = GetEyeEulerAngles(worldPosition);
            float yawWeight;
            if (eulerAngles.x > 0)
            {
                yawWeight = 1 - Mathf.InverseLerp(80f, 90f, eulerAngles.x);
            }
            else
            {
                yawWeight = 1 - Mathf.InverseLerp(-80f, -90f, eulerAngles.x);
            }

            float pitchWeight;
            if (eulerAngles.y > 0)
            {
                pitchWeight = 1 - Mathf.InverseLerp(80f, 90f, eulerAngles.y);
            }
            else
            {
                pitchWeight = 1 - Mathf.InverseLerp(-80f, -90f, eulerAngles.y);
            }

            return distanceWeight * yawWeight * pitchWeight;
        }

        /// <summary>
        /// 目を回転させる
        /// </summary>
        public void Rotate(Vector2 eulerAngles, float weight, RotationApplyMethod method)
        {
            switch (method)
            {
                case RotationApplyMethod.Direct:
                    _currentEulerAngles = eulerAngles * weight;
                    break;
                case RotationApplyMethod.Append:
                    _currentEulerAngles += eulerAngles * weight;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(method), method, null);
            }

            Apply();
        }

        public void NormalizedRotate(Vector2 normalizedEulerAngles, float weight, RotationApplyMethod method)
        {
            Rotate(_setting.GetEulerAnglesFromNormalized(_eyeType, normalizedEulerAngles), weight, method);
        }

        private void Apply()
        {
            var eulerAngles = _setting.GetClampedEulerAngles(_eyeType, _currentEulerAngles);
            
            _eyeBone.localRotation =
                _defaultLocalRotation *
                Quaternion.Inverse(_defaultRotation) *
                Quaternion.AngleAxis(eulerAngles.x, Vector3.up) *
                Quaternion.AngleAxis(eulerAngles.y, Vector3.left) *
                _defaultRotation;
        }
    }
}