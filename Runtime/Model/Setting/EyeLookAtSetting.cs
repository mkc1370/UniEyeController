﻿using System;
using UnityEngine;

namespace SimpleEyeController.Model.Setting
{
    [Serializable]
    public class EyeLookAtSetting
    {
        [Header("機能の有効化")]
        public bool enabled = true;
        
        [Header("ターゲットを見るか角度で指定するか")]
        public bool useTarget;
    
        [Header("見る対象")]
        public Transform target;

        [Header("目の角度（左右, 上下） [オイラー角]")]
        public Vector2 eulerAngles;
    }
}