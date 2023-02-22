using System;
using System.Linq;
using UniEyeController.Core.Controller.Eyelid.Constants;
using UnityEngine;
#if USE_VRM1
using UniVRM10;
#endif

namespace UniEyeController.Core.Controller.Eyelid
{
    public class EyelidController
    {
        private EyelidSetting _setting;

        public EyelidController(EyelidSetting setting)
        {
            _setting = setting;
        }

        public void SetBlink(float value, Action<float> onBlink)
        {
            value = Mathf.Clamp01(value);
            
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
#if USE_VRM1
                    _setting.vrm10Instance.Runtime.Expression.SetWeight(ExpressionKey.Blink, value);
#endif
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public float GetBlink()
        {
            switch (_setting.eyelidType)
            {
                case EyelidType.Manual:
                    // TODO: 正しく実装する
                    return 1;
                case EyelidType.BlendShapeIndex:
                {
                    if(_setting.blendShapeIndexes.Length == 0) return 1;

                    var blendShapeIndex = _setting.blendShapeIndexes.First();
                    return _setting.blendShapeMesh.GetBlendShapeWeight(blendShapeIndex) / 100f;
                }
                case EyelidType.BlendShapeName:
                {
                    if(_setting.blendShapeNames.Length == 0) return 1;
                    
                    var blendShapeName = _setting.blendShapeNames.First();
                    var index = _setting.blendShapeMesh.sharedMesh.GetBlendShapeIndex(blendShapeName);
                    return _setting.blendShapeMesh.GetBlendShapeWeight(index) / 100f;
                }
                case EyelidType.Vrm1:
#if USE_VRM1
                    return _setting.vrm10Instance.Runtime.Expression.GetWeight(ExpressionKey.Blink);
#endif
                    return 1;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}