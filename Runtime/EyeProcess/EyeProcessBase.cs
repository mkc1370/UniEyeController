using UniEyeController.Core.Constants;
using UniEyeController.Core.Rotator;
using UnityEngine;

namespace UniEyeController.EyeProcess
{
    /// <summary>
    /// 目のモーションを制御する抽象クラス
    /// </summary>
    public abstract class EyeProcessBase : MonoBehaviour
    {
        public UpdateMethod updateMethod = UpdateMethod.LateUpdate;
        
        public bool CanExecute => Application.isPlaying || executeAlways;
        
        public bool executeAlways;

        /// <summary>
        /// IEyeProcessの実行順
        /// 値が小さいほど先に実行される
        /// </summary>
        public int executionOrder;

        public DoubleEyeRotator Rotator;

        public abstract void Progress(double time, bool controlFromTimeline);
    }
}