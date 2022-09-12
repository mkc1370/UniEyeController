using System;
using UniEyeController.Core.Controller.Eye;
using UniEyeController.Core.Controller.Eye.Constants;
using UniEyeController.Core.Controller.Eyelid;
using UniEyeController.Core.Process.Core;
using UnityEngine;
using Random = UnityEngine.Random;

namespace UniEyeController.Core.Process.MicroMove
{
    /// <summary>
    /// 眼球微細運動を再現する
    /// </summary>
    [Serializable]
    public class MicroMoveProcess : EyeProcessBase<MicroMoveProcessSetting, MicroMoveProcessStatus>
    {
        private float _eyeMoveTimer;

        private Vector2 _currentNormalizedEulerAngles;

        protected override void ProgressInternal(double time, MicroMoveProcessStatus status)
        {
            _eyeMoveTimer -= Time.deltaTime;
            if (_eyeMoveTimer < 0)
            {
                var x = Random.Range(-1, 1);
                var y = Random.Range(-1, 1);
                _currentNormalizedEulerAngles = new Vector2(x, y) * setting.eyeMoveMultiplier;
                
                _eyeMoveTimer = Random.Range(setting.eyeMoveStopTimeMin, setting.eyeMoveStopTimeMax);
            }
            
            EyeController.NormalizedRotate(_currentNormalizedEulerAngles, status.weight, RotationApplyMethod.Append);
        }
    }
}
