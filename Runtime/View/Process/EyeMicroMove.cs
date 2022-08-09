using SimpleEyeController.Constants;
using SimpleEyeController.Model.Rotator;
using SimpleEyeController.View.Process.Interface;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SimpleEyeController.View.Process
{
    /// <summary>
    /// 眼球微細運動を再現する
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(EyeController))]
    public class EyeMicroMove : MonoBehaviour, IEyeProcess
    {
        [Range(0f, 1f)]
        public float weight = 1f;
        
        [Range(0f, 1f)]
        public float eyeMicroMoveMultiplier = 0.1f;
        public float eyeMoveStopTimeMin = 0.5f;
        public float eyeMoveStopTimeMax = 2.3f;

        public int ExecutionOrder { get; set; } = 2;
        public DoubleEyeRotator Rotator { get; set; }
        
        private float _eyeMoveTimer;


        private Vector2 _currentNormalizedEulerAngles;

        // To show enabled state in inspector.
        private void Start()
        {
        }

        public void Progress(double time, bool controlFromTimeline)
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
            
            Rotator.NormalizedRotate(_currentNormalizedEulerAngles, weight, RotationApplyMethod.Append);
        }
    }
}