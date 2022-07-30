﻿using System;
using SimpleEyeController.Interface;
using SimpleEyeController.Model.Rotator;
using SimpleEyeController.Model.Setting;

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
                Rotator.Rotate(setting.eulerAngles);
            }
        }
    }
}