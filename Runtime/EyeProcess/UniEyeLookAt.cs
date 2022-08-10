using System;
using UniEyeController.Core.Constants;
using UniEyeController.Core.Status;
using UnityEngine;

namespace UniEyeController.EyeProcess
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(UniEyeController))]
    public class UniEyeLookAt : UniEyeProcessBase
    {
        private void Reset()
        {
            executionOrder = 1;
        }

        public EyeLookAtStatus status = EyeLookAtStatus.Default;

        public void ResetEyeRotation()
        {
            if (!enabled) return;
            if (EyeController == null) return;
            
            EyeController.Rotate(Vector2.zero, 1, RotationApplyMethod.Direct);
        }

        public override void Progress(double time, bool controlFromTimeline)
        {
            if (!enabled) return;
            if (EyeController == null) return;
            
            var rotationApplyMethod = controlFromTimeline ? RotationApplyMethod.Append : RotationApplyMethod.Direct;
            
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