using System;
using UniEyeController.Core.Controller.Eye;
using UniEyeController.Core.Controller.Eyelid;
using UniEyeController.Core.EyeProcess.EyeStatus;
using UnityEngine;

namespace UniEyeController.Core.EyeProcess
{
    [Serializable]
    public abstract class EyeProcessBase<TEyeStatus> where TEyeStatus : IEyeStatus
    {
        public TEyeStatus statusMonoBehaviour;

        public bool enabled;
        
        public bool CanExecute => enabled && (Application.isPlaying || executeAlways);
        
        public bool executeAlways;

        public DoubleEyeController EyeController;
        public EyelidController EyelidController;

        public void Progress(double time, TEyeStatus status)
        {
            ProgressInternal(time, status, true);
        }

        public void Progress(double time)
        {
            ProgressInternal(time, statusMonoBehaviour, false);
        }

        protected abstract void ProgressInternal(double time, TEyeStatus status, bool controlFromTileLine);
    }
}
