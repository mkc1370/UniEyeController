using UniEyeController.Core.Process.Blink;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace UniEyeController.Timeline.Blink
{
    public class UniEyeBlinkMixer : PlayableBehaviour
    {
        public UniEyeBlinkTrack Track { get; set; }
        public TimelineClip[] Clips { get; set; }

        public PlayableDirector Director;
        
        private BlinkProcess _target;

        private BlinkProcessStatus _processStatus = new BlinkProcessStatus();

        public override void OnPlayableDestroy(Playable playable)
        {
            if (_target == null) return;
            _target.Progress(Time.time, _processStatus);
        }

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            _target = playerData as BlinkProcess;
            Debug.Log(_target);
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

            _processStatus.weight = anyWeight ? 1 : 0;
            _target.Progress(playable.GetTime(), _processStatus);
        }
    }
}