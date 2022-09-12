using System;
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
        public UpdateMethod updateMethod = UpdateMethod.LateUpdate;
        
        public bool executeAlways;
        
        public bool CanExecute => Application.isPlaying || executeAlways;
        
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
        
        public UniEyeLookAt lookAt;
        public UniEyeMicroMove microMove;
        public UniEyeBlink blink;

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
            
            lookAt.EyeController = eyeController;
            microMove.EyeController = eyeController;
            blink.EyeController = eyeController;
            
            lookAt.EyelidController = eyelidController;
            microMove.EyelidController = eyelidController;
            blink.EyelidController = eyelidController;
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

        private void UpdateInternal()
        {
            if (CanExecute)
            {
                lookAt.Progress(Time.time, false);
                microMove.Progress(Time.time, false);
                blink.Progress(Time.time, false);
            }
        }

        private void Update()
        {
            UpdateInternal();
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
            UpdateInternal();
        }

        public void ManualUpdate()
        {
            UpdateInternal();
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
        }
    }
}
