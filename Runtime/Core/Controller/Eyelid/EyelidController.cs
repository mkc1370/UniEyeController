using System;
using UniEyeController.Core.Controller.Eyelid.Constants;

namespace UniEyeController.Core.Controller.Eyelid
{
    public partial class EyelidController
    {
        private EyelidSetting _setting;

        partial void BlinkVRM(float value);

        public EyelidController(EyelidSetting setting)
        {
            _setting = setting;
        }

        public void Blink(float value, Action<float> onBlink)
        {
            switch (_setting.eyelidType)
            {
                case EyelidType.Manual:
                    onBlink?.Invoke(value);
                    break;
                case EyelidType.BlendShapeIndex:
                    foreach (var blendShapeIndex in _setting.blendShapeIndexes)
                    {
                        _setting.blendShapeMesh.SetBlendShapeWeight(blendShapeIndex, value * 100);
                    }
                    break;
                case EyelidType.BlendShapeName:
                    foreach (var blendShapeName in _setting.blendShapeNames)
                    {
                        var index = _setting.blendShapeMesh.sharedMesh.GetBlendShapeIndex(blendShapeName);
                        _setting.blendShapeMesh.SetBlendShapeWeight(index, value * 100);
                    }
                    break;
                case EyelidType.Vrm1:
                    BlinkVRM(value);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}