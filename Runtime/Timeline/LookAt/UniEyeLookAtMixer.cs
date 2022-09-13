using UniEyeController.Core.Process.LookAt;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace UniEyeController.Timeline.LookAt
{
    public class UniEyeLookAtMixer : PlayableBehaviour
    {
        public UniEyeLookAtTrack Track { get; set; }
        public TimelineClip[] Clips { get; set; }
        
        private LookAtProcess _process;

        /// <summary>
        /// 目の状態を元に戻す
        /// GatherPropertiesの仕様上、メンテナンスが難しくなってしまうため
        /// 自前でキャッシュを用意しています
        /// </summary>
        private LookAtStatus? _statusCache;

        public override void OnPlayableDestroy(Playable playable)
        {
            if (_process == null) return;
                
            _process.ResetEyeRotation();
        }
        
        public override void OnGraphStop(Playable playable)
        {
            // 再生する前の状態を復元する
            if (_statusCache.HasValue)
            {
                _process.status = _statusCache.Value;
            }
            
            _statusCache = null;
        }

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            var controller = playerData as Core.Main.UniEyeController;
            if (controller == null) return;

            _process = controller.lookAtProcess;
            if (_process == null) return;
            
            // 再生する前の状態をキャッシュする
            if (_statusCache == null)
            {
                _statusCache = _process.status;
            }
            
            // 一度、正面を向いた状態にする
            _process.status = LookAtStatus.LookForward;

            for (var i = 0; i < Clips.Length; i++)
            {
                var clip = Clips[i];
                var asset = clip.asset as UniEyeLookAtClip;
                if (asset == null) continue;
                
                var weight = playable.GetInputWeight(i);
                if (weight > 0)
                {
                    var status = asset.status;
                    status.weight *= weight;
                    status.targetTransform =
                        asset.status.targetTransformTimeline.Resolve(playable.GetGraph().GetResolver());
                    
                    _process.status = status;
                    // TODO : ブレンドに対応させる
                    break;
                }
            }
            
            _process.Progress(playable.GetTime());
        }
    }
}