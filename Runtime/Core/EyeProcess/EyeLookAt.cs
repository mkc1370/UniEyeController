using System;
using UniEyeController.Core.Constants;
using UniEyeController.Core.EyeProcess.EyeStatus;
using UnityEngine;

namespace UniEyeController.Core.EyeProcess
{
    [Serializable]
    public class EyeLookAt : EyeProcessBase<EyeLookAtStatus>
    {
        public void ResetEyeRotation()
        {
            if (EyeController == null) return;
            
            EyeController.Rotate(Vector2.zero, 1, RotationApplyMethod.Direct);
        }

        protected override void ProgressInternal(double time, EyeLookAtStatus status, bool controlFromTileLine)
        {
            if (!CanExecute) return;
            if (EyeController == null) return;
            var rotationApplyMethod = RotationApplyMethod.Direct;
            
            switch (status.method)
            {
                case LookAtMethod.Transform:
                    if (status.targetTransform == null)
                    {
                        Debug.LogError($"Target Transform is not set.");
                        EyeController.Rotate(Vector2.zero, 1, RotationApplyMethod.Direct);
                        return;
                    }

                    EyeController.LookAt(status.targetTransform.position, status.weight, rotationApplyMethod);
                    break;
                case LookAtMethod.MainCamera:
                    var mainCamera = Camera.main;
                    if (mainCamera == null)
                    {
                        Debug.LogError($"MainCamera is not found.");
                        EyeController.Rotate(Vector2.zero, 1, RotationApplyMethod.Direct);
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
                        case EyeLookAtDirection.Front:
                            direction = Vector2.zero;
                            break;
                        case EyeLookAtDirection.Left:
                            direction = Vector2.left;
                            break;
                        case EyeLookAtDirection.Right:
                            direction = Vector2.right;
                            break;
                        case EyeLookAtDirection.Up:
                            direction = Vector2.up;
                            break;
                        case EyeLookAtDirection.Down:
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