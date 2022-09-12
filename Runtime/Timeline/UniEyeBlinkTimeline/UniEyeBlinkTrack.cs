using System.Linq;
using UniEyeController.Core.EyeProcess;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace UniEyeController.Timeline.UniEyeBlinkTimeline
{
    [TrackClipType(typeof(UniEyeBlinkClip))]
    [TrackBindingType(typeof(EyeBlink))]
    public class UniEyeBlinkTrack : TrackAsset
    {
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            var playable = ScriptPlayable<UniEyeBlinkMixer>.Create(graph, inputCount);
            var mixer = playable.GetBehaviour();
            mixer.Clips = GetClips().ToArray();
            mixer.Director = go.GetComponent<PlayableDirector>();

            return playable;
        }
    }
}