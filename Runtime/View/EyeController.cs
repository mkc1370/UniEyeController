using System;
using SimpleEyeController.Constants;
using SimpleEyeController.Model.Extensions;
using SimpleEyeController.Model.Process;
using SimpleEyeController.Model.Rotator;
using SimpleEyeController.Model.Setting;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Timeline;

namespace SimpleEyeController.View
{
    /// <summary>
    /// 回転軸やローカルの回転がめちゃくちゃでもいい感じに視線制御するサンプル
    /// Start()時に顔が正面に向いている必要があります(TスタンスやAスタンスであれば大丈夫です)
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(EyeMicroRotator))]
    [RequireComponent(typeof(EyeLookAt))]
    public class EyeController : MonoBehaviour, ITimeControl
    {
        [Header("現在は目のボーンとAnimatorのRotationの取得用")]
        public Animator animator;
        
        public EyeControllerSetting setting;

        [Header("注視点の設定")]
        private EyeLookAt _lookAt;
        
        [Header("眼球微細運動の設定")]
        private EyeMicroRotator _microRotator;

        #if UNITY_EDITOR
        [ContextMenu(nameof(SelectEyeBones))]
        private void SelectEyeBones()
        {
            Selection.activeObject = animator.GetBoneTransform(HumanBodyBones.LeftEye).gameObject;
        }
        #endif

        private void Start()
        {
            GetRequiredComponents();
            
            var eyeL = animator.GetBoneTransform(HumanBodyBones.LeftEye);
            var eyeR = animator.GetBoneTransform(HumanBodyBones.RightEye);
            if (eyeL == null || eyeR == null)
            {
                throw new Exception($"Both eyes must be assigned to Avatar.");
            }

            var rotator = new DoubleEyeRotator(eyeL, eyeR, setting);
            _lookAt.Rotator = rotator;
            _microRotator.Rotator = rotator;
        }

        private void UpdateInternal()
        {
            _lookAt.Progress();
            _microRotator.Progress();
        }

        private void Update()
        {
            if (setting.updateMethod == UpdateMethod.Update)
            {
                UpdateInternal();
            }
        }

        private void LateUpdate()
        {
            if (setting.updateMethod == UpdateMethod.LateUpdate)
            {
                UpdateInternal();
            }
        }

        private void FixedUpdate()
        {
            if (setting.updateMethod == UpdateMethod.FixedUpdate)
            {
                UpdateInternal();
            }
        }

        public void ManualUpdate()
        {
            if (setting.updateMethod == UpdateMethod.Manual)
            {
                UpdateInternal();
            }
        }

        private void Reset()
        {
            GetRequiredComponents();
        }

        private void GetRequiredComponents()
        {
            if (animator == null)
            {
                animator = gameObject.GetOrAddComponent<Animator>();
            }

            if (_lookAt == null)
            {
                _lookAt = gameObject.GetOrAddComponent<EyeLookAt>();
            }

            if (_microRotator == null)
            {
                _microRotator = gameObject.GetOrAddComponent<EyeMicroRotator>();
            }
        }

        public void SetTime(double time)
        {
            throw new NotImplementedException();
        }

        public void OnControlTimeStart()
        {
            throw new NotImplementedException();
        }

        public void OnControlTimeStop()
        {
            throw new NotImplementedException();
        }
    }
}