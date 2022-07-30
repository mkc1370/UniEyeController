using System;
using UnityEngine;

/// <summary>
/// 回転軸やローカルの回転がめちゃくちゃでもいい感じに視線制御するサンプル
/// Start()時に顔が正面に向いている必要があります(TスタンスやAスタンスであれば大丈夫です)
/// </summary>
[RequireComponent(typeof(Animator))]
public class EyeControllerSample : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    
    [Header("ターゲットを見るか角度で指定するか")]
    [SerializeField] private bool _useTarget;
    
    [Header("見る対象")]
    [SerializeField] private Transform _target;

    [Header("目の角度(左右)")]
    [Range(-SingleEyeController.YawRange, SingleEyeController.YawRange)]
    [SerializeField] private float _yaw;
    
    [Header("目の角度(上下)")]
    [Range(-SingleEyeController.PitchRange, SingleEyeController.PitchRange)]
    [SerializeField] private float _pitch;

    private EyesController _eyesController;

    private void Start()
    {
        _eyesController = new EyesController(_animator);
    }

    private void Update()
    {
        if (_useTarget)
        {
            _eyesController.LookAt(_target);
        }
        else
        {
            _eyesController.Rotate(_yaw, _pitch);
        }
    }

    private void Reset()
    {
        _animator = GetComponent<Animator>();
    }
}

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
