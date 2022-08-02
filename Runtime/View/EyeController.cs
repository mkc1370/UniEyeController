using System;
using System.Collections.Generic;
using System.Linq;
using SimpleEyeController.Constants;
using SimpleEyeController.Model.Extensions;
using SimpleEyeController.Model.Rotator;
using SimpleEyeController.Model.Setting;
using SimpleEyeController.View.Process.Interface;
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
        [Header("アップデート方法")]
        public UpdateMethod updateMethod = UpdateMethod.LateUpdate;

        [Header("目のボーンの指定方法")]
        public EyeAssignMethod assignMethod = EyeAssignMethod.Animator;
        
        [Header("現在は目のボーンとAnimatorのRotationの取得用")]
        public Animator animator;

        [Header("手動で割り当てる場合の左目のボーン")]
        public Transform manualEyeL;
        
        [Header("手動で割り当てる場合の右目のボーン")]
        public Transform manualEyeR;
        
        public EyeRangeSetting rangeSetting;

        private List<IEyeProcess> _processes = new List<IEyeProcess>();

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
            
            GetEyeBones(out var eyeL, out var eyeR);
            
            var rotator = new DoubleEyeRotator(eyeL, eyeR, rangeSetting);
            foreach (var process in _processes)
            {
                process.Rotator = rotator;
            }
        }
        
        private void GetEyeBones(out Transform eyeL, out Transform eyeR)
        {
            switch (assignMethod)
            {
                case EyeAssignMethod.Animator:
                    eyeL = animator.GetBoneTransform(HumanBodyBones.LeftEye);
                    eyeR = animator.GetBoneTransform(HumanBodyBones.RightEye);
                    break;
                case EyeAssignMethod.Transform:
                    eyeL = manualEyeL;
                    eyeR = manualEyeR;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void UpdateInternal()
        {
            foreach (var process in _processes.OrderBy(x=>x.ExecutionOrder))
            {
                process.Progress();
            }
        }

        private void Update()
        {
            if (updateMethod == UpdateMethod.Update)
            {
                UpdateInternal();
            }
        }

        private void LateUpdate()
        {
            if (updateMethod == UpdateMethod.LateUpdate)
            {
                UpdateInternal();
            }
        }

        private void FixedUpdate()
        {
            if (updateMethod == UpdateMethod.FixedUpdate)
            {
                UpdateInternal();
            }
        }

        public void ManualUpdate()
        {
            if (updateMethod == UpdateMethod.Manual)
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

            _processes = GetComponents<IEyeProcess>().ToList();
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