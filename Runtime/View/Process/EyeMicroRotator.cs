using SimpleEyeController.Model.Rotator;
using SimpleEyeController.Model.Setting;
using SimpleEyeController.View.Process.Interface;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SimpleEyeController.View.Process
{
    /// <summary>
    /// 眼球微細運動を再現する
    /// </summary>
    public class EyeMicroRotator : MonoBehaviour, IEyeProcess
    {
        public EyeMicroRotatorSetting setting;
        
        private float _eyeMoveTimer;

        public DoubleEyeRotator Rotator { private get; set; }

        private Vector2 _currentNormalizedEulerAngles;

        // To show enabled state in inspector.
        private void Start()
        {
        }

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
            
            Rotator.AppendNormalizedRotate(_currentNormalizedEulerAngles * setting.weight);
        }
    }
}