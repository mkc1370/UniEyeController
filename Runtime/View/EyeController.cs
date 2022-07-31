using System;
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
    public class EyeController : MonoBehaviour, ITimeControl
    {
        [Header("現在は目のボーンとAnimatorのRotationの取得用")]
        public Animator animator;
        
        public EyeControllerSetting setting;

        [Header("注視点の設定")]
        [SerializeField] protected EyeLookAt lookAt;
        
        [Header("眼球微細運動の設定")]
        [SerializeField] protected EyeMicroRotator microRotator;

        #if UNITY_EDITOR
        [ContextMenu(nameof(SelectEyeBones))]
        private void SelectEyeBones()
        {
            Selection.activeObject = animator.GetBoneTransform(HumanBodyBones.LeftEye).gameObject;
        }
        #endif

        private void Start()
        {
            var eyeL = animator.GetBoneTransform(HumanBodyBones.LeftEye);
            var eyeR = animator.GetBoneTransform(HumanBodyBones.RightEye);
            if (eyeL == null || eyeR == null)
            {
                throw new Exception($"Both eyes must be assigned to Avatar.");
            }

            var rotator = new DoubleEyeRotator(eyeL, eyeR, setting);
            lookAt.Rotator = rotator;
            microRotator.Rotator = rotator;
        }
        
        private void UpdateInternal()
        {
            lookAt.Progress();
            microRotator.Progress();
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
            animator = GetComponent<Animator>();
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