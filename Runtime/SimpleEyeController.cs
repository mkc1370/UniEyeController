using UnityEngine;

/// <summary>
/// 回転軸やローカルの回転がめちゃくちゃでもいい感じに視線制御するサンプル
/// Start()時に顔が正面に向いている必要があります(TスタンスやAスタンスであれば大丈夫です)
/// </summary>
[RequireComponent(typeof(Animator))]
public class SimpleEyeController : MonoBehaviour
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