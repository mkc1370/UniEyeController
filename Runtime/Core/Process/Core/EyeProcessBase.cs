using System;
using UniEyeController.Core.Controller.Eye;
using UniEyeController.Core.Controller.Eyelid;
using UniEyeController.Constants;
using UnityEngine;

namespace UniEyeController.Core.Process.Core
{
    [Serializable]
    public abstract class EyeProcessBase<TEyeSetting, TEyeStatus> : EyeProcessBase where TEyeSetting : EyeSettingBase where TEyeStatus : IEyeStatusBase
    {
        public TEyeSetting setting;
        public TEyeStatus status;
    }

    [Serializable]
    public abstract class EyeProcessBase
    {
        public bool executeAlways;
        
        public UpdateMethod updateMethod = UpdateMethod.LateUpdate;
        
        public bool enabled = true;

        protected DoubleEyeController EyeController;
        protected EyelidController EyelidController;
        
        public void SetControllers(DoubleEyeController eyeController, EyelidController eyelidController)
        {
            EyeController = eyeController;
            EyelidController = eyelidController;
        }
        
        public void Progress(UpdateMethod currentUpdateMethod)
        {
            if (!enabled) return;

            if (currentUpdateMethod != UpdateMethod.Timeline)
            {
                if (updateMethod != currentUpdateMethod) return;
                if (!Application.isPlaying && !executeAlways) return;
            }
            
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
            ProgressInternal(Time.time);
        }

        protected abstract void ProgressInternal(double time);
    }
}
