using System;
using System.Collections.Generic;
using System.Linq;
using UniEyeController.Core.Constants;
using UniEyeController.Core.Extensions;
using UniEyeController.Core.Rotator;
using UniEyeController.Core.Setting;
using UniEyeController.Core.Status;
using UnityEngine;
using UnityEngine.Timeline;

namespace UniEyeController
{
    [ExecuteAlways]
    [DisallowMultipleComponent]
    public class UniEyeController : MonoBehaviour, ITimeControl
    {
        
        public EyeAssignMethod assignMethod = EyeAssignMethod.Humanoid;
        
        public Animator animator;

        public GameObject prefabForGenericAvatar;
        
        public Transform manualEyeL;
        public Transform manualEyeR;
        
        public EyeRangeSetting rangeSetting;

        public Transform CurrentEyeL => _currentEyeL.Bone;
        public Transform CurrentEyeR => _currentEyeR.Bone;

        private EyeDefaultStatus _currentEyeL;
        private EyeDefaultStatus _currentEyeR;

        private List<EyeProcess.EyeProcessBase> _processes = new List<EyeProcess.EyeProcessBase>();

        private void Start()
        {
            Init();
        }

        public void Init()
        {
            GetRequiredComponents();
            ChangeEyeBones();
        }

        public void ChangeEyeBones()
        {
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
                case EyeAssignMethod.Humanoid:
                    // TransformとAnimatorのみのクローンを作成して、デフォルトの目の回転を取得する
                    if (animator == null)
                    {
                        Debug.LogError("Animatorが設定されていません");
                        eyeL = eyeR = null;
                        return;
                    }

                    if (!animator.isHuman)
                    {
                        Debug.LogError("AnimatorがHumanoidではありません");
                        eyeL = eyeR = null;
                        return;
                    }
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

                    DestroyImmediate(clonedParent.gameObject);
                    break;
                case EyeAssignMethod.Generic:
                    var eyeLPath = manualEyeL.GetFullPath(transform);
                    var eyeRPath = manualEyeR.GetFullPath(transform);
                    eyeL = new EyeDefaultStatus(
                        manualEyeL,
                        prefabForGenericAvatar.transform.Find(eyeLPath),
                        EyeType.Left
                    );
                    eyeR = new EyeDefaultStatus(
                        manualEyeR,
                        prefabForGenericAvatar.transform.Find(eyeRPath),
                        EyeType.Right
                    );
                    break;
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

        private void UpdateInternal(UpdateMethod updateMethod)
        {
            var processes =
                _processes
                    .Where(x => x.updateMethod == updateMethod)
                    .Where(x => x.CanExecute)
                    .OrderBy(x => x.executionOrder);
            
            foreach (var process in processes)
            {
                process.Progress(Time.time, false);
            }
        }

        private void Update()
        {
            UpdateInternal(UpdateMethod.Update);
        }

        private void LateUpdate()
        {
            UpdateInternal(UpdateMethod.LateUpdate);
        }

        private void FixedUpdate()
        {
            UpdateInternal(UpdateMethod.FixedUpdate);
        }

        public void ManualUpdate()
        {
            UpdateInternal(UpdateMethod.Manual);
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

            _processes = GetComponents<EyeProcess.EyeProcessBase>().ToList();
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