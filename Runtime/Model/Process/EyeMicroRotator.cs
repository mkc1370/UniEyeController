using System;
using SimpleEyeController.Interface;
using SimpleEyeController.Model.Rotator;
using SimpleEyeController.Model.Setting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SimpleEyeController.Model.Process
{
    [Serializable]
    public class EyeMicroRotator : IEyeProcess
    {
        public bool enabled = true;
        public EyeMicroRotatorSetting setting;
        
        private float _eyeMoveTimer;

        public DoubleEyeRotator Rotator { private get; set; }

        private Vector2 _currentNormalizedEulerAngles;

        public void Progress()
        {
            if (!enabled) return;
            
            _eyeMoveTimer -= Time.deltaTime;
            if (_eyeMoveTimer < 0)
            {
                var x = Random.Range(-1, 1);
                var y = Random.Range(-1, 1);
                _currentNormalizedEulerAngles = new Vector2(x, y) * setting.eyeMicroMoveMultiplier;
                
                _eyeMoveTimer = Random.Range(setting.eyeMoveStopTimeMin, setting.eyeMoveStopTimeMax);
            }
            
            Rotator.AppendNormalizedRotate(_currentNormalizedEulerAngles);
        }
    }
}