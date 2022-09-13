using System;
using UniEyeController.Core.Controller.Eye;
using UniEyeController.Core.Controller.Eyelid;
using UniEyeController.Core.Extensions;
using UniEyeController.Core.Main.Constants;
using UniEyeController.Core.Process.Blink;
using UniEyeController.Core.Process.LookAt;
using UniEyeController.Core.Process.MicroMove;
using UnityEngine;

namespace UniEyeController.Core.Main
{
    [ExecuteAlways]
    [DisallowMultipleComponent]
    public class UniEyeController : MonoBehaviour
    {
        public UpdateMethod updateMethod = UpdateMethod.LateUpdate;
        
        public bool executeAlways;
        
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
        
        public LookAtProcess lookAtProcess = new LookAtProcess();
        public MicroMoveProcess microMoveProcess = new MicroMoveProcess();
        public BlinkProcess blinkProcess = new BlinkProcess();

        private void OnEnable()
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
            
            lookAtProcess.SetControllers(eyeController, eyelidController);
            microMoveProcess.SetControllers(eyeController, eyelidController);
            blinkProcess.SetControllers(eyeController, eyelidController);
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
            lookAtProcess.Progress();
            microMoveProcess.Progress();
            blinkProcess.Progress();
        }

        private void AutoUpdate()
        {
            if (Application.isPlaying || executeAlways)
            {
                UpdateInternal();
            }
        }

        private void Update()
        {
            if (updateMethod == UpdateMethod.Update)
            {
                AutoUpdate();
            }
        }

        private void LateUpdate()
        {
            if (updateMethod == UpdateMethod.LateUpdate)
            {
                AutoUpdate();
            }
        }

        private void FixedUpdate()
        {
            if (updateMethod == UpdateMethod.FixedUpdate)
            {
                AutoUpdate();
            }
        }

        /// <summary>
        /// 手動でのアップデート
        /// Application.isPlayingやexecuteAlwaysの設定に関わらず、UpdateInternalを呼び出す
        /// </summary>
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
        }
    }
}