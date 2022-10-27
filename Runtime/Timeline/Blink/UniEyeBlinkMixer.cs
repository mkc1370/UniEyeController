using UniEyeController.Constants;
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

        private BlinkProcess _process;

        private BlinkStatus _status;

        public override void OnGraphStop(Playable playable)
        {
            if (_process == null) return;
            
            // タイムラインを停止した際に目を開いた状態に戻す
            _process.ForceReset();
        }

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            var controller = playerData as UniEyeController;
            if (controller == null) return;
            
            _process = controller.blinkProcess;
            if (_process == null) return;

            // 1個以上のクリップがある場合はまばたきを止める
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

            _status.blinkOffFromOutside = anyWeight;
            _process.status = _status;
            _process.Progress(UpdateMethod.Timeline);
        }
    }
}