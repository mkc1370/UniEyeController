using UniEyeController.Core.Constants;
using UniEyeController.Core.Extensions;
using UnityEngine;

namespace UniEyeController.Core.Controller.Eye
{
    /// <summary>
    /// 目のボーンの初期状態を保持するクラス
    /// </summary>
    public class DoubleEyeDefaultStatus
    {
        public SingleEyeDefaultStatus EyeL;
        public SingleEyeDefaultStatus EyeR;

        public static DoubleEyeDefaultStatus CreateFromHumanoid(Animator animator)
        {
            // TransformとAnimatorのみのクローンを作成して、デフォルトの目の回転を取得する
            if (animator == null)
            {
                Debug.LogError("Animatorが設定されていません");
                return null;
            }

            if (!animator.isHuman)
            {
                Debug.LogError("AnimatorがHumanoidではありません");
                return null;
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

            var eyeL = new SingleEyeDefaultStatus(
                animator.GetBoneTransform(HumanBodyBones.LeftEye),
                clonedAnimator.GetBoneTransform(HumanBodyBones.LeftEye),
                EyeType.Left
            );

            var eyeR = new SingleEyeDefaultStatus(
                animator.GetBoneTransform(HumanBodyBones.RightEye),
                clonedAnimator.GetBoneTransform(HumanBodyBones.RightEye),
                EyeType.Right
            );

            Object.DestroyImmediate(clonedParent.gameObject);
            return new DoubleEyeDefaultStatus()
            {
                EyeL = eyeL,
                EyeR = eyeR
            };
        }

        public static DoubleEyeDefaultStatus CreateFromGeneric(
            Transform transform,
            Transform manualEyeL,
            Transform manualEyeR,
            GameObject prefabForGenericAvatar
        )
        {
            var eyeLPath = manualEyeL.GetFullPath(transform);
            var eyeRPath = manualEyeR.GetFullPath(transform);
            var eyeL = new SingleEyeDefaultStatus(
                manualEyeL,
                prefabForGenericAvatar.transform.Find(eyeLPath),
                EyeType.Left
            );
            var eyeR = new SingleEyeDefaultStatus(
                manualEyeR,
                prefabForGenericAvatar.transform.Find(eyeRPath),
                EyeType.Right
            );
            return new DoubleEyeDefaultStatus()
            {
                EyeL = eyeL,
                EyeR = eyeR
            };
        }

        /// <summary>
        /// Transformの木構造のコピーを行う
        /// </summary>
        /// <param name="originalParent"></param>
        /// <returns></returns>
        private static Transform CreateTransformTreeClone(Transform originalParent)
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
        private static void CreateCloneRecursive(Transform originalParent, Transform clonedParent)
        {
            foreach (Transform originalChild in originalParent)
            {
                var clone = CreateTransformClone(originalChild);
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
        private static Transform CreateTransformClone(Transform original, bool addDebugCube = false)
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
    }
}