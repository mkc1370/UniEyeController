using System;
using UnityEngine;

namespace SimpleEyeController.Model.Setting
{
    [Serializable]
    public class EyeMicroRotatorSetting
    {
        [Header("機能の有効化")]
        public bool enabled = true;
        
        [Header("目の可動域の何倍の範囲で眼球微細運動をするか")]
        [Range(0f, 1f)]
        public float eyeMicroMoveMultiplier = 0.1f;
        
        [Header("眼球微細運動を止める時間の最小値")]
        public float eyeMoveStopTimeMin = 0.5f;
        
        [Header("眼球微細運動を止める時間の最大値")]
        public float eyeMoveStopTimeMax = 2.3f;
    }
}