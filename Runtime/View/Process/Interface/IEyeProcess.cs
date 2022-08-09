using SimpleEyeController.Model.Rotator;

namespace SimpleEyeController.View.Process.Interface
{
    public interface IEyeProcess
    {
        /// <summary>
        /// IEyeProcessの実行順
        /// 値が小さいほど先に実行される
        /// </summary>
        int ExecutionOrder { get; set; }

        DoubleEyeRotator Rotator { get; set; }
        void Progress(double time, bool controlFromTimeline);
    }
}