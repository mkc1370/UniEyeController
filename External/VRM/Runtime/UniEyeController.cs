#if USE_VRM1
using UniEyeController.Core.Controller.Eye;
using UniVRM10;

namespace UniEyeController
{
    public partial class UniEyeController
    {
        public Vrm10Instance _vrm10Instance;

        partial void GetEyeDefaultStatusBonesVRM()
        {
            _defaultStatus = DoubleEyeDefaultStatus.CreateFromVrm(_vrm10Instance);
        }
    }
}
#endif
