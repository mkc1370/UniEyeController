using UniEyeController.Core.Status;
using UniEyeController.Timeline.UniEyeBlinkTimeline;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace UniEyeController.Timeline.UniEyeBlinkTimeline
{
    public class UniEyeBlinkClip : PlayableAsset, ITimelineClipAsset
    {
        public UniEyeBlinkStatus status;

        public ClipCaps clipCaps =>
            ClipCaps.ClipIn |
            ClipCaps.SpeedMultiplier |
            ClipCaps.Blending;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
        {
            var playable = ScriptPlayable<UniEyeBlinkBehaviour>.Create(graph);
            return playable;
        }
    }
}