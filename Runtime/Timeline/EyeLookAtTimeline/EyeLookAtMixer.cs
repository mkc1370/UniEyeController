using SimpleEyeController.View.Process;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace SimpleEyeController.Timeline.EyeLookAtTimeline
{
    public class EyeLookAtMixer : PlayableBehaviour
    {
        public EyeLookAtTrack Track { get; set; }
        public TimelineClip[] Clips { get; set; }
        
        private EyeLookAt _target;

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            _target = playerData as EyeLookAt;
            if (_target == null) return;

            for (var i = 0; i < Clips.Length; i++)
            {
                var clip = Clips[i];
                var asset = clip.asset as EyeLookAtClip;
                if (asset == null) continue;
                
                var weight = playable.GetInputWeight(i);
                if (weight > 0)
                {
                    _target.status = asset.status;
                }
            }
        }
    }
}