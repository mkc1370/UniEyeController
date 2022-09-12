using System;
using UniEyeController.Core.Controller.Eye;
using UniEyeController.Core.Controller.Eyelid;
using UnityEngine;

namespace UniEyeController.Core.Process.Core
{
    [Serializable]
    public abstract class EyeProcessBase<TEyeSetting, TEyeStatus> : EyeProcessBase where TEyeSetting : EyeProcessSettingBase where TEyeStatus : EyeProcessStatusBase
    {
        public TEyeSetting setting;
        
        /// <summary>
        /// インスペクターから操作する用
        /// </summary>
        public TEyeStatus serializedStatus;
        
        public bool executeAlways;

        protected DoubleEyeController EyeController;
        protected EyelidController EyelidController;
        
        public void SetControllers(DoubleEyeController eyeController, EyelidController eyelidController)
        {
            EyeController = eyeController;
            EyelidController = eyelidController;
        }

        private bool CanExecute => enabled && (Application.isPlaying || executeAlways);

        public void Progress(double time, TEyeStatus status = null)
        {
            if (!CanExecute) return;
            
            if (EyeController == null)
            {
                Debug.LogError($"{nameof(EyeController)} is null");
                return;
            }

            if (EyelidController == null)
            {
                Debug.LogError($"{nameof(EyelidController)} is null");
                return;
            }

            // 引数からStatusが指定されていない場合は、MonoBehaviour側のStatusを使用する
            ProgressInternal(time, status ?? serializedStatus);
        }

        protected abstract void ProgressInternal(double time, TEyeStatus status);
    }

    [Serializable]
    public abstract class EyeProcessBase
    {
        public bool enabled;
    }
}
