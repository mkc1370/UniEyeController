using System;
using System.Collections.Generic;
using System.Linq;
using UniEyeController.Core;
using UniEyeController.Core.Constants;
using UniEyeController.Core.Extensions;
using UniEyeController.Core.Rotator;
using UniEyeController.Core.Setting;
using UniEyeController.Core.Status;
using UniEyeController.EyeProcess;
using UnityEngine;

namespace UniEyeController
{
    [ExecuteAlways]
    [DisallowMultipleComponent]
    public class UniEyeController : MonoBehaviour
    {
        public EyeAssignMethod assignMethod = EyeAssignMethod.Humanoid;
        
        public Animator animator;

        public GameObject prefabForGenericAvatar;
        
        public Transform manualEyeL;
        public Transform manualEyeR;
        
        public EyeSetting setting;
        public EyelidSetting eyelidSetting;

        public Transform CurrentEyeL => _defaultStatus.EyeL.Bone;
        public Transform CurrentEyeR => _defaultStatus.EyeR.Bone;

        private DoubleEyeDefaultStatus _defaultStatus;

        private List<UniEyeProcessBase> _processes = new List<UniEyeProcessBase>();

        private void Start()
        {
            Init();
        }

        public void Init()
        {
            GetRequiredComponents();
            ChangeEyeBones();
        }

        public void ChangeEyeBones()
        {
            _defaultStatus = GetEyeDefaultStatusBones();
            
            var eyeController = new DoubleEyeController(_defaultStatus, setting);
            var eyelidController = new EyelidController(eyelidSetting);
            foreach (var process in _processes)
            {
                process.EyeController = eyeController;
                process.EyelidController = eyelidController;
            }
        }

        private DoubleEyeDefaultStatus GetEyeDefaultStatusBones()
        {
            switch (assignMethod)
            {
                case EyeAssignMethod.Humanoid:
                    return DoubleEyeDefaultStatus.CreateFromHumanoid(animator);
                case EyeAssignMethod.Generic:
                    return DoubleEyeDefaultStatus.CreateFromGeneric(
                        transform,
                        manualEyeL,
                        manualEyeR,
                        prefabForGenericAvatar
                    );
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void UpdateInternal(UpdateMethod updateMethod)
        {
            var processes =
                _processes
                    .Where(x => x.updateMethod == updateMethod)
                    .Where(x => x.CanExecute)
                    .OrderBy(x => x.executionOrder);
            
            foreach (var process in processes)
            {
                process.Progress(Time.time, false);
            }
        }

        private void Update()
        {
            // 一度Updateのタイミングで回転を0にする
            foreach (var process in _processes)
            {
                if (process.CanExecute)
                {
                    // TODO : これは雑すぎるので直す
                    if (process.EyeController == null)
                    {
                        Init();
                        return;
                    }
                    process.EyeController.Rotate(Vector2.zero, 1, RotationApplyMethod.Direct);
                }
            }
            
            UpdateInternal(UpdateMethod.Update);
        }

        private void LateUpdate()
        {
            UpdateInternal(UpdateMethod.LateUpdate);
        }

        private void FixedUpdate()
        {
            UpdateInternal(UpdateMethod.FixedUpdate);
        }

        public void ManualUpdate()
        {
            UpdateInternal(UpdateMethod.Manual);
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

            _processes = GetComponents<UniEyeProcessBase>().ToList();
        }
    }
}