using UniEyeController.Core.Process.LookAt;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace UniEyeController.Timeline.LookAt
{
    public class UniEyeLookAtClip : PlayableAsset, ITimelineClipAsset
    {
        public LookAtStatus status = LookAtStatus.Default;

        public ClipCaps clipCaps =>
            ClipCaps.ClipIn |
            ClipCaps.SpeedMultiplier |
            ClipCaps.Blending;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
        {
            var playable = ScriptPlayable<UniEyeLookAtBehaviour>.Create(graph);
            return playable;
        }
    }
}