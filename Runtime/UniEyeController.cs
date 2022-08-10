using System;
using System.Collections.Generic;
using System.Linq;
using UniEyeController.Core.Constants;
using UniEyeController.Core.Extensions;
using UniEyeController.Core.Rotator;
using UniEyeController.Core.Setting;
using UniEyeController.Core.Status;
using UnityEngine;
using UnityEngine.Timeline;

namespace UniEyeController
{
    [ExecuteAlways]
    [DisallowMultipleComponent]
    public class UniEyeController : MonoBehaviour, ITimeControl
    {
        public EyeAssignMethod assignMethod = EyeAssignMethod.Humanoid;
        
        public Animator animator;

        public GameObject prefabForGenericAvatar;
        
        public Transform manualEyeL;
        public Transform manualEyeR;
        
        public EyeRangeSetting rangeSetting;

        public Transform CurrentEyeL => _defaultStatus.EyeL.Bone;
        public Transform CurrentEyeR => _defaultStatus.EyeR.Bone;

        private DoubleEyeDefaultStatus _defaultStatus;

        private List<EyeProcess.EyeProcessBase> _processes = new List<EyeProcess.EyeProcessBase>();

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
            
            var rotator = new DoubleEyeRotator(_defaultStatus, rangeSetting);
            foreach (var process in _processes)
            {
                process.Rotator = rotator;
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

            _processes = GetComponents<EyeProcess.EyeProcessBase>().ToList();
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