using UniEyeController.Core.Constants;
using UniEyeController.Core.Status;
﻿using System;
using UniEyeController.Core.Constants;
using UnityEngine;
using Random = UnityEngine.Random;

namespace UniEyeController.EyeProcess
{
    /// <summary>
    /// 眼球微細運動を再現する
    /// </summary>
    [Serializable]
    public class UniEyeMicroMove : UniEyeProcessBase
    {
        [Range(0f, 1f)]
        public float weight = 1f;
        
        [Range(0f, 1f)]
        public float eyeMoveMultiplier = 0.1f;
        
        [Range(0f, 5f)]
        public float eyeMoveStopTimeMin = 0.5f;
        [Range(0f, 5f)]
        public float eyeMoveStopTimeMax = 2.3f;
        
        private float _eyeMoveTimer;


        private Vector2 _currentNormalizedEulerAngles;

        public override void Progress(double time, IEyeStatus? status)
        {
            if (!CanExecute && status == null) return;
            
            _eyeMoveTimer -= Time.deltaTime;
            if (_eyeMoveTimer < 0)
            {
                var x = Random.Range(-1, 1);
                var y = Random.Range(-1, 1);
                _currentNormalizedEulerAngles = new Vector2(x, y) * eyeMoveMultiplier;
                
                _eyeMoveTimer = Random.Range(eyeMoveStopTimeMin, eyeMoveStopTimeMax);
            }
            
            EyeController.NormalizedRotate(_currentNormalizedEulerAngles, weight, RotationApplyMethod.Append);
        }
    }
}
