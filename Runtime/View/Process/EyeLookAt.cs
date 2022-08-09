﻿using System;
using SimpleEyeController.Constants;
using SimpleEyeController.Model.Rotator;
using SimpleEyeController.Model.Status;
using SimpleEyeController.View.Process.Interface;
using UnityEngine;

namespace SimpleEyeController.View.Process
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(EyeController))]
    public class EyeLookAt : MonoBehaviour, IEyeProcess
    {
        public EyeLookAtStatus status = EyeLookAtStatus.Default;

        public int ExecutionOrder { get; set; } = 1;
        
        public DoubleEyeRotator Rotator { get; set; }

        // To show enabled state in inspector.
        private void Start()
        {
        }

        public void BeforeProgressTimeline()
        {
            Rotator.Rotate(Vector2.zero, 1, RotationApplyMethod.Direct);
        }

        public void Progress(double time, bool controlFromTimeline)
        {
            if (!enabled) return;
            
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