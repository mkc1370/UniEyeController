#if USE_VRM1
using UniEyeController.Core.Controller.Eye;
using UniGLTF.Extensions.VRMC_vrm;
using UniVRM10;

namespace UniEyeController
{
    public partial class UniEyeController
    {
        public Vrm10Instance vrm10Instance;

        partial void GetEyeDefaultStatusBonesVRM()
        {
            _defaultStatus = DoubleEyeDefaultStatus.CreateFromVrm(vrm10Instance);
            vrm10Instance.Vrm.LookAt.LookAtType = LookAtType.expression;
        }
    }
}
#endif
