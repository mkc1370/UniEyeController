using System;
using UniEyeController.Core.Constants;
using UniEyeController.Timeline.EyeLookAtTimeline;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.Timeline;

namespace UniEyeController.Editor.Timeline.EyeLookAtTimeline
{
    [CustomTimelineEditor(typeof(EyeLookAtClip))]
    public class EyeLookAtClipEditor : ClipEditor
    {
        public override ClipDrawOptions GetClipOptions(TimelineClip clip)
        {
            var asset = clip.asset as EyeLookAtClip;
            var options = base.GetClipOptions(clip);
            Color color;
            switch (asset.status.method)
            {
                case LookAtMethod.Transform:
                    color = Color.red;
                    break;
                case LookAtMethod.MainCamera:
                    color = Color.blue;
                    break;
                case LookAtMethod.WorldPosition:
                    color = Color.green;
                    break;
                case LookAtMethod.Rotation:
                    color = Color.yellow;
                    break;
                case LookAtMethod.Direction:
                    color = Color.white;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            clip.displayName = asset.status.ToString();
            options.errorText = asset.status.ErrorMessage;
            options.highlightColor = color;
            return options;
        }
    }
}