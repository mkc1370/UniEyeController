using System;
using UnityEngine;

namespace SimpleEyeController.Model.Setting
{
    [Serializable]
    public class EyeMicroRotatorSetting
    {
        [Range(0f, 1f)]
        public float eyeMicroMoveMultiplier = 0.1f;
        
        public float eyeMoveStopTimeMin = 0.5f;
        public float eyeMoveStopTimeMax = 2.3f;
    }
}