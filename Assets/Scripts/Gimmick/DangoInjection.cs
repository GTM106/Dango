using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TM.Easing;
using TM.Easing.Management;
using TMPro;
using System;
using static FloorManager;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class DangoInjection : MonoBehaviour
{
    [Flags]
    private enum DangoColorChoice
    {
        [InspectorName("�S������")]
        None = 0,

        Red = 1 << 1,
        Orange = 1 << 2,
        Yellow = 1 << 3,
        Green = 1 << 4,
        Cyan = 1 << 5,
        Blue = 1 << 6,
        Purple = 1 << 7,

        [InspectorName("")]
        All = 8,
        [InspectorName("�S�I��")]
        SET_ALL = ~0,
    }

    private enum ShotMode
    {
        Random,
        Fixed
    }

    [SerializeField, Tooltip("���˃��[�h")] ShotMode shotMode;
    [SerializeField, Tooltip("�o���F")] DangoColorChoice colorChoice;
    [SerializeField, Tooltip("�c�q�̎ˏo�|�C���g")] GameObject spawner = default!;

    [SerializeField, Tooltip("��x�ɔ��˂��鐔"), Min(1)] private int defalutInjectionCount = 1;
    [SerializeField, Tooltip("2�ȏ㔭�˂���Ƃ��̊Ԋu"), Min(0)] private int defalutContinueFrame = 0;
    [SerializeField, Tooltip("�A�j���[�V��������"), Min(0)] private int animationFrame = 40;
    [SerializeField, Tooltip("�A�j���[�V�����I��肩�甭�˂܂ł̑ҋ@����"), Min(0)] private int shotWaitFrame = 10;
    [SerializeField, Tooltip("�A�j���[�V�����܂ł̃N�[���^�C��"), Min(1)] private int animationWaitFrame = 10;

    [SerializeField, Tooltip("�ˏo��"), Min(0.1f)] private float shotPower = 10f;

    [SerializeField, Tooltip("�c�����̉���")] Vector2 verticalRot;
    [SerializeField, Tooltip("�������̉���")] Vector2 horizontalRot;

    [SerializeField] DangoInjectionFixed[] fixedAngles;
    [SerializeField] FloorManager floorManager;

    [SerializeField] bool _canShot = true;

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

    private Floor _floor;

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
            if (!parent._canShot) return IState.E_State.WaitInjection;
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

        InitState();
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
        var color = dangoColors[UnityEngine.Random.Range(0, dangoColors.Count)];

        //�I�u�W�F�N�g�v�[������c�q�����o���Đݒ�B
        _poolManager.SetCreateColor(color);
        var dango = _poolManager.DangoPool[(int)color - 1].Get();
        dango.SetDangoColor(color);

        dango.transform.position = spawner.transform.position;
        dango.Rb.AddForce(transform.forward.normalized * shotPower, ForceMode.Impulse);
        _continueFrame = defalutContinueFrame;
        dango.SetFloor(_floor);

        floorManager.FloorArrays[(int)_floor].AddDangoCount();

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
        if (shotMode == ShotMode.Random)
        {
            _nextLookAngle = _firstLookAngle + new Vector3(UnityEngine.Random.Range(verticalRot.x, verticalRot.y), UnityEngine.Random.Range(horizontalRot.x, horizontalRot.y), 0);
        }
        else
        {
            if (fixedAngles.Length == 0) return;
            int index = UnityEngine.Random.Range(0, fixedAngles.Length);
            _nextLookAngle = _firstLookAngle.SetX(fixedAngles[index].CannonAngle).SetY(fixedAngles[index].BaseAngle);
        }

        //��Ԓl�����߂�
        _interpolatedVec = _nextLookAngle - _lookedAngle;
    }

    private float Around(float val)
    {
        if (val > 180f) return val - 360f;
        if (val < -180f) return val + 360f;

        return val;
    }

    public void SetFloor(Floor floor) => _floor = floor;

    public void SetCanShot(bool canShot) => _canShot = canShot;
    public bool CanShot => _canShot;

    [Serializable]
    class DangoInjectionFixed
    {
        [SerializeField, Range(0, 360f)] float _baseAngle;
        [SerializeField, Range(0, 360f)] float _cannonAngle;

        [NonSerialized] public float ba = 0;
        [NonSerialized] public float ca = 0;

        public float BaseAngle => _baseAngle;
        public float CannonAngle => _cannonAngle;
        public void SetBaseAngle(float value) => _baseAngle = value;
        public void SetCannonAngle(float value) => _cannonAngle = value;

    }


#if UNITY_EDITOR
    [CustomEditor(typeof(DangoInjection))]
    [CanEditMultipleObjects]
    public class DangoInjectionEditor : Editor
    {
        private DangoInjection dangoInjection;

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            dangoInjection = target as DangoInjection;
            base.OnInspectorGUI();

            if (dangoInjection.fixedAngles.Length == 0) return;

            if (GUILayout.Button("Copy"))
            {
                dangoInjection.fixedAngles[^1].ba = dangoInjection.fixedAngles[^1].BaseAngle;
                dangoInjection.fixedAngles[^1].ca = dangoInjection.fixedAngles[^1].CannonAngle;
                dangoInjection.fixedAngles[^1].SetBaseAngle(dangoInjection.transform.localEulerAngles.y);
                dangoInjection.fixedAngles[^1].SetCannonAngle(dangoInjection.transform.localEulerAngles.x);
            }
            if (GUILayout.Button("Redo"))
            {
                dangoInjection.fixedAngles[^1].SetBaseAngle(dangoInjection.fixedAngles[^1].ba);
                dangoInjection.fixedAngles[^1].SetCannonAngle(dangoInjection.fixedAngles[^1].ca);
            }
        }

    }
#endif
}