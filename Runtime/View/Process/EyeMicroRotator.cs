using SimpleEyeController.Model.Rotator;
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
        [Header("眼球微細運動の適用度")]
        [Range(0f, 1f)]
        public float weight = 1f;
        
        [Header("目の可動域の何倍の範囲で眼球微細運動をするか")]
        [Range(0f, 1f)]
        public float eyeMicroMoveMultiplier = 0.1f;
        
        [Header("眼球微細運動を止める時間の最小値")]
        public float eyeMoveStopTimeMin = 0.5f;
        
        [Header("眼球微細運動を止める時間の最大値")]
        public float eyeMoveStopTimeMax = 2.3f;
        
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
                _currentNormalizedEulerAngles = new Vector2(x, y) * eyeMicroMoveMultiplier;
                
                _eyeMoveTimer = Random.Range(eyeMoveStopTimeMin, eyeMoveStopTimeMax);
            }
            
            Rotator.AppendNormalizedRotate(_currentNormalizedEulerAngles * weight);
        }
    }
}