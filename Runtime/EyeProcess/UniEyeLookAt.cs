using System;
using UniEyeController.Core.Constants;
using UniEyeController.Core.Status;
using UnityEngine;

namespace UniEyeController.EyeProcess
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(UniEyeController))]
    public class UniEyeLookAt : EyeProcessBase
    {
        private void Reset()
        {
            executionOrder = 1;
        }

        public EyeLookAtStatus status = EyeLookAtStatus.Default;

        // To show enabled state in inspector.
        private void Start()
        {
        }

        public void ResetEyeRotation()
        {
            if (!enabled) return;
            if (Rotator == null) return;
            
            Rotator.Rotate(Vector2.zero, 1, RotationApplyMethod.Direct);
        }

        public override void Progress(double time, bool controlFromTimeline)
        {
            if (!enabled) return;
            if (Rotator == null) return;
            
            var rotationApplyMethod = controlFromTimeline ? RotationApplyMethod.Append : RotationApplyMethod.Direct;
            
            switch (status.method)
            {
                case LookAtMethod.Transform:
                    if (status.targetTransform == null)
                    {
                        Debug.LogError($"Target Transform is not set.");
                        Rotator.Rotate(Vector2.zero, 1, RotationApplyMethod.Direct);
                        return;
                    }

                    Rotator.LookAt(status.targetTransform.position, status.weight, rotationApplyMethod);
                    break;
                case LookAtMethod.MainCamera:
                    var mainCamera = Camera.main;
                    if (mainCamera == null)
                    {
                        Debug.LogError($"MainCamera is not found.");
                        Rotator.Rotate(Vector2.zero, 1, RotationApplyMethod.Direct);
                        return;
                    }

                    Rotator.LookAt(mainCamera.transform.position, status.weight, rotationApplyMethod);
                    break;
                case LookAtMethod.WorldPosition:
                    Rotator.LookAt(status.worldPosition, status.weight, rotationApplyMethod);
                    break;
                case LookAtMethod.Rotation:
                    Rotator.NormalizedRotate(new Vector2(status.normalizedYaw, status.normalizedPitch), status.weight, rotationApplyMethod);
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

                    Rotator.NormalizedRotate(direction, status.weight, rotationApplyMethod);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}