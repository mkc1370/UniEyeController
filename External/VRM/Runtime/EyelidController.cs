#if USE_VRM1
using UniVRM10;

namespace UniEyeController.Core.Controller.Eyelid
{
    public partial class EyelidController
    {
        partial void BlinkVRM(float value)
        {
            _setting.vrm10Instance.Runtime.Expression.SetWeight(ExpressionKey.Blink, value);
        }
    }
}
#endif