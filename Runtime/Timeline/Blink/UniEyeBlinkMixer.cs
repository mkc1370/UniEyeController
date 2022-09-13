using UniEyeController.Core.Process.Blink;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace UniEyeController.Timeline.Blink
{
    public class UniEyeBlinkMixer : PlayableBehaviour
    {
        public UniEyeBlinkTrack Track { get; set; }
        public TimelineClip[] Clips { get; set; }

        public PlayableDirector Director;
        
        private Core.Main.UniEyeController _target;

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            _target = playerData as Core.Main.UniEyeController;
            if (_target == null) return;

            var anyWeight = false;
            
            for (var i = 0; i < Clips.Length; i++)
            {
                var clip = Clips[i];
                var asset = clip.asset as UniEyeBlinkClip;
                if (asset == null) continue;
                
                var clipProgress = (float)((Director.time - clip.start) / clip.duration);
                if (clipProgress >= 0 && clipProgress <= 1)
                {
                    anyWeight = true;
                }
            }

            var status = new BlinkStatus();
            status.blinkOffFromTimeline = anyWeight;
            _target.blinkProcess.status = status;
            _target.blinkProcess.Progress(playable.GetTime());
        }
    }
}