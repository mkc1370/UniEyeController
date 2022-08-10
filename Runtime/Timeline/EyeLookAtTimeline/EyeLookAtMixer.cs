using UniEyeController.EyeProcess;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace UniEyeController.Timeline.EyeLookAtTimeline
{
    public class EyeLookAtMixer : PlayableBehaviour
    {
        public EyeLookAtTrack Track { get; set; }
        public TimelineClip[] Clips { get; set; }
        
        private UniUniEyeLookAt _target;

        public override void OnPlayableDestroy(Playable playable)
        {
            if (_target == null) return;
                
            _target.ResetEyeRotation();
        }

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            _target = playerData as UniUniEyeLookAt;
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

            for (var i = 0; i < Clips.Length; i++)
            {
                var clip = Clips[i];
                var asset = clip.asset as EyeLookAtClip;
                if (asset == null) continue;
                
                var weight = playable.GetInputWeight(i);
                if (weight > 0)
                {
                    asset.status.targetTransform =
                        asset.status.targetTransformTimeline.Resolve(playable.GetGraph().GetResolver());
                    _target.status = asset.status;
                    _target.status.weight *= weight;
                    _target.Progress(playable.GetTime(), true);
                }
            }
        }
    }
}