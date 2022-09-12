using UniEyeController.Core.EyeProcess.EyeStatus;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace UniEyeController.Timeline.UniEyeLookAtTimeline
{
    public class UniEyeLookAtClip : PlayableAsset, ITimelineClipAsset
    {
        public EyeLookAtStatus status = EyeLookAtStatus.Default;

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