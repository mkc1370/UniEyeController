using System;
using SimpleEyeController.Constants;
using SimpleEyeController.Model.Process.Interface;
using SimpleEyeController.Model.Rotator;
using SimpleEyeController.Model.Setting;
using UnityEngine;

namespace SimpleEyeController.Model.Process
{
    public class EyeLookAt : MonoBehaviour, IEyeProcess
    {
        public EyeLookAtSetting setting;
        
        public DoubleEyeRotator Rotator { private get; set; }

        // To show enabled state in inspector.
        private void Start()
        {
        }

        public void Progress()
        {
            if (!enabled) return;

            switch (setting.method)
            {
                case LookAtMethod.Transform:
                    if (setting.target == null)
                    {
                        Debug.LogError($"Target Transform is not set.");
                        Rotator.Rotate(Vector2.zero);
                        return;
                    }
                    Rotator.LookAt(setting.target.position, setting.weight);
                    break;
                case LookAtMethod.Rotation:
                    Rotator.NormalizedRotate(new Vector2(setting.normalizedYaw, setting.normalizedPitch) * setting.weight); break;
                case LookAtMethod.MainCamera:
                    var mainCamera = Camera.main;
                    if (mainCamera == null)
                    {
                        Debug.LogError($"MainCamera is not found.");
                        Rotator.Rotate(Vector2.zero);
                        return;
                    }
                    Rotator.LookAt(mainCamera.transform.position, setting.weight);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}