using System;
using UniEyeController.Core.Process.LookAt;
using UniEyeController.Timeline.LookAt;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.Timeline;

namespace UniEyeController.Editor.Timeline.LookAt
{
    [CustomTimelineEditor(typeof(UniEyeLookAtClip))]
    public class UniEyeLookAtClipEditor : ClipEditor
    {
        public override ClipDrawOptions GetClipOptions(TimelineClip clip)
        {
            var asset = clip.asset as UniEyeLookAtClip;
            var options = base.GetClipOptions(clip);
            Color color;
            switch (asset.processStatus.method)
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

            clip.displayName = asset.processStatus.ToString();
            options.errorText = asset.processStatus.ErrorMessage;
            options.highlightColor = color;
            return options;
        }
    }
}