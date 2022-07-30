using SimpleEyeController.Model.Setting;
using UnityEngine;

namespace SimpleEyeController.Model.Rotator
{
    /// <summary>
    /// 両目の視線制御
    /// 初期化時に顔が正面に向いている必要があります(TスタンスやAスタンスであれば大丈夫です)
    /// </summary>
    public class DoubleEyeRotator
    {
        private SingleEyeRotator _singleEyeRotatorL;
        private SingleEyeRotator _singleEyeRotatorR;

        /// <summary>
        /// 初期化
        /// </summary>
        public DoubleEyeRotator(Quaternion animatorRotation, Transform eyeL, Transform eyeR, EyeControllerSetting setting)
        {
            _singleEyeRotatorL = new SingleEyeRotator(animatorRotation, eyeL, setting, EyeType.Left);
            _singleEyeRotatorR = new SingleEyeRotator(animatorRotation, eyeR, setting, EyeType.Right);
        }

        /// <summary>
        /// ターゲットの方向を向く
        /// </summary>
        /// <param name="worldPosition">見る対象</param>
        public void LookAt(Vector3 worldPosition)
        {
            _singleEyeRotatorL.LookAt(worldPosition);
            _singleEyeRotatorR.LookAt(worldPosition);
        }
        
        public void AppendNormalizedRotate(Vector2 normalizedEulerAngles)
        {
            _singleEyeRotatorL.AppendNormalizedRotate(normalizedEulerAngles);
            _singleEyeRotatorR.AppendNormalizedRotate(normalizedEulerAngles);
        }
        
        public void NormalizedRotate(Vector2 normalizedEulerAngles)
        {
            _singleEyeRotatorL.NormalizedRotate(normalizedEulerAngles);
            _singleEyeRotatorR.NormalizedRotate(normalizedEulerAngles);
        }

        /// <summary>
        /// 目を回転させる
        /// </summary>
        public void Rotate(Vector2 eulerAngles)
        {
            _singleEyeRotatorL.Rotate(eulerAngles);
            _singleEyeRotatorR.Rotate(eulerAngles);
        }
        
        public void AppendRotate(Vector2 eulerAngles)
        {
            _singleEyeRotatorL.AppendRotate(eulerAngles);
            _singleEyeRotatorR.AppendRotate(eulerAngles);
        }
    }
}
