using UniEyeController.Core;
using UniEyeController.Core.Rotator;
using UniEyeController.Core.Status;
using UnityEngine;

namespace UniEyeController.EyeProcess
{
    public abstract class UniEyeProcessBase
    {
        public UpdateMethod updateMethod = UpdateMethod.LateUpdate;

        public bool CanExecute => enabled && (Application.isPlaying || executeAlways);
        
        public bool executeAlways;

        /// <summary>
        /// IEyeProcessの実行順
        /// 値が小さいほど先に実行される
        /// </summary>
        public int executionOrder;

        public DoubleEyeController EyeController;
        public EyelidController EyelidController;

        public abstract void Progress(double time, IEyeStatus status);

    }
}
