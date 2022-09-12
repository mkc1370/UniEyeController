using UniEyeController.Core.Constants;
using UniEyeController.Core.Setting;
using UnityEngine;

namespace UniEyeController.Core.Controller.Eye
{
    /// <summary>
    /// 両目の視線制御
    /// 初期化時に顔が正面に向いている必要があります(TスタンスやAスタンスであれば大丈夫です)
    /// </summary>
    public class DoubleEyeController
    {
        private SingleEyeController _eyeL;
        private SingleEyeController _eyeR;

        /// <summary>
        /// 初期化
        /// </summary>
        public DoubleEyeController(DoubleEyeDefaultStatus defaultStatus, EyeSetting setting)
        {
            _eyeL = new SingleEyeController(defaultStatus.EyeL, setting);
            _eyeR = new SingleEyeController(defaultStatus.EyeR, setting);
        }

        /// <summary>
        /// ターゲットの方向を向く
        /// </summary>
        /// <param name="worldPosition">見る対象</param>
        /// <param name="weight"></param>
        /// <param name="method"></param>
        public void LookAt(Vector3 worldPosition, float weight, RotationApplyMethod method)
        {
            // 目の追従度合いは左右の小さい方に合わせる
            var adjustedWeight = Mathf.Min(_eyeL.GetLookAtWeight(worldPosition), _eyeR.GetLookAtWeight(worldPosition)) * weight;
            _eyeL.LookAt(worldPosition, adjustedWeight, method);
            _eyeR.LookAt(worldPosition, adjustedWeight, method);
        }
        
        /// <summary>
        /// 目を回転させる （[-1, 1]の範囲）
        /// (Yaw, Pitch)
        /// </summary>
        /// <param name="normalizedEulerAngles"></param>
        /// <param name="weight"></param>
        /// <param name="method"></param>
        public void NormalizedRotate(Vector2 normalizedEulerAngles, float weight, RotationApplyMethod method)
        {
            _eyeL.NormalizedRotate(normalizedEulerAngles, weight, method);
            _eyeR.NormalizedRotate(normalizedEulerAngles, weight, method);
        }

        /// <summary>
        /// 目を回転させる （オイラー角）
        /// (Yaw, Pitch)
        /// </summary>
        public void Rotate(Vector2 eulerAngles, float weight, RotationApplyMethod method)
        {
            _eyeL.Rotate(eulerAngles, weight, method);
            _eyeR.Rotate(eulerAngles, weight, method);
        }
    }
}
