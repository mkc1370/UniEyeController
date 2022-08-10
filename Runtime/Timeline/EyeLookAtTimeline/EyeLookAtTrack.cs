using System.Linq;
using UniEyeController.EyeProcess;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace UniEyeController.Timeline.EyeLookAtTimeline
{
    [TrackClipType(typeof(EyeLookAtClip))]
    [TrackBindingType(typeof(UniUniEyeLookAt))]
    public class EyeLookAtTrack : TrackAsset
    {
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            var playable = ScriptPlayable<EyeLookAtMixer>.Create(graph, inputCount);
            var mixer = playable.GetBehaviour();
            mixer.Clips = GetClips().ToArray();

            return playable;
        }
    }
}