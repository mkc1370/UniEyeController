using SimpleEyeController.Constants;
using SimpleEyeController.Model.Setting;
using SimpleEyeController.Model.Status;
using UnityEngine;

namespace SimpleEyeController.Model.Rotator
{
    /// <summary>
    /// 両目の視線制御
    /// 初期化時に顔が正面に向いている必要があります(TスタンスやAスタンスであれば大丈夫です)
    /// </summary>
    public class DoubleEyeRotator
    {
        private SingleEyeRotator _eyeL;
        private SingleEyeRotator _eyeR;

        /// <summary>
        /// 初期化
        /// </summary>
        public DoubleEyeRotator(EyeDefaultStatus eyeL, EyeDefaultStatus eyeR, EyeRangeSetting setting)
        {
            _eyeL = new SingleEyeRotator(eyeL, setting);
            _eyeR = new SingleEyeRotator(eyeR, setting);
        }

        /// <summary>
        /// ターゲットの方向を向く
        /// </summary>
        /// <param name="worldPosition">見る対象</param>
        /// <param name="weight"></param>
        public void LookAt(Vector3 worldPosition, float weight = 1f)
        {
            // 目の追従度合いは左右の小さい方に合わせる
            var minWeight = Mathf.Min(_eyeL.GetLookAtWeight(worldPosition), _eyeR.GetLookAtWeight(worldPosition));
            _eyeL.LookAt(worldPosition, minWeight * weight);
            _eyeR.LookAt(worldPosition, minWeight * weight);
        }
        
        public void AppendNormalizedRotate(Vector2 normalizedEulerAngles)
        {
            _eyeL.AppendNormalizedRotate(normalizedEulerAngles);
            _eyeR.AppendNormalizedRotate(normalizedEulerAngles);
        }
        
        public void NormalizedRotate(Vector2 normalizedEulerAngles)
        {
            _eyeL.NormalizedRotate(normalizedEulerAngles);
            _eyeR.NormalizedRotate(normalizedEulerAngles);
        }

        /// <summary>
        /// 目を回転させる
        /// </summary>
        public void Rotate(Vector2 eulerAngles)
        {
            _eyeL.Rotate(eulerAngles);
            _eyeR.Rotate(eulerAngles);
        }
        
        public void AppendRotate(Vector2 eulerAngles)
        {
            _eyeL.AppendRotate(eulerAngles);
            _eyeR.AppendRotate(eulerAngles);
        }
    }
}
