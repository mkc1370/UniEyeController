﻿using SimpleEyeController.Model.Setting;
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
        public DoubleEyeRotator(Transform eyeL, Transform eyeR, EyeControllerSetting setting)
        {
            _eyeL = new SingleEyeRotator(eyeL, setting, EyeType.Left);
            _eyeR = new SingleEyeRotator(eyeR, setting, EyeType.Right);
        }

        /// <summary>
        /// ターゲットの方向を向く
        /// </summary>
        /// <param name="worldPosition">見る対象</param>
        public void LookAt(Vector3 worldPosition)
        {
            // 目の追従度合いは左右の小さい方に合わせる
            var weight = Mathf.Min(_eyeL.GetLookAtWeight(worldPosition), _eyeR.GetLookAtWeight(worldPosition));
            _eyeL.LookAt(worldPosition, weight);
            _eyeR.LookAt(worldPosition, weight);
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