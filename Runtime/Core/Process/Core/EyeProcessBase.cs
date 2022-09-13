using System;
using UniEyeController.Core.Controller.Eye;
using UniEyeController.Core.Controller.Eyelid;
using UnityEngine;

namespace UniEyeController.Core.Process.Core
{
    [Serializable]
    public abstract class EyeProcessBase<TEyeSetting, TEyeStatus> where TEyeSetting : EyeSettingBase where TEyeStatus : IEyeStatusBase
    {
        public bool enabled = true;
        
        public TEyeSetting setting;
        
        public TEyeStatus status;

        protected DoubleEyeController EyeController;
        protected EyelidController EyelidController;
        
        public void SetControllers(DoubleEyeController eyeController, EyelidController eyelidController)
        {
            EyeController = eyeController;
            EyelidController = eyelidController;
        }

        public void Progress()
        {
            if (!enabled) return;
            
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
