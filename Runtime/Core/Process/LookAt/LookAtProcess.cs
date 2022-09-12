using System;
using UniEyeController.Core.Controller.Eye;
using UniEyeController.Core.Controller.Eye.Constants;
using UniEyeController.Core.Controller.Eyelid;
using UniEyeController.Core.Process.Core;
using UnityEngine;

namespace UniEyeController.Core.Process.LookAt
{
    [Serializable]
    public class LookAtProcess : EyeProcessBase<LookAtStatus>
    {
        public LookAtProcess(DoubleEyeController eyeController, EyelidController eyelidController) : base(eyeController, eyelidController)
        {
        }
        
        public void ResetEyeRotation()
        {
            if (EyeController == null) return;
            
            EyeController.Rotate(Vector2.zero, 1, RotationApplyMethod.Direct);
        }

        protected override void ProgressInternal(double time, LookAtStatus status)
        {
            var rotationApplyMethod = RotationApplyMethod.Direct;
            
            switch (status.method)
            {
                case LookAtMethod.Transform:
                    if (status.targetTransform == null)
                    {
                        Debug.LogError($"Target Transform is not set.");
                        EyeController.Rotate(Vector2.zero, status.weight, RotationApplyMethod.Direct);
                        return;
                    }

                    EyeController.LookAt(status.targetTransform.position, status.weight, rotationApplyMethod);
                    break;
                case LookAtMethod.MainCamera:
                    var mainCamera = Camera.main;
                    if (mainCamera == null)
                    {
                        Debug.LogError($"MainCamera is not found.");
                        EyeController.Rotate(Vector2.zero, status.weight, RotationApplyMethod.Direct);
                        return;
                    }

                    EyeController.LookAt(mainCamera.transform.position, status.weight, rotationApplyMethod);
                    break;
                case LookAtMethod.WorldPosition:
                    EyeController.LookAt(status.worldPosition, status.weight, rotationApplyMethod);
                    break;
                case LookAtMethod.Rotation:
                    EyeController.NormalizedRotate(new Vector2(status.normalizedYaw, status.normalizedPitch), status.weight, rotationApplyMethod);
                    break;
                case LookAtMethod.Direction:
                    Vector2 direction;
                    switch (status.direction)
                    {
                        case LookAtDirection.Front:
                            direction = Vector2.zero;
                            break;
                        case LookAtDirection.Left:
                            direction = Vector2.left;
                            break;
                        case LookAtDirection.Right:
                            direction = Vector2.right;
                            break;
                        case LookAtDirection.Up:
                            direction = Vector2.up;
                            break;
                        case LookAtDirection.Down:
                            direction = Vector2.down;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    EyeController.NormalizedRotate(direction, status.weight, rotationApplyMethod);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}