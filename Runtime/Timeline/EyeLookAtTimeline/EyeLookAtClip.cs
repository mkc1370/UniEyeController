using SimpleEyeController.Model.Status;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace SimpleEyeController.Timeline.EyeLookAtTimeline
{
    public class EyeLookAtClip : PlayableAsset, ITimelineClipAsset
    {
        public EyeLookAtStatus status = EyeLookAtStatus.Default;

        public ClipCaps clipCaps =>
            ClipCaps.ClipIn |
            ClipCaps.SpeedMultiplier |
            ClipCaps.Blending;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
        {
            var playable = ScriptPlayable<EyeLookAtBehaviour>.Create(graph);
            return playable;
        }
    }
}