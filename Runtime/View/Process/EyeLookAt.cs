using System;
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
        public EyeLookAtStatus status;

        public int ExecutionOrder { get; set; } = 1;
        
        public DoubleEyeRotator Rotator { get; set; }

        // To show enabled state in inspector.
        private void Start()
        {
        }

        public void Progress(double time)
        {
            if (!enabled) return;

            switch (status.method)
            {
                case LookAtMethod.Transform:
                    if (status.target == null)
                    {
                        Debug.LogError($"Target Transform is not set.");
                        Rotator.Rotate(Vector2.zero);
                        return;
                    }
                    Rotator.LookAt(status.target.position, status.weight);
                    break;
                case LookAtMethod.MainCamera:
                    var mainCamera = Camera.main;
                    if (mainCamera == null)
                    {
                        Debug.LogError($"MainCamera is not found.");
                        Rotator.Rotate(Vector2.zero);
                        return;
                    }
                    Rotator.LookAt(mainCamera.transform.position, status.weight);
                    break;
                case LookAtMethod.WorldPosition:
                    Rotator.LookAt(status.worldPosition, status.weight);
                    break;
                case LookAtMethod.Rotation:
                    Rotator.NormalizedRotate(new Vector2(status.normalizedYaw, status.normalizedPitch) * status.weight);
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
                    Rotator.NormalizedRotate(direction);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}