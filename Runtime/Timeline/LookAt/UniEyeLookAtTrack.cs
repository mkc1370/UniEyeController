using System.Linq;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace UniEyeController.Timeline.LookAt
{
    [TrackClipType(typeof(UniEyeLookAtClip))]
    [TrackBindingType(typeof(Core.Main.UniEyeController))]
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