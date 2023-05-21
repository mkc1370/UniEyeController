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
            NotInitialized,
            Idle,
            Closing,
            Opening
        }

        private float _prevTime;
        private float _eyeTime;

        private float _blinkValueOnBlinkStart;

        private float _timeToCloseEyelid;
        private float _timeToOpenEyelid;

        /// <summary>
        /// 強制的に目を開けた状態に戻す
        /// </summary>
        public void ForceReset()
        {
            SetBlink(0);
        }
        
        protected override void ProgressInternal(double time)
        {
            // 初回のまばたきまでの時間がランダムになるように
            if (_eyeBlinkState == EyeBlinkState.NotInitialized)
            {
                _eyeTime = Random.Range(setting.eyeBlinkStopTimeMin, setting.eyeBlinkStopTimeMax);
                _eyeBlinkState = EyeBlinkState.Idle;
            }
            
            var deltaTime = (float) (time - _prevTime);
            _prevTime = (float)time;
            if (deltaTime < 0)
            {
                deltaTime = 0;
            }

            if (status.blinkOffFromOutside)
            {
                return;
            }
            
            _eyeTime -= deltaTime;

            switch (_eyeBlinkState)
            {
                case EyeBlinkState.Idle:
                    if (_eyeTime <= 0)
                    {
                        _timeToCloseEyelid = Random.Range(setting.timeToCloseEyelidMin, setting.timeToCloseEyelidMax);
                        _timeToOpenEyelid = Random.Range(setting.timeToOpenEyelidMin, setting.timeToOpenEyelidMax);
                        
                        _eyeBlinkState = EyeBlinkState.Closing;
                        _eyeTime = _timeToCloseEyelid;
                        _blinkValueOnBlinkStart = EyelidController.GetBlink();
                    }
                    break;
                case EyeBlinkState.Closing:
                    var close = EaseInOutSine(1f - _eyeTime / _timeToCloseEyelid);
                    SetBlink(Remap(close, 0, 1, _blinkValueOnBlinkStart, 1));
                    if (_eyeTime <= 0)
                    {
                        _eyeBlinkState = EyeBlinkState.Opening;
                        _eyeTime = _timeToOpenEyelid;
                        // 完全に閉じる
                        SetBlink(Remap(1, 0, 1, _blinkValueOnBlinkStart, 1));
                    }
                    break;
                case EyeBlinkState.Opening:
                    var open = EaseInOutCubic(_eyeTime / _timeToOpenEyelid);
                    SetBlink(Remap(open, 0, 1, _blinkValueOnBlinkStart, 1));
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
        /// SineInOut
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        private float EaseInOutSine(float t)
        {
            return -(Mathf.Cos(Mathf.PI * t) - 1) / 2;
        }
        
        /// <summary>
        /// CubicInOut
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        private float EaseInOutCubic(float t)
        {
            return t < 0.5f ? 4 * t * t * t : (t - 1) * (2 * t - 2) * (2 * t - 2) + 1;
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
