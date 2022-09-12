using System.Linq;
using UniEyeController.Core.Process.Blink;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace UniEyeController.Timeline.Blink
{
    [TrackClipType(typeof(UniEyeBlinkClip))]
    [TrackBindingType(typeof(BlinkProcess))]
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