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

        private float _blinkValueOnBlinkStart;

        /// <summary>
        /// 強制的に目を開けた状態に戻す
        /// </summary>
        public void ForceReset()
        {
            SetBlink(0);
        }
        
        protected override void ProgressInternal(double time)
        {
            var deltaTime = (float) (time - _prevTime);
            _prevTime = (float)time;
            if (deltaTime < 0)
            {
                deltaTime = 0;
            }

            if (status.blinkOffFromOutside)
            {
                SetBlink(0);
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
                        _blinkValueOnBlinkStart = EyelidController.GetBlink();
                    }
                    break;
                case EyeBlinkState.Closing:
                    SetBlink(Remap(1f - _eyeTime / setting.timeToCloseEyelid, 0, 1, _blinkValueOnBlinkStart, 1));
                    if (_eyeTime <= 0)
                    {
                        _eyeBlinkState = EyeBlinkState.Opening;
                        _eyeTime = setting.timeToOpenEyelid;
                        // 完全に閉じる
                        SetBlink(Remap(1, 0, 1, _blinkValueOnBlinkStart, 1));
                    }
                    break;
                case EyeBlinkState.Opening:
                    SetBlink(Remap(_eyeTime / setting.timeToOpenEyelid, 0, 1, _blinkValueOnBlinkStart, 1));
                    if (_eyeTime <= 0)
                    {
                        _eyeBlinkState = EyeBlinkState.Idle;
                        _eyeTime = Random.Range(setting.eyeBlinkStopTimeMin, setting.eyeBlinkStopTimeMax);
                        // 完全に開く
                        SetBlink(Remap(0, 0, 1, _blinkValueOnBlinkStart, 1));
                    }
                    break;
            }
        }

        private void SetBlink(float value)
        {
            EyelidController.SetBlink(value * setting.weight, OnBlink);
            if (setting.moveEyeWithBlink)
            {
                EyeController.NormalizedRotate(Vector2.down * (value * setting.eyeMoveMultiplier), setting.weight, RotationApplyMethod.Append);
            }
        }
        
        /// <summary>
        /// Remap
        /// </summary>
        /// <param name="value"></param>
        /// <param name="from1"></param>
        /// <param name="to1"></param>
        /// <param name="from2"></param>
        /// <param name="to2"></param>
        /// <returns></returns>
        private static float Remap(float value, float from1, float to1, float from2, float to2)
        {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }
    }
}
