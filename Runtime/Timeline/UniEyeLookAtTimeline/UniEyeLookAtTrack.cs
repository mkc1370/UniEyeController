using System.Linq;
using UniEyeController.Core.EyeProcess;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace UniEyeController.Timeline.UniEyeLookAtTimeline
{
    [TrackClipType(typeof(UniEyeLookAtClip))]
    [TrackBindingType(typeof(EyeLookAt))]
    public class UniEyeLookAtTrack : TrackAsset
    {
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            var playable = ScriptPlayable<UniEyeLookAtMixer>.Create(graph, inputCount);
            var mixer = playable.GetBehaviour();
            mixer.Clips = GetClips().ToArray();

            return playable;
        }
    }
}