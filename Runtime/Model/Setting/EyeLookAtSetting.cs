using System;
using UnityEngine;

namespace SimpleEyeController.Model.Setting
{
    [Serializable]
    public class EyeLookAtSetting
    {
        [Header("ターゲットを見るか角度で指定するか")]
        public bool useTarget;
    
        [Header("見る対象")]
        public Transform target;

        [Header("目の角度（左右） [-1, 1]")]
        [Range(-1f, 1f)]
        public float normalizedYaw;
        
        [Header("目の角度（上下） [-1, 1]")]
        [Range(-1f, 1f)]
        public float normalizedPitch;
    }
}