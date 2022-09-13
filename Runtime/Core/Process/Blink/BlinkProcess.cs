using System;
using UniEyeController.Core.Controller.Eye.Constants;
using UniEyeController.Core.Process.Core;
using UnityEngine;
using Random = UnityEngine.Random;

namespace UniEyeController.Core.Process.Blink
{
    [Serializable]
    public class BlinkProcess : EyeProcessBase<BlinkSetting ,BlinkStatus>
    {
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

        private float _prevTime;
        private float _eyeTime;

        /// <summary>
        /// 強制的に目を開けた状態に戻す
        /// </summary>
        public void ForceReset()
        {
            Blink(0);
        }
        
        protected override void ProgressInternal(double time)
        {
            var deltaTime = (float) (time - _prevTime);
            _prevTime = (float)time;
            if (deltaTime < 0)
            {
                deltaTime = 0;
            }

            if (status.blinkOffFromTimeline)
            {
                Blink(0);
                return;
            }
            
            _eyeTime -= deltaTime;

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
                    Blink(1f - _eyeTime / setting.timeToCloseEyelid);
                    if (_eyeTime <= 0)
                    {
                        _eyeBlinkState = EyeBlinkState.Opening;
                        _eyeTime = setting.timeToOpenEyelid;
                        // 完全に閉じる
                        Blink(1);
                    }
                    break;
                case EyeBlinkState.Opening:
                    Blink(_eyeTime / setting.timeToOpenEyelid);
                    if (_eyeTime <= 0)
                    {
                        _eyeBlinkState = EyeBlinkState.Idle;
                        _eyeTime = Random.Range(setting.eyeBlinkStopTimeMin, setting.eyeBlinkStopTimeMax);
                        // 完全に開く
                        Blink(0);
                    }
                    break;
            }
        }

        private void Blink(float value)
        {
            EyelidController.Blink(value * setting.weight, OnBlink);
            if (setting.moveEyeWithBlink)
            {
                EyeController.NormalizedRotate(Vector2.up * (value * setting.eyeMoveMultiplier), setting.weight, RotationApplyMethod.Append);
            }
        }
    }
}
