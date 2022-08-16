using System;
using System.Collections;
using UniEyeController.Core.Constants;
using UniEyeController.Core.Status;
using UnityEngine;
using Random = UnityEngine.Random;

namespace UniEyeController.EyeProcess
{
    public class UniEyeBlink : UniEyeProcessBase
    {
        private void Reset()
        {
            executionOrder = 3;
        }

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
        
        private void Start()
        {
            StartCoroutine(EyeBlinkLoop());
        }
        
        public override void Progress(double time, IEyeStatus statusFromTimeline)
        {
        }

        private IEnumerator EyeBlinkLoop()
        {
            while (true)
            {
                // まばたきを始めるまで待つ
                yield return new WaitForSeconds(Random.Range(eyeBlinkStopTimeMin, eyeBlinkStopTimeMax));
                
                // 閉じ始める
                var closeTime = timeToCloseEyelid;
                while (closeTime > 0)
                {
                    if (enabled)
                    {
                        Blink(1f - closeTime / timeToCloseEyelid);
                    }
                    closeTime -= Time.deltaTime;
                    yield return null;
                }
                
                // 完全に閉じる
                Blink(1);
                yield return null;
                
                var openTime = timeToOpenEyelid;
                while (openTime > 0)
                {
                    if (enabled)
                    {
                        Blink(openTime / timeToOpenEyelid);
                    }
                    openTime -= Time.deltaTime;
                    yield return null;
                }
                
                // 完全に開く
                Blink(0);
                yield return null;
            }
        }

        private void Blink(float value)
        {
            EyelidController.Blink(value * weight, OnBlink);
            if (moveEyeWithBlink)
            {
                EyeController.NormalizedRotate(Vector2.down * value * eyeMoveMultiplier, weight, RotationApplyMethod.Append);
            }
        }
    }
}