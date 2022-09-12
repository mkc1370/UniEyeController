using System;
using UniEyeController.Core.Process.Core;
using UnityEngine;

namespace UniEyeController.Core.Process.MicroMove
{
    [Serializable]
    public class MicroMoveProcessSetting : EyeProcessSettingBase
    {
        [Range(0f, 1f)]
        public float eyeMoveMultiplier = 0.1f;
        
        [Range(0f, 5f)]
        public float eyeMoveStopTimeMin = 0.5f;
        [Range(0f, 5f)]
        public float eyeMoveStopTimeMax = 2.3f;
    }
}