using System;
using SimpleEyeController.Constants;
using SimpleEyeController.Model.Rotator;
using SimpleEyeController.View.Process.Interface;
using UnityEngine;

namespace SimpleEyeController.View.Process
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(EyeController))]
    public class EyeLookAt : MonoBehaviour, IEyeProcess
    {
        [Header("視線制御の適用度")]
        [Range(0f, 1f)]
        public float weight = 1f;
        
        [Header("指定方法")]
        public LookAtMethod method = LookAtMethod.Rotation;
    
        [Header("見る対象（Transform）")]
        public Transform target;
        
        [Header("見る対象（ワールド座標）")]
        public Vector3 worldPosition;

        [Header("目の角度（左右） [-1, 1]")]
        [Range(-1f, 1f)]
        public float normalizedYaw;
        
        [Header("目の角度（上下） [-1, 1]")]
        [Range(-1f, 1f)]
        public float normalizedPitch;

        public int ExecutionOrder { get; set; } = 1;
        
        public DoubleEyeRotator Rotator { get; set; }

        // To show enabled state in inspector.
        private void Start()
        {
        }

        public void Progress()
        {
            if (!enabled) return;

            switch (method)
            {
                case LookAtMethod.Transform:
                    if (target == null)
                    {
                        Debug.LogError($"Target Transform is not set.");
                        Rotator.Rotate(Vector2.zero);
                        return;
                    }
                    Rotator.LookAt(target.position, weight);
                    break;
                case LookAtMethod.MainCamera:
                    var mainCamera = Camera.main;
                    if (mainCamera == null)
                    {
                        Debug.LogError($"MainCamera is not found.");
                        Rotator.Rotate(Vector2.zero);
                        return;
                    }
                    Rotator.LookAt(mainCamera.transform.position, weight);
                    break;
                case LookAtMethod.WorldPosition:
                    Rotator.LookAt(worldPosition, weight);
                    break;
                case LookAtMethod.Rotation:
                    Rotator.NormalizedRotate(new Vector2(normalizedYaw, normalizedPitch) * weight);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}