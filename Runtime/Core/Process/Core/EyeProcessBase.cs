using System;
using UniEyeController.Core.Controller.Eye;
using UniEyeController.Core.Controller.Eyelid;
using UnityEngine;

namespace UniEyeController.Core.Process.Core
{
    [Serializable]
    public abstract class EyeProcessBase<TEyeStatus> : EyeProcessBase where TEyeStatus : EyeStatusBase
    {
        /// <summary>
        /// インスペクターから操作する用
        /// </summary>
        public TEyeStatus statusMonoBehaviour;
        
        public bool executeAlways;
        
        protected DoubleEyeController EyeController => _eyeController;
        protected EyelidController EyelidController => _eyelidController;
        
        private DoubleEyeController _eyeController;
        private EyelidController _eyelidController;

        protected EyeProcessBase(DoubleEyeController eyeController, EyelidController eyelidController)
        {
            _eyeController = eyeController;
            _eyelidController = eyelidController;
        }

        private bool CanExecute => enabled && (Application.isPlaying || executeAlways);

        public void Progress(double time, TEyeStatus status = null)
        {
            if (!CanExecute) return;
            
            if (_eyeController == null)
            {
                Debug.LogError($"{nameof(_eyeController)} is null");
                return;
            }

            if (_eyelidController == null)
            {
                Debug.LogError($"{nameof(_eyelidController)} is null");
                return;
            }

            // 引数からStatusが指定されていない場合は、MonoBehaviour側のStatusを使用する
            ProgressInternal(time, status ?? statusMonoBehaviour);
        }

        protected abstract void ProgressInternal(double time, TEyeStatus status);
    }

    [Serializable]
    public abstract class EyeProcessBase
    {
        public bool enabled;
    }
}
