using System;
using UniEyeController.Core.Process.Core;
using UnityEngine;

namespace UniEyeController.Core.Process.Blink
{
    [Serializable]
    public class BlinkProcessSetting : EyeProcessSettingBase
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
        public float timeToCloseEyelid = 0.04f;
        public float timeToOpenEyelid = 0.09f;
        
        /// <summary>
        /// まばたき中に眼球を動かすかどうか
        /// </summary>
        public bool moveEyeWithBlink = true;
    }
}