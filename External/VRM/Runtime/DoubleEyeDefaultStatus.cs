#if USE_VRM1
using UniEyeController.Core.Controller.Eye.Constants;
using UniVRM10;

namespace UniEyeController.Core.Controller.Eye
{
    public partial class DoubleEyeDefaultStatus
    {
        public static DoubleEyeDefaultStatus CreateFromVrm(Vrm10Instance vrm10Instance)
        {
            var eyeL = new SingleEyeDefaultStatus(
                vrm10Instance.Humanoid.LeftEye,
                vrm10Instance.Humanoid.LeftEye,
                EyeType.Left
            );
            var eyeR = new SingleEyeDefaultStatus(
                vrm10Instance.Humanoid.RightEye,
                vrm10Instance.Humanoid.RightEye,
                EyeType.Right
            );
            return new DoubleEyeDefaultStatus()
            {
                EyeL = eyeL,
                EyeR = eyeR
            };
        }
    }
}
#endif
