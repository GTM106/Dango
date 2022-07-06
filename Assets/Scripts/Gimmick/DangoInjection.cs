using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TM.Easing;
using TM.Easing.Management;

public class DangoInjection : MonoBehaviour
{
    [System.Flags]
    private enum DangoColorChoice
    {
        None = 0,

        Red = 1 << 1,
        Orange = 1 << 2,
        Yellow = 1 << 3,
        Green = 1 << 4,
        Cyan = 1 << 5,
        Blue = 1 << 6,
        Purple = 1 << 7,

        All = 8,
        SET_ALL = ~0,
    }

    [SerializeField, Tooltip("�o���F")] DangoColorChoice colorChoice;
    [SerializeField, Tooltip("�c�q�̎ˏo�|�C���g")] GameObject spawner = default!;

    [SerializeField, Tooltip("��x�ɔ��˂��鐔"), Min(1)] private int defalutInjectionCount = 1;
    [SerializeField, Tooltip("2�ȏ㔭�˂���Ƃ��̊Ԋu"), Min(0)] private int defalutContinueFrame = 0;
    [SerializeField, Tooltip("�A�j���[�V��������"), Min(0)] private int animationFrame = 40;
    [SerializeField, Tooltip("�A�j���[�V�����I��肩�甭�˂܂ł̑ҋ@����"), Min(0)] private int shotWaitFrame = 10;
    [SerializeField, Tooltip("�A�j���[�V�����܂ł̃N�[���^�C��"), Min(0)] private int animationWaitFrame = 10;

    [SerializeField, Tooltip("�ˏo��"), Min(0.1f)] private float shotPower = 10f;

    [SerializeField, Tooltip("�c�����̉���")] Vector2 verticalRot;
    [SerializeField, Tooltip("�������̉���")] Vector2 horizontalRot;

    private DangoPoolManager _poolManager = default!;

    private Vector3 _nextLookAngle = default;
    private Vector3 _lookedAngle = default;
    private Vector3 _firstLookAngle = default;
    private Vector3 _interpolatedVec = default;

    private int _injectionCount = default;
    private int _continueFrame = default;
    private int _currentAnimFrame = default;
    private int _shotWaitFrame = default;
    private int _animWaitFrame = default;

    private List<DangoColor> dangoColors = new();

    interface IState
    {
        public enum E_State
        {
            Injection,
            Animation,
            WaitInjection,
            WaitAnimation,

            Max,

            Unchanged,
        }

        public E_State Init(DangoInjection parent);
        public E_State Update(DangoInjection parent);
    }

    //��ԊǗ�
    private IState.E_State _currentState = IState.E_State.Injection;
    private static readonly IState[] states = new IState[(int)IState.E_State.Max]
     {
         new InjectionState(),
         new AnimationState(),
         new WaitInjectionState(),
         new WaitAnimationState(),
     };

    class InjectionState : IState
    {
        public IState.E_State Init(DangoInjection parent)
        {
            return IState.E_State.Unchanged;
        }
        public IState.E_State Update(DangoInjection parent)
        {
            return parent.Injection() ? IState.E_State.WaitAnimation : IState.E_State.Unchanged;
        }
    }
    class AnimationState : IState
    {
        public IState.E_State Init(DangoInjection parent)
        {
            parent._currentAnimFrame = 0;
            return IState.E_State.Unchanged;
        }
        public IState.E_State Update(DangoInjection parent)
        {
            return parent.IsSmoothLookRotation() ? IState.E_State.WaitInjection : IState.E_State.Unchanged;
        }
    }
    class WaitInjectionState : IState
    {
        public IState.E_State Init(DangoInjection parent)
        {
            parent._shotWaitFrame = parent.shotWaitFrame;
            return IState.E_State.Unchanged;
        }
        public IState.E_State Update(DangoInjection parent)
        {
            return --parent._shotWaitFrame == 0 ? IState.E_State.Injection : IState.E_State.Unchanged;
        }
    }
    class WaitAnimationState : IState
    {
        public IState.E_State Init(DangoInjection parent)
        {
            parent._animWaitFrame = parent.animationWaitFrame;
            return IState.E_State.Unchanged;
        }
        public IState.E_State Update(DangoInjection parent)
        {
            return --parent._animWaitFrame == 0 ? IState.E_State.Animation : IState.E_State.Unchanged;
        }
    }

    private void InitState()
    {
        Logger.Assert(_currentState is >= 0 and < IState.E_State.Max);

        var nextState = states[(int)_currentState].Init(this);

        if (nextState != IState.E_State.Unchanged)
        {
            _currentState = nextState;
            InitState();//�������ŏ�Ԃ��ς��Ȃ�ċA�I�ɏ���������B
        }
    }
    private void UpdateState()
    {
        Logger.Assert(_currentState is >= 0 and < IState.E_State.Max);

        var nextState = states[(int)_currentState].Update(this);

        if (nextState != IState.E_State.Unchanged)
        {
            //���ɑJ��
            _currentState = nextState;
            InitState();
        }
    }

    private void Awake()
    {
        Logger.Assert(verticalRot.x < verticalRot.y);
        Logger.Assert(horizontalRot.x < horizontalRot.y);
        Logger.Assert(colorChoice != DangoColorChoice.None);

        //������
        _poolManager = GameObject.Find("DangoPoolManager").GetComponent<DangoPoolManager>();
        _firstLookAngle = transform.rotation.eulerAngles;
        NextLook();

        for (int i = 1; i < (int)DangoColorChoice.All; i++)
        {
            if (colorChoice.HasFlag((DangoColorChoice)(1 << i)))
            {
                dangoColors.Add((DangoColor)i);
            }
        }
    }

    private void FixedUpdate()
    {
        UpdateState();
    }

    private bool Injection()
    {
        //�����m��
        transform.rotation = Quaternion.Euler(_nextLookAngle);

        //�A���ł���FRAME���Ǘ�
        if (--_continueFrame > 0) return false;

        //�c�q����
        if (!ShotDango()) return false;

        //���̔��˒n�_������
        NextLook();

        return true;
    }

    private bool ShotDango()
    {
        if (dangoColors.Count == 0) return false;

        //�I�������F�̒����烉���_���ɐF���擾
        //�������_���Ȃ��ߊm����ݒ�ł���悤�ɕύX������
        var color = dangoColors[Random.Range(0, dangoColors.Count)];

        //�I�u�W�F�N�g�v�[������c�q�����o���Đݒ�B
        var dango = _poolManager.DangoPool.Get();
        dango.SetDangoColor(color);
        dango.Rend.material.color = dango.GetDangoColor() switch
        {
            DangoColor.Red => Color.red,
            DangoColor.Orange => new Color32(255, 155, 0, 255),
            DangoColor.Yellow => Color.yellow,
            DangoColor.Green => Color.green,
            DangoColor.Cyan => Color.cyan,
            DangoColor.Blue => Color.blue,
            DangoColor.Purple => new Color32(200, 0, 255, 255),
            DangoColor.Other => Color.gray,
            _ => Color.white,
        };

        dango.transform.position = spawner.transform.position;
        dango.Rb.AddForce(transform.forward.normalized * shotPower, ForceMode.Impulse);
        _continueFrame = defalutContinueFrame;

        return --_injectionCount <= 0;
    }

    private bool IsSmoothLookRotation()
    {
        var progress = EasingManager.EaseProgress(EaseType.OutBack, ++_currentAnimFrame, animationFrame, 1f, 0);

        transform.rotation = Quaternion.Euler(_lookedAngle + (_interpolatedVec * progress));

        return _currentAnimFrame == animationFrame;
    }

    private void NextLook()
    {
        //���݂̈ʒu�����
        _lookedAngle = transform.rotation.eulerAngles;
        
        //�ʒu�̕␳
        _lookedAngle.Set(Around(_lookedAngle.x), Around(_lookedAngle.y), Around(_lookedAngle.z));

        //������
        _continueFrame = 0;
        _injectionCount = defalutInjectionCount;

        //���̈ʒu�̒��I
        _nextLookAngle = _firstLookAngle + new Vector3(Random.Range(verticalRot.x, verticalRot.y), Random.Range(horizontalRot.x, horizontalRot.y), 0);
        
        //��Ԓl�����߂�
        _interpolatedVec = _nextLookAngle - _lookedAngle;
    }

    private float Around(float val)
    {
        if (val > 180) return val - 360f;
        if (val < -180) return val + 360;

        return val;
    }
}