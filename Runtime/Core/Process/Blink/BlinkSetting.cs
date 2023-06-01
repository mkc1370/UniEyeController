using System;
using UniEyeController.Core.Process.Core;
using UnityEngine;

namespace UniEyeController.Core.Process.Blink
{
    [Serializable]
    public class BlinkSetting : EyeSettingBase
    {
        [Range(0f, 1f)]
        public float eyeMoveMultiplier = 0.8f;
        
        [Range(0f, 10f)]
        public float eyeBlinkStopTimeMin = 3f;
        [Range(0f, 10f)]
        public float eyeBlinkStopTimeMax = 8f;

        /// <summary>
        /// 目を閉じるのにかかる時間
        /// </summary>
        [Range(0f, 2f)]
        public float timeToCloseEyelidMin = 0.04f;
        [Range(0f, 2f)]
        public float timeToCloseEyelidMax = 0.04f;
        [Range(0f, 2f)]
        public float timeToOpenEyelidMin = 0.2f;
        [Range(0f, 2f)]
        public float timeToOpenEyelidMax = 0.2f;

        /// <summary>
        /// まばたき中に眼球を動かすかどうか
        /// </summary>
        public bool moveEyeWithBlink = true;
    }
}