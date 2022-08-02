using SimpleEyeController.Interface;
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