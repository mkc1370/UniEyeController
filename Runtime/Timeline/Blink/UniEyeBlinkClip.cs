using UniEyeController.Core.Process.Blink;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace UniEyeController.Timeline.Blink
{
    public class UniEyeBlinkClip : PlayableAsset, ITimelineClipAsset
    {
        public BlinkProcessStatus processStatus;

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