using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace UniEyeController.Timeline.LookAt
{
    public class UniEyeLookAtMixer : PlayableBehaviour
    {
        public UniEyeLookAtTrack Track { get; set; }
        public TimelineClip[] Clips { get; set; }
        
        private Core.Main.UniEyeController _target;

        private bool _wasPrevFrameControlled;

        public override void OnPlayableDestroy(Playable playable)
        {
            if (_target == null) return;
                
            _target.lookAtProcess.ResetEyeRotation();
        }

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            _target = playerData as Core.Main.UniEyeController;
            if (_target == null) return;
            
            // 1個でもWeightが0より大きいものがあれば一度目の回転をリセットする
            // タイムラインは回転を加算していく形になるため
            var inputCount = playable.GetInputCount();
            for (var i = 0; i < inputCount; i++)
            {
                if (playable.GetInputWeight(i) > 0)
                {
                    _target.lookAtProcess.ResetEyeRotation();
                    break;
                }
            }

            if (_wasPrevFrameControlled)
            {
                _target.lookAtProcess.ResetEyeRotation();
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
                    _target.lookAtProcess.status.weight = weight;
                    _target.lookAtProcess.Progress(playable.GetTime());
                    _wasPrevFrameControlled = true;
                }
            }
        }
    }
}