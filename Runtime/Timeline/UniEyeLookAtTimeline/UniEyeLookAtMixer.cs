using UniEyeController.Core.EyeProcess;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace UniEyeController.Timeline.UniEyeLookAtTimeline
{
    public class UniEyeLookAtMixer : PlayableBehaviour
    {
        public UniEyeLookAtTrack Track { get; set; }
        public TimelineClip[] Clips { get; set; }
        
        private EyeLookAt _target;

        private bool _wasPrevFrameControlled;

        public override void OnPlayableDestroy(Playable playable)
        {
            if (_target == null) return;
                
            _target.ResetEyeRotation();
        }

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            _target = playerData as EyeLookAt;
            if (_target == null) return;
            
            // 1個でもWeightが0より大きいものがあれば一度目の回転をリセットする
            // タイムラインは回転を加算していく形になるため
            var inputCount = playable.GetInputCount();
            for (var i = 0; i < inputCount; i++)
            {
                if (playable.GetInputWeight(i) > 0)
                {
                    _target.ResetEyeRotation();
                    break;
                }
            }

            if (_wasPrevFrameControlled)
            {
                _target.ResetEyeRotation();
            }

            _wasPrevFrameControlled = false;

            for (var i = 0; i < Clips.Length; i++)
            {
                var clip = Clips[i];
                var asset = clip.asset as UniEyeLookAtClip;
                if (asset == null) continue;
                
                var weight = playable.GetInputWeight(i);
                if (weight > 0)
                {
                    asset.status.targetTransform =
                        asset.status.targetTransformTimeline.Resolve(playable.GetGraph().GetResolver());
                    _target.statusMonoBehaviour.weight *= weight;
                    _target.Progress(playable.GetTime(), asset.status);
                    _wasPrevFrameControlled = true;
                }
            }
        }
    }
}