using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

/// 
/// ���������邱�Ɓ�
/// �E�ڒn������܂Ƃ���
/// 

[RequireComponent(typeof(Rigidbody), typeof(RideMoveObj))]
class PlayerData : MonoBehaviour
{
    #region inputSystem
    //�X�R�A�Ɩ����x�̃��[�g
    const float SCORE_TIME_RATE = 0.2f;

    //���͒l
    private Vector2 _moveAxis;
    private Vector2 _roteAxis;

    private bool _hasAttacked = false;
    private bool _hasFalled = false;
    private bool _hasStayedEat = false;
    private bool _hasJumped = false;

    [SerializeField] private Rigidbody rb = default!;
    [SerializeField] private CapsuleCollider capsuleCollider = default!;
    [SerializeField] private GameObject playerCamera = default!;

    private DangoRole dangoRole = DangoRole.instance;

    private PlayerFallAction _playerFall = new();
    private PlayerAttackAction _playerAttack = new();

    public PlayerFallAction PlayerFall => _playerFall;

    //�ړ�����
    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            _moveAxis = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            _moveAxis = Vector2.zero;
        }
    }

    //�W�����v����
    public void OnJump(InputAction.CallbackContext context)
    {
        if (!IsGround) return;

        if (context.phase == InputActionPhase.Performed)
        {
            _hasJumped = true;
        }
    }

    //�c�q�e(���O��)
    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            //���ɉ����Ȃ���������s���Ȃ��B
            if (_dangos.Count == 0) return;

            //[Debug]�������������킩����
            //���܂ł́Adangos[dangos.Count - 1]�Ƃ��Ȃ���΂Ȃ�܂���ł������A
            //C#8.0�ȍ~�ł͈ȉ��̂悤�ɏȗ��ł���悤�ł��B
            //���́A�����m��Ȃ��l���ǂނƂ킯��������Ȃ��B
            Logger.Log(_dangos[^1]);

            //SE
            GameManager.SoundManager.PlaySE(SoundSource.SE_REMOVE_DANGO);

            //���������B
            _dangos.RemoveAt(_dangos.Count - 1);
            _dangoUISC.DangoUISet(_dangos);
        }
    }

    //�˂��h���A�j���[�V����
    public void OnAttack(InputAction.CallbackContext context)
    {
        //�����A�N�V�������󂯕t���Ȃ��B
        if (_playerFall.IsFallAction) return;

        if (context.phase == InputActionPhase.Performed)
        {
            //�󒆂��n�ʂɋ߂����Ȃ���Η����h���Ɉڍs
            if (!_isGround)
            {
                Ray ray = new(transform.position, Vector3.down);

                //�߂��ɒn�ʂ����邩(player�̔����̑傫��)����
                if (Physics.Raycast(ray, capsuleCollider.height + capsuleCollider.height / 2f)) _hasAttacked = true;
                else _hasFalled = true;
            }
            //�n�ʂȂ畁�ʂɓ˂��h���Ɉڍs
            else
            {
                _hasAttacked = true;
            }
        }
    }

    //�H�ׂ�
    public void OnEatDango(InputAction.CallbackContext context)
    {
        //���Ɏh�����ĂȂ���������s���Ȃ��B
        if (_dangos.Count == 0) return;

        if (context.phase == InputActionPhase.Performed) _hasStayedEat = true;
        else if (context.phase == InputActionPhase.Canceled) _hasStayedEat = false;
    }

    //��]����
    public void OnRote(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            _roteAxis = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            _roteAxis = Vector2.zero;
        }

    }

    //�i����g�p���܂���j
    public void OnCompression(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            //for(int i = 0; i < debuffs.; i++)
        }
    }

    private void EatDango()
    {
        //SE
        GameManager.SoundManager.PlaySE(SoundSource.SE_PLAYER_EATDANGO);

        _hasStayedEat = false;

        //�H�ׂ��c�q�̓_�����擾
        var score = dangoRole.CheckRole(_dangos);

        //���o�֐��̌Ăяo��
        _directing.Dirrecting(_dangos);

        _playerUIManager.SetEventText("�H�ׂ��I" + (int)score + "�_�I");

        //�c�莞�Ԃ̑���
        PlayerUIManager.time += score;

        //�����x���㏸
        _satiety += score * SCORE_TIME_RATE;

        //�X�R�A���㏸
        GameManager.GameScore += score * 100f;

        //�����N���A�B
        _dangos.Clear();
        //UI�X�V
        _dangoUISC.DangoUISet(_dangos);
    }

    private void ResetSpit()
    {
        spitManager.isSticking = false;
        spitManager.gameObject.transform.localRotation = Quaternion.identity;
        spitManager.gameObject.transform.localPosition = new Vector3(0, 0.4f, 1.1f);
    }
    #endregion

    #region statePattern
    interface IState
    {
        public enum E_State
        {
            Control = 0,
            FallAction = 1,
            AttackAction = 2,
            StayEatDango = 3,
            EatDango = 4,
            GrowStab = 5,

            Max,

            Unchanged,
        }

        E_State Initialize(PlayerData parent);
        E_State Update(PlayerData parent);
        E_State FixedUpdate(PlayerData parent);
    }
    //��ԊǗ�
    private IState.E_State _currentState = IState.E_State.Control;
    private static readonly IState[] states = new IState[(int)IState.E_State.Max]
     {
        new ControlState(),
        new FallActionState(),
        new AttackActionState(),
        new StayEatDangoState(),
        new EatDangoState(),
        new GrowStabState(),
     };

    class ControlState : IState
    {
        public IState.E_State Initialize(PlayerData parent)
        {
            //���̈ʒu�����Z�b�g
            parent.ResetSpit();
            return IState.E_State.Unchanged;
        }
        public IState.E_State Update(PlayerData parent)
        {
            return IState.E_State.Unchanged;
        }
        public IState.E_State FixedUpdate(PlayerData parent)
        {
            //�v���C���[�𓮂�������
            parent.PlayerMove(parent._cameraForward);

            //�X�e�[�g�Ɉڍs�B
            if (parent._hasAttacked) return IState.E_State.AttackAction;
            if (parent._hasStayedEat) return IState.E_State.StayEatDango;
            if (parent._hasFalled) return IState.E_State.FallAction;

            return IState.E_State.Unchanged;
        }
    }
    class FallActionState : IState
    {
        public IState.E_State Initialize(PlayerData parent)
        {
            parent._hasFalled = false;
            parent._playerFall.IsFallAction = true;
            return IState.E_State.Unchanged;
        }
        public IState.E_State Update(PlayerData parent)
        {
            return IState.E_State.Unchanged;
        }
        public IState.E_State FixedUpdate(PlayerData parent)
        {
            //�ړ�
            parent.PlayerMove(parent._cameraForward);

            //�r���Őڒn������R���g���[���ɖ߂�
            if (parent.IsGround) return IState.E_State.Control;

            //�ҋ@���Ԃ��I�������A�^�b�N�X�e�[�g�Ɉڍs
            return parent._playerFall.FixedUpdate(parent.rb, parent.spitManager)
                ? IState.E_State.AttackAction : IState.E_State.Unchanged;
        }
    }
    class AttackActionState : IState
    {
        public IState.E_State Initialize(PlayerData parent)
        {
            parent._hasAttacked = false;

            if (!parent.CanStab()) return IState.E_State.Control;
            return IState.E_State.Unchanged;
        }
        public IState.E_State Update(PlayerData parent)
        {
            return IState.E_State.Unchanged;
        }
        public IState.E_State FixedUpdate(PlayerData parent)
        {
            return parent._playerAttack.FixedUpdate() ? IState.E_State.Control : IState.E_State.Unchanged;
        }

    }
    class StayEatDangoState : IState
    {
        public IState.E_State Initialize(PlayerData parent)
        {
            parent._playerUIManager.SetEventText("�H�׃`���[�W���I");
            parent._playerStayEat.ResetCount();
            //SE����

            return IState.E_State.Unchanged;
        }
        public IState.E_State Update(PlayerData parent)
        {
            //�`���[�W���Ă銴���̃A�j���[�V�����Ƃ��͂���

            return IState.E_State.Unchanged;
        }
        public IState.E_State FixedUpdate(PlayerData parent)
        {
            parent.PlayerMove(parent._cameraForward);

            //�H�ׂ�ҋ@���I�������H�ׂ�X�e�[�g�Ɉڍs
            if (parent._playerStayEat.CanEat()) return IState.E_State.EatDango;

            //�ҋ@����߂���R���g���[���ɖ߂�
            if (!parent._hasStayedEat) return IState.E_State.Control;

            return IState.E_State.Unchanged;
        }
    }
    class EatDangoState : IState
    {
        public IState.E_State Initialize(PlayerData parent)
        {
            if (parent._canGrowStab) return IState.E_State.GrowStab;

            parent.EatDango();
            return IState.E_State.Control;
        }
        public IState.E_State Update(PlayerData parent)
        {
            return IState.E_State.Unchanged;
        }
        public IState.E_State FixedUpdate(PlayerData parent)
        {
            parent.PlayerMove(parent._cameraForward);

            return IState.E_State.Unchanged;
        }

    }
    class GrowStabState : IState
    {
        public IState.E_State Initialize(PlayerData parent)
        {
            parent._maxStabCount = parent._playerGrowStab.GrowStab(parent._maxStabCount);
            parent._playerUIManager.SetEventText("������c�q�̐����������I(" + parent._maxStabCount + "��)");
            parent._canGrowStab = false;
            //�h����͈͕\���̊g��B�������L�тȂ��̂ŃR�����g�A�E�g���Ă܂��B
            //parent.rangeUI.transform.localScale = new Vector3(parent.rangeUI.transform.localScale.x, parent.rangeUI.transform.localScale.y + 0.01f, parent.rangeUI.transform.localScale.z);
            return IState.E_State.EatDango;
        }
        public IState.E_State Update(PlayerData parent)
        {
            return IState.E_State.Unchanged;
        }
        public IState.E_State FixedUpdate(PlayerData parent)
        {
            return IState.E_State.Unchanged;
        }

    }

    private void InitState()
    {
        Logger.Assert(_currentState is >= 0 and < IState.E_State.Max);

        var nextState = states[(int)_currentState].Initialize(this);

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
    private void FixedUpdateState()
    {
        Logger.Assert(_currentState is >= 0 and < IState.E_State.Max);

        var nextState = states[(int)_currentState].FixedUpdate(this);

        if (nextState != IState.E_State.Unchanged)
        {
            //���ɑJ��
            _currentState = nextState;
            InitState();
        }
    }
    #endregion

    #region �����o�ϐ�
    //�v���C���[�̔\��
    [SerializeField] private float _moveSpeed = 50f;
    [SerializeField] private float _jumpPower = 10f;

    [SerializeField] private SpitManager spitManager = default!;
    [SerializeField] private GameObject makerUI = default!;
    [SerializeField] private GameObject rangeUI = default;

    //UI�֘A
    RoleDirectingScript _directing;
    PlayerUIManager _playerUIManager;
    DangoUIScript _dangoUISC;

    //����L�΂�����
    const int MAX_DANGO = 7;
    const int GROW_STAB_FRAME = 500;
    PlayerGrowStab _playerGrowStab = new(MAX_DANGO, GROW_STAB_FRAME);
    bool _canGrowStab = false;

    //�H�ׂ鏈��
    const int STAY_FRAME = 100;
    PlayerStayEat _playerStayEat = new(STAY_FRAME);

    /// <summary>
    /// �����x�A�������Ԃ̑���i�P��:[sec]�j
    /// </summary>
    /// �t���[�����ŊǗ����܂����A�����ł͕b�Ǘ��ō\���܂���B
    private float _satiety = 100f;

    /// <summary>
    /// ���A�����Ă�c�q
    /// </summary>
    /// ���܂ł�new List<DangoColor>()�Ƃ��Ȃ���΂Ȃ�܂���ł�����
    /// C#9.0�ȍ~�͂��̂悤�Ɋȑf���o���邻���ł��B
    private List<DangoColor> _dangos = new();

    /// <summary>
    /// �h���鐔�A���X�ɑ�����
    /// </summary>    
    private int _maxStabCount = 3;

    private bool _isGround = false;

    public bool IsGround
    {
        get => _isGround;
        private set
        {
            if (value)
            {
                _playerFall.IsFallAction = false;
            }

            _isGround = value;
        }
    }

    //�ړ����x�萔
    const float MAX_VELOCITY_MAG = 8f;
    const float RUN_SPEED_MAG = 7f;

    public Vector3 MoveVec { get; private set; }
    private Vector3 _cameraForward;

    public void SetCameraForward(Vector3 camForward) => _cameraForward = camForward;
    #endregion

    private void Awake()
    {
        _playerUIManager = GameObject.Find("PlayerUICanvas").GetComponent<PlayerUIManager>();
        _dangoUISC = GameObject.Find("PlayerUICanvas").transform.Find("DangoBackScreen").GetComponent<DangoUIScript>();
        _directing = GameObject.Find("PlayerUICanvas").transform.Find("DirectingObj").GetComponent<RoleDirectingScript>();
    }

    private void OnEnable()
    {
        InitDangos();
    }

    private void Start()
    {
        makerUI.SetActive(false);
    }

    private void Update()
    {
        IsGrounded();
        UpdateState();
        FallActionMaker();
        RangeUI();
    }

    private void FixedUpdate()
    {
        DecreaseSatiety();
        _canGrowStab = _playerGrowStab.CanGrowStab(_maxStabCount);
        FixedUpdateState();
        if (_hasJumped)
        {
            rb.AddForce(Vector3.up * (_jumpPower + _maxStabCount), ForceMode.Impulse);
            _hasJumped = false;
        }
    }

    //�f�o�b�O�I���ɍ폜
    private void OnGUI()
    {
        GUI.Label(new Rect(20, 20, 100, 50), "" + _currentState);
    }

    private void InitDangos()
    {
        if (_dangos == null) return;

        //������
        _dangos.Clear();
    }

    private void FallActionMaker()
    {
        var ray = new Ray(transform.position, Vector3.down);

            if (Physics.Raycast(ray, out RaycastHit hit,Mathf.Infinity))
        {
            makerUI.transform.position = hit.point + new Vector3(0,0.01f,0);
            //�˂��h���ł���悤�ɂȂ�����L����
            if (!Physics.Raycast(ray, capsuleCollider.height + capsuleCollider.height / 2f))
                makerUI.SetActive(true);
            else
                makerUI.SetActive(false);
        }
        
    }

    private void RangeUI()
    {
        var ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            rangeUI.transform.position = new Vector3(rangeUI.transform.position.x, hit.point.y + 0.01f, rangeUI.transform.position.z);
        }

        //�󒆂ł��o�Ă�ƈ�a���������̂ŏ����܂�
        if (_isGround)
        {
            rangeUI.SetActive(true);
        }
        else
        {
            rangeUI.SetActive(false);
        }
    }

    private bool CanStab()
    {
        //�c�q������ȏコ���Ȃ��Ȃ���s���Ȃ�
        if (_dangos.Count >= _maxStabCount)
        {
            Logger.Warn("�˂��h���鐔�𒴂��Ă��܂�");
            _playerUIManager.SetEventText("����ȏコ���Ȃ���I");

            return false;
        }

        //�h�����Ԃ̓����ŃJ�E���g���Ă���Time�����Z�b�g
        _playerAttack.ResetTime();

        //�˂��h�����Ԃɂ���
        spitManager.isSticking = true;

        //���̈ʒu��ύX�i�A�j���[�V���������j
        if (_playerFall.IsFallAction)
        {
            spitManager.gameObject.transform.localPosition = new Vector3(0, -2f, 0);
            spitManager.gameObject.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            spitManager.gameObject.transform.localPosition = new Vector3(0, 0, 2.2f);
            spitManager.gameObject.transform.localRotation = Quaternion.Euler(90f, 0, 0);
        }

        return true;
    }

    private void IsGrounded()
    {
        var ray = new Ray(transform.position, Vector3.down);
        IsGround = Physics.Raycast(ray, capsuleCollider.height / 1.999f);
    }

    /// <summary>
    /// Player���J�����̕����ɍ��킹�ē������֐��B
    /// </summary>
    private void PlayerMove(Vector3 camForward)
    {
        //�J�����̌��������Ƀx�N�g���̍쐬
        MoveVec = _moveAxis.y * _moveSpeed * _cameraForward + _moveAxis.x * _moveSpeed * playerCamera.transform.right;

        if (rb.velocity.magnitude < MAX_VELOCITY_MAG)
        {
            float mag = Mathf.Sqrt(rb.velocity.x * rb.velocity.x + rb.velocity.z * rb.velocity.z);
            float speedMag = RUN_SPEED_MAG - mag;
            rb.AddForce(MoveVec * speedMag);
        }
    }

    /// <summary>
    /// �����x���ւ炷�֐��AfixedUpdate�ɔz�u�B
    /// </summary>
    private void DecreaseSatiety()
    {
        //�����x��0.02�b(fixedUpdate�̌Ă΂��b��)���炷
        _satiety -= Time.fixedDeltaTime;
    }

    #region GetterSetter
    public Vector2 GetRoteAxis() => _roteAxis;
    public List<DangoColor> GetDangoType() => _dangos;
    public DangoColor GetDangoType(int value)
    {
        try
        {
            return _dangos[value];
        }
        catch (IndexOutOfRangeException e)
        {
            Logger.Error(e);
            Logger.Error("����ɐ擪�i�z���0�ԁj��Ԃ��܂��B");
            return _dangos[0];
        }
    }
    public void PlayerRotate(Quaternion rot)
    {
        if (_currentState != IState.E_State.Control) return;

        transform.rotation = rot;
    }

    public int GetMaxDango() => _maxStabCount;
    public List<DangoColor> GetDangos() => _dangos;
    public void AddDangos(DangoColor d) => _dangos.Add(d);
    public float GetSatiety() => _satiety;
    public DangoUIScript GetDangoUIScript() => _dangoUISC;

    #endregion
}