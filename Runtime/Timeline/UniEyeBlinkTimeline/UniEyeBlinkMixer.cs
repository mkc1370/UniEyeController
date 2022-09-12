using UniEyeController.Core.Status;
using UniEyeController.EyeProcess;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace UniEyeController.Timeline.UniEyeBlinkTimeline
{
    public class UniEyeBlinkMixer : PlayableBehaviour
    {
        public UniEyeBlinkTrack Track { get; set; }
        public TimelineClip[] Clips { get; set; }

        public PlayableDirector Director;
        
        private UniEyeBlink _target;

        private UniEyeBlinkStatus _status;

        public override void OnPlayableDestroy(Playable playable)
        {
            if (_target == null) return;
            _status.ForceBlinkOff = true;
            _target.Progress(Time.time, _status);
        }

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            _target = playerData as UniEyeBlink;
            Debug.Log(_target);
            if (_target == null) return;

            var anyWeight = false;
            
            for (var i = 0; i < Clips.Length; i++)
            {
                var clip = Clips[i];
                var asset = clip.asset as UniEyeBlinkClip;
                if (asset == null) continue;
                
                var weight = playable.GetInputWeight(i);
                var clipProgress = (float)((Director.time - clip.start) / clip.duration);
                if (clipProgress >= 0 && clipProgress <= 1)
                {
                    anyWeight = true;
                }
            }

            _status.ForceBlinkOff = anyWeight;

            _target.Progress(playable.GetTime(), _status);
        }
    }
}