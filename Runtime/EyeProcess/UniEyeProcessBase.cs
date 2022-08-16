using UniEyeController.Core;
using UniEyeController.Core.Constants;
using UniEyeController.Core.Rotator;
using UniEyeController.Core.Status;
using UnityEngine;

namespace UniEyeController.EyeProcess
{
    /// <summary>
    /// 目のモーションを制御する抽象クラス
    /// </summary>
    public abstract class UniEyeProcessBase : MonoBehaviour
    {
        public UpdateMethod updateMethod = UpdateMethod.LateUpdate;
        
        public bool CanExecute => Application.isPlaying || executeAlways;
        
        public bool executeAlways;

        /// <summary>
        /// IEyeProcessの実行順
        /// 値が小さいほど先に実行される
        /// </summary>
        public int executionOrder;

        public DoubleEyeController EyeController;
        public EyelidController EyelidController;

        public abstract void Progress(double time, IEyeStatus status);

        // To show enabled state in inspector.
        private void Start()
        {
        }
    }
}