using System;
using UnityEngine;

/// <summary>
/// 両目の視線制御
/// 初期化時に顔が正面に向いている必要があります(TスタンスやAスタンスであれば大丈夫です)
/// </summary>
public class EyesController
{
    private SingleEyeController _eyeL;
    private SingleEyeController _eyeR;

    /// <summary>
    /// 初期化
    /// </summary>
    /// <param name="animator"></param>
    /// <exception cref="Exception"></exception>
    public EyesController(Animator animator)
    {
        var eyeTransformL = animator.GetBoneTransform(HumanBodyBones.LeftEye);
        var eyeTransformR = animator.GetBoneTransform(HumanBodyBones.RightEye);
        if (eyeTransformL == null || eyeTransformR == null)
        {
            throw new Exception($"Both eyes must be assigned to Avatar.");
        }

        _eyeL = new SingleEyeController(animator, eyeTransformL);
        _eyeR = new SingleEyeController(animator, eyeTransformR);
    }
    /// <summary>
    /// ターゲットの方向を向く
    /// </summary>
    /// <param name="target">見る対象</param>
    public void LookAt(Transform target)
    {
        LookAt(target.position);
    }

    /// <summary>
    /// ターゲットの方向を向く
    /// </summary>
    /// <param name="worldPosition"></param>
    public void LookAt(Vector3 worldPosition)
    {
        _eyeL.LookAt(worldPosition);
        _eyeR.LookAt(worldPosition);
    }

    /// <summary>
    /// 目を回転させる
    /// </summary>
    /// <param name="yaw">左右</param>
    /// <param name="pitch">上下</param>
    public void Rotate(float yaw, float pitch)
    {
        _eyeL.Rotate(yaw, pitch);
        _eyeR.Rotate(yaw, pitch);
    }
}