using UniEyeController.Core;
using UniEyeController.Core.Rotator;

namespace UniEyeController.EyeProcess
{
    public abstract class UniEyeProcessBase
    {
        public bool enabled = true;

        public DoubleEyeController EyeController;
        public EyelidController EyelidController;

        public abstract void Progress(double time, bool controlFromTimeline);
    }
}