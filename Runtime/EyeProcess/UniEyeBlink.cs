using System;
using UniEyeController.Core.Constants;
using UniEyeController.Core.Status;
using UnityEngine;
using Random = UnityEngine.Random;

namespace UniEyeController.EyeProcess
{
    [Serializable]
    public class UniEyeBlink : UniEyeProcessBase
    {
        [Range(0f, 1f)]
        public float weight = 1f;
        
        [Range(0f, 1f)]
        public float eyeMoveMultiplier = 0.8f;
        
        [Range(0f, 10f)]
        public float eyeBlinkStopTimeMin = 3f;
        [Range(0f, 10f)]
        public float eyeBlinkStopTimeMax = 8f;

        /// <summary>
        /// 目を閉じるのにかかる時間
        /// </summary>
        public float timeToCloseEyelid = 0.04f;
        public float timeToOpenEyelid = 0.09f;
        
        /// <summary>
        /// まばたき中に眼球を動かすかどうか
        /// </summary>
        public bool moveEyeWithBlink = true;

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

        private void Start()
        {
            // TODO : これも無理やりなので修正する
            updateMethod = UpdateMethod.Update;
        }
        
        public override void Progress(double time, IEyeStatus statusFromTimeline)
        {
            // TODO : これも無理やりなので修正する
            moveEyeWithBlink = false;
            if (!CanExecute) return;
            if (EyeController == null) return;
            if (EyelidController == null) return;
            
            // TODO : これも無理やりなので修正する
            if (statusFromTimeline != null)
            {
                if (((UniEyeBlinkStatus)statusFromTimeline).ForceBlinkOff)
                {
                    Blink(0);
                    _eyeBlinkState = EyeBlinkState.Idle;
                    _eyeTime = eyeBlinkStopTimeMax;
                }
                
                return;
            }
            
            _eyeTime -= Time.deltaTime;
            
            switch (_eyeBlinkState)
            {
                case EyeBlinkState.Idle:
                    if (_eyeTime <= 0)
                    {
                        _eyeBlinkState = EyeBlinkState.Closing;
                        _eyeTime = timeToCloseEyelid;
                    }
                    break;
                case EyeBlinkState.Closing:
                    Blink(1f - _eyeTime / timeToCloseEyelid);
                    if (_eyeTime <= 0)
                    {
                        _eyeBlinkState = EyeBlinkState.Opening;
                        _eyeTime = timeToOpenEyelid;
                        // 完全に閉じる
                        Blink(1);
                    }
                    break;
                case EyeBlinkState.Opening:
                    Blink(_eyeTime / timeToOpenEyelid);
                    if (_eyeTime <= 0)
                    {
                        _eyeBlinkState = EyeBlinkState.Idle;
                        _eyeTime = Random.Range(eyeBlinkStopTimeMin, eyeBlinkStopTimeMax);
                        // 完全に開く
                        Blink(0);
                    }
                    break;
            }
        }

        private void Blink(float value)
        {
            EyelidController.Blink(value * weight, OnBlink);
            if (moveEyeWithBlink)
            {
                EyeController.NormalizedRotate(Vector2.up * value * eyeMoveMultiplier, weight, RotationApplyMethod.Append);
            }
        }
    }
}
