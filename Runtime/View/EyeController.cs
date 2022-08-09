using System;
using System.Collections.Generic;
using System.Linq;
using SimpleEyeController.Constants;
using SimpleEyeController.Model.Extensions;
using SimpleEyeController.Model.Rotator;
using SimpleEyeController.Model.Setting;
using SimpleEyeController.Model.Status;
using SimpleEyeController.View.Process.Interface;
using UnityEngine;
using UnityEngine.Timeline;

namespace SimpleEyeController.View
{
    /// <summary>
    /// 回転軸やローカルの回転がめちゃくちゃでもいい感じに視線制御するサンプル
    /// Start()時に顔が正面に向いている必要があります(TスタンスやAスタンスであれば大丈夫です)
    /// </summary>
    [ExecuteAlways]
    [DisallowMultipleComponent]
    public class EyeController : MonoBehaviour, ITimeControl
    {
        public UpdateMethod updateMethod = UpdateMethod.LateUpdate;
        
        public EyeAssignMethod assignMethod = EyeAssignMethod.Animator;
        
        public Animator animator;
        
        public Transform manualEyeL;
        public Transform manualEyeR;
        
        public EyeRangeSetting rangeSetting;

        public Transform CurrentEyeL => _currentEyeL.Bone;
        public Transform CurrentEyeR => _currentEyeR.Bone;

        private EyeDefaultStatus _currentEyeL;
        private EyeDefaultStatus _currentEyeR;

        private List<IEyeProcess> _processes = new List<IEyeProcess>();

        private void Start()
        {
            GetRequiredComponents();
            ChangeEyeBones();
        }

        private void OnEnable()
        {
            ChangeEyeBones();
        }

        public void ChangeEyeBones()
        {
            if (!Application.isPlaying) return;
            GetEyeDefaultStatusBones(out var eyeL, out var eyeR);
            _currentEyeL = eyeL;
            _currentEyeR = eyeR;
            
            var rotator = new DoubleEyeRotator(eyeL, eyeR, rangeSetting);
            foreach (var process in _processes)
            {
                process.Rotator = rotator;
            }
        }
        
        private void GetEyeDefaultStatusBones(out EyeDefaultStatus eyeL, out EyeDefaultStatus eyeR)
        {
            switch (assignMethod)
            {
                case EyeAssignMethod.Animator:
                    // TransformとAnimatorのみのクローンを作成して、デフォルトの目の回転を取得する
                    var clonedParent = CreateTransformTreeClone(animator.transform);
                    var clonedAnimator = clonedParent.gameObject.AddComponent<Animator>();
                    clonedAnimator.avatar = animator.avatar;

                    var handler = new HumanPoseHandler(clonedAnimator.avatar, clonedAnimator.transform);
                    var humanPose = new HumanPose();
                    handler.GetHumanPose(ref humanPose);
                    for (var i = 0; i < humanPose.muscles.Length; i++)
                    {
                        humanPose.muscles[i] = 0;
                    }

                    humanPose.bodyPosition = Vector3.zero;
                    humanPose.bodyRotation = Quaternion.identity;
                    clonedAnimator.transform.rotation = Quaternion.identity;

                    handler.SetHumanPose(ref humanPose);

                    eyeL = new EyeDefaultStatus(
                        animator.GetBoneTransform(HumanBodyBones.LeftEye),
                        clonedAnimator.GetBoneTransform(HumanBodyBones.LeftEye),
                        EyeType.Left
                    );
                    eyeR = new EyeDefaultStatus(
                        animator.GetBoneTransform(HumanBodyBones.RightEye),
                        clonedAnimator.GetBoneTransform(HumanBodyBones.RightEye),
                        EyeType.Right
                    );

                    Destroy(clonedParent.gameObject);
                    break;
                // case EyeAssignMethod.Transform:
                //     eyeL = manualEyeL;
                //     eyeR = manualEyeR;
                //     break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Transformの木構造のコピーを行う
        /// </summary>
        /// <param name="originalParent"></param>
        /// <returns></returns>
        private Transform CreateTransformTreeClone(Transform originalParent)
        {
            var parentCloned = CreateTransformClone(originalParent);
            CreateCloneRecursive(originalParent, parentCloned);
            return parentCloned;
        }

        /// <summary>
        /// 幅優先でTransformのコピーを行う
        /// </summary>
        /// <param name="originalParent"></param>
        /// <param name="clonedParent"></param>
        private void CreateCloneRecursive(Transform originalParent, Transform clonedParent)
        {
            foreach (Transform originalChild in originalParent)
            {
                var clone = CreateTransformClone(originalChild, true);
                clone.transform.SetParent(clonedParent);
                CreateCloneRecursive(originalChild, clone);
            }
        }

        /// <summary>
        /// Transformのコピーを作成する
        /// </summary>
        /// <param name="original"></param>
        /// <param name="addDebugCube"></param>
        /// <returns></returns>
        private Transform CreateTransformClone(Transform original, bool addDebugCube = false)
        {
            var clone = new GameObject();
            clone.transform.position = original.position;
            clone.transform.rotation = original.rotation;
            clone.transform.localScale = original.localScale;
            clone.name = original.name;

            // デバッグ用にCubeを追加する
            if (addDebugCube)
            {
                var debugCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                debugCube.transform.SetParent(clone.transform);
                debugCube.transform.localScale = Vector3.one * 0.01f;
                debugCube.transform.localPosition = Vector3.zero;
                debugCube.transform.localRotation = Quaternion.identity;
            }

            return clone.transform;
        }

        private void UpdateInternal()
        {
            //TODO : エディターでも動くようにする
            if (!Application.isPlaying) return;
            foreach (var process in _processes.OrderBy(x=>x.ExecutionOrder))
            {
                process.Progress(Time.time);
            }
        }

        private void Update()
        {
            if (updateMethod == UpdateMethod.Update)
            {
                UpdateInternal();
            }
        }

        private void LateUpdate()
        {
            if (updateMethod == UpdateMethod.LateUpdate)
            {
                UpdateInternal();
            }
        }

        private void FixedUpdate()
        {
            if (updateMethod == UpdateMethod.FixedUpdate)
            {
                UpdateInternal();
            }
        }

        public void ManualUpdate()
        {
            if (updateMethod == UpdateMethod.Manual)
            {
                UpdateInternal();
            }
        }

        private void Reset()
        {
            GetRequiredComponents();
        }

        private void GetRequiredComponents()
        {
            if (animator == null)
            {
                animator = gameObject.GetOrAddComponent<Animator>();
            }

            _processes = GetComponents<IEyeProcess>().ToList();
        }

        public void SetTime(double time)
        {
            throw new NotImplementedException();
        }

        public void OnControlTimeStart()
        {
            throw new NotImplementedException();
        }

        public void OnControlTimeStop()
        {
            throw new NotImplementedException();
        }
    }
}