using System;
using SimpleEyeController.Interface;
using SimpleEyeController.Model.Rotator;
using SimpleEyeController.Model.Setting;
using UnityEngine;

namespace SimpleEyeController.Model.Process
{
    [Serializable]
    public class EyeLookAt : IEyeProcess
    {
        public EyeLookAtSetting setting;
        
        public DoubleEyeRotator Rotator { private get; set; }
        
        public void Progress()
        {
            if (!setting.enabled) return;
            
            if (setting.useTarget)
            {
                Rotator.LookAt(setting.target.position);
            }
            else
            {
                Rotator.NormalizedRotate(new Vector2(setting.normalizedYaw, setting.normalizedPitch));
            }
        }
    }
}