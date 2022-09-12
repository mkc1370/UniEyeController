using System;
using UniEyeController.Core.Controller.Eye;
using UniEyeController.Core.Controller.Eye.Constants;
using UniEyeController.Core.Controller.Eyelid;
using UniEyeController.Core.Process.Core;
using UnityEngine;
using Random = UnityEngine.Random;

namespace UniEyeController.Core.Process.Blink
{
    [Serializable]
    public class BlinkProcess : EyeProcessBase<BlinkProcessSetting ,BlinkProcessStatus>
    {
        public BlinkProcess(DoubleEyeController eyeController, EyelidController eyelidController) : base(eyeController, eyelidController)
        {
        }
        
        /// <summary>
        /// 目を閉じるときのイベント
        /// EyelidTypeがManualの場合に使用される
        /// </summary>
        public Action<float> OnBlink;

        private EyeBlinkState _eyeBlinkState;
        
        private enum EyeBlinkState
        {
            Idle,
            Closing,
            Opening
        }
        
        private float _eyeTime;
        
        protected override void ProgressInternal(double time, BlinkProcessStatus status)
        {
            _eyeTime -= Time.deltaTime;
            
            switch (_eyeBlinkState)
            {
                case EyeBlinkState.Idle:
                    if (_eyeTime <= 0)
                    {
                        _eyeBlinkState = EyeBlinkState.Closing;
                        _eyeTime = setting.timeToCloseEyelid;
                    }
                    break;
                case EyeBlinkState.Closing:
                    Blink(1f - _eyeTime / setting.timeToCloseEyelid, status);
                    if (_eyeTime <= 0)
                    {
                        _eyeBlinkState = EyeBlinkState.Opening;
                        _eyeTime = setting.timeToOpenEyelid;
                        // 完全に閉じる
                        Blink(1, status);
                    }
                    break;
                case EyeBlinkState.Opening:
                    Blink(_eyeTime / setting.timeToOpenEyelid, status);
                    if (_eyeTime <= 0)
                    {
                        _eyeBlinkState = EyeBlinkState.Idle;
                        _eyeTime = Random.Range(setting.eyeBlinkStopTimeMin, setting.eyeBlinkStopTimeMax);
                        // 完全に開く
                        Blink(0, status);
                    }
                    break;
            }
        }

        private void Blink(float value, BlinkProcessStatus status)
        {
            EyelidController.Blink(value * status.weight, OnBlink);
            if (setting.moveEyeWithBlink)
            {
                EyeController.NormalizedRotate(Vector2.up * (value * setting.eyeMoveMultiplier), status.weight, RotationApplyMethod.Append);
            }
        }
    }
}
