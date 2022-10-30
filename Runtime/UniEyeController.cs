using UniEyeController.Constants;
using UniEyeController.Core.Controller.Eye;
using UniEyeController.Core.Controller.Eyelid;
using UniEyeController.Core.Extensions;
using UniEyeController.Core.Process.Blink;
using UniEyeController.Core.Process.LookAt;
using UniEyeController.Core.Process.MicroMove;
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
            GetEyeDefaultStatusBones();
            
            var eyeController = new DoubleEyeController(_defaultStatus, setting);
            var eyelidController = new EyelidController(eyelidSetting);
            
            lookAtProcess.SetControllers(eyeController, eyelidController);
            microMoveProcess.SetControllers(eyeController, eyelidController);
            blinkProcess.SetControllers(eyeController, eyelidController);
        }


        private void GetEyeDefaultStatusBones()
        {
            switch (assignMethod)
            {
                case EyeAssignMethod.Humanoid:
                    _defaultStatus = DoubleEyeDefaultStatus.CreateFromHumanoid(animator);
                    break;
                case EyeAssignMethod.Generic:
                    _defaultStatus = DoubleEyeDefaultStatus.CreateFromGeneric(
                        transform,
                        manualEyeL,
                        manualEyeR,
                        prefabForGenericAvatar
                    );
                    break;
            }
        }

        private void UpdateInternal(UpdateMethod updateMethod)
        {
            lookAtProcess.Progress(updateMethod);
            microMoveProcess.Progress(updateMethod);
            blinkProcess.Progress(updateMethod);
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

        /// <summary>
        /// 手動でのアップデート
        /// Application.isPlayingやexecuteAlwaysの設定に関わらず、UpdateInternalを呼び出す
        /// </summary>
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
        }
    }
}
