﻿using System;
using SimpleEyeController.Constants;
using UnityEngine;

namespace SimpleEyeController.Model.Setting
{
    [Serializable]
    public class EyeLookAtSetting
    {
        [Header("視線制御の適用度")]
        [Range(0f, 1f)]
        public float weight = 1f;
        
        [Header("指定方法")]
        public LookAtMethod method;
    
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