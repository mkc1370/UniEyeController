using UnityEngine;

/// <summary>
/// 片目の視線制御
/// 初期化時に顔が正面に向いている必要があります(TスタンスやAスタンスであれば大丈夫です)
/// </summary>
public class SingleEyeController
{
    private Transform _eyeBone;
    private Quaternion DefaultRotation;
    private Quaternion DefaultLocalRotation;

    /// <summary>
    /// 左右に動かす範囲(度数法)
    /// [-YawRange, YawRange]
    /// </summary>
    public const float YawRange = 20;

    /// <summary>
    /// 上下に動かす範囲(度数法)
    /// [-PitchRange, PitchRange]
    /// </summary>
    public const float PitchRange = 10;

    public SingleEyeController(Animator animator, Transform eyeBone)
    {
        _eyeBone = eyeBone;
        DefaultRotation = animator.transform.rotation * eyeBone.rotation;
        DefaultLocalRotation = eyeBone.localRotation;
    }

    /// <summary>
    /// ターゲットの方向を向く
    /// </summary>
    /// <param name="worldPosition"></param>
    public void LookAt(Vector3 worldPosition)
    {
        // 一度Y-Upに直してから方向を計算する
        _eyeBone.localRotation = DefaultLocalRotation * Quaternion.Inverse(DefaultRotation);
        var targetLocalPosition = _eyeBone.InverseTransformPoint(worldPosition);

        // 向くのをやめはじめる距離
        var lookAtLerpStartZ = 0.5f;
        // 向くのを完全にやめる距離
        var lookAtLerpEndZ = -0.3f;
        // どれだけターゲットの方向を見るかの重み(これにより見るのをやめるときの動作をスムーズにできる)
        var weight = 1 - Mathf.InverseLerp(lookAtLerpStartZ, lookAtLerpEndZ, targetLocalPosition.z);

        var yaw = Mathf.Atan2(targetLocalPosition.x, targetLocalPosition.z) * Mathf.Rad2Deg * weight;
        var pitch = Mathf.Atan2(targetLocalPosition.y, targetLocalPosition.z) * Mathf.Rad2Deg * weight;

        Rotate(yaw, pitch);
    }

    /// <summary>
    /// 目を回転させる
    /// </summary>
    /// <param name="yaw">左右</param>
    /// <param name="pitch">上下</param>
    public void Rotate(float yaw, float pitch)
    {
        var adjustedYaw = Mathf.Clamp(yaw, -YawRange, YawRange);
        var adjustedPitch = Mathf.Clamp(pitch, -PitchRange, PitchRange);

        _eyeBone.localRotation =
            DefaultLocalRotation *
            Quaternion.Inverse(DefaultRotation) *
            Quaternion.AngleAxis(adjustedYaw, Vector3.up) *
            Quaternion.AngleAxis(adjustedPitch, Vector3.left) *
            DefaultRotation;
    }
}