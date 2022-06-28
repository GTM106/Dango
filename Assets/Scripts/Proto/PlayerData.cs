using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody), typeof(RideMoveObj))]
public class PlayerData : MonoBehaviour
{
    #region inputSystem
    //�����A�N�V�����萔
    const int FALLACTION_STAY_AIR_FRAME = 50;
    const int FALLACTION_FALL_POWER = 30;
    const int FALLACTION_MOVE_POWER = 10;

    //�X�R�A�Ɩ����x�̃��[�g
    const float SCORE_TIME_RATE = 0.2f;

    //�����A�N�V��������
    private bool _isFallAction;
    public bool IsFallAction
    {
        get => _isFallAction;
        private set
        {
            if (value != IsFallAction)
            {
                if (!value) ResetSpit();
                _isFallAction = value;
            }
        }
    }

    //���͒l
    private Vector2 _moveAxis;
    private Vector2 _roteAxis;

    [SerializeField] private Rigidbody _rigidbody;

    [SerializeField] private GameObject playerCamera;

    private RoleDirectingScript directing;

    private DangoRole dangoRole = DangoRole.instance;

    //�ړ�����
    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            _moveAxis = context.ReadValue<Vector2>().normalized;
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            _moveAxis = Vector2.zero;
        }
    }

    //�W�����v����
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            if (IsGround)
            {
                _rigidbody.AddForce(Vector3.up * (_jumpPower + _Maxdango), ForceMode.Impulse);
            }
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

            //���������B
            _dangos.RemoveAt(_dangos.Count - 1);
            DangoUISC.DangoUISet(_dangos);
        }
    }

    //�˂��h���A�j���[�V����
    public void OnAttack(InputAction.CallbackContext context)
    {
        //�����A�N�V�������󂯕t���Ȃ��B
        if (IsFallAction) return;

        if (context.phase == InputActionPhase.Performed)
        {
            if (FallAction()) return;

            //�˂��h���鐔�𒴂��Ă����ꍇ�A���s���Ȃ�
            if (_dangos.Count >= _Maxdango)
            {
                //�Ȃ�炩�̓˂��Ȃ����Ƃ�m�点�鏈�������B

                Logger.Warn("�˂��h���鐔�𒴂��Ă��܂�");
                _eventText.text = "����ȏコ���Ȃ���I";
                return;
            }

            //�����ɓ˂��h���A�j���[�V�����𐄏��B
            spitManager.isSticking = true;
            spitManager.gameObject.transform.localPosition = new Vector3(0, 0, 2.2f);
            spitManager.gameObject.transform.localRotation = Quaternion.Euler(90f, 0, 0);

        }
        if (context.phase == InputActionPhase.Canceled)
        {
            ResetSpit();
        }
    }

    private void ResetSpit()
    {
        spitManager.isSticking = false;
        spitManager.gameObject.transform.localRotation = Quaternion.identity;
        spitManager.gameObject.transform.localPosition = new Vector3(0, 0.4f, 1.1f);
    }

    //�H�ׂ�
    public void OnEatDango(InputAction.CallbackContext context)
    {
        //���Ɏh�����ĂȂ���������s���Ȃ��B
        if (_dangos.Count == 0) return;

        switch (context.phase)
        {
            case InputActionPhase.Started:
                Logger.Log("�H�׃`���[�W�J�n�I");
                _eventText.text = "�H�׃`���[�W���I";
                //SE����

                break;
            case InputActionPhase.Performed:
                Logger.Log("�H�ׂ��I�I");
                //SE����

                //�H�ׂ��c�q�̓_�����擾
                var score = dangoRole.CheckRole(_dangos);

                //���o�֐��̌Ăяo��
                _directing.Dirrecting(_dangos);

                _eventText.text = "�H�ׂ��I" + (int)score + "�_�I";

                //�����x���㏸
                _satiety += score * SCORE_TIME_RATE;

                //�X�R�A���㏸
                GameManager.GameScore += score * 100f;

                //�����N���A�B
                _dangos.Clear();

                //UI�X�V
                DangoUISC.DangoUISet(_dangos);
                break;
        }
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

    /// <summary>
    /// �h�˃A�N�V����
    /// </summary>
    /// <returns>�\���ǂ���</returns>
    private bool FallAction()
    {
        //�ڒn���Ă��� �܂��� �����A�N�V�������Ȃ���s���Ȃ�
        if (IsGround || IsFallAction) return false;

        //�}�b�N�X�܂Ŏh���ĂȂ�������}�~���˂��h�����[�V�����Ɉڍs
        if (_dangos.Count < _Maxdango)
        {
            spitManager.isSticking = true;
            spitManager.gameObject.transform.localPosition = new Vector3(0, -2f, 0);
            spitManager.gameObject.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }

        StartCoroutine(StayAir());

        return true;
    }

    private IEnumerator StayAir()
    {
        IsFallAction = true;
        int time = FALLACTION_STAY_AIR_FRAME;

        while (--time > 0)
        {
            yield return new WaitForFixedUpdate();

            //�؋󏈗�
            _rigidbody.velocity = new Vector3(_rigidbody.velocity.x / FALLACTION_MOVE_POWER, 0, _rigidbody.velocity.z / FALLACTION_MOVE_POWER);
        }

        _rigidbody.AddForce(Vector3.down * FALLACTION_FALL_POWER, ForceMode.Impulse);
    }

    #endregion

    //�v���C���[�̔\��
    [SerializeField] private float _moveSpeed = 3f;
    [SerializeField] private float _jumpPower = 10f;

    [SerializeField] private SpitManager spitManager = default!;
    [SerializeField] private DangoUIScript DangoUISC = default!;
    [SerializeField] private GameObject makerPrefab = default!;
    GameObject _maker = null;
    RoleDirectingScript _directing;

    //��UI�p���M��
    private TextMeshProUGUI _timeText;
    private TextMeshProUGUI _eventText;

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
    private int _Maxdango = 3;

    private float time = 0;

    private bool _isGround = false;

    public bool IsGround
    {
        get => _isGround;
        private set
        {
            if (value)
            {
                IsFallAction = false;
                _maker.SetActive(false);
            }

            _isGround = value;
        }
    }


    public Vector3 MoveVec { get; private set; }

    private void OnEnable()
    {
        InitDangos();
    }

    private void Start()
    {
        if (DangoUISC == null)
        {
            DangoUISC = GameObject.Find("Canvas").transform.Find("DangoBackScreen").GetComponent<DangoUIScript>();
        }

        if (_timeText == null)
        {
            _timeText = GameObject.Find("Canvas").transform.Find("Time").GetComponent<TextMeshProUGUI>();
        }

        if (_eventText == null)
        {
            _eventText = GameObject.Find("Canvas").transform.Find("Event").GetComponent<TextMeshProUGUI>();
        }

        _directing = GameObject.Find("Canvas").transform.Find("DirectingObj").GetComponent<RoleDirectingScript>();

        _maker = Instantiate(makerPrefab);
        _maker.SetActive(false);
    }

    private void Update()
    {
        IsGrounded();
        FallActionMaker();
    }

    private void FixedUpdate()
    {
        PlayerMove();
        DecreaseSatiety();
        GrowStab();

        //���ł�����
        _timeText.text = "�c�莞�ԁF" + (int)_satiety + "�b";

    }

    private void InitDangos()
    {
        if (_dangos == null) return;

        //������
        _dangos.Clear();
    }

    private void FallActionMaker()
    {
        if (IsGround) return;
        var ray = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, 10f))
        {
            _maker.transform.position = hit.point;
            _maker.SetActive(true);
        }
    }

    private void IsGrounded()
    {
        var ray = new Ray(transform.position, Vector3.down);
        IsGround = Physics.Raycast(ray, 1f);
    }

    /// <summary>
    /// Player���J�����̕����ɍ��킹�ē������֐��B
    /// </summary>
    private void PlayerMove()
    {
        //�J�����̌������m�F�ACameraforward�ɑ��
        var Cameraforward = Vector3.Scale(playerCamera.transform.forward, new Vector3(1, 0, 1)).normalized;

        //�J�����̌��������Ƀx�N�g���̍쐬
        MoveVec = _moveAxis.y * Cameraforward * _moveSpeed + _moveAxis.x * playerCamera.transform.right * _moveSpeed;

        if (_rigidbody.velocity.magnitude < 8f)
            _rigidbody.AddForce(MoveVec * _moveSpeed);
    }

    /// <summary>
    /// �����x���ւ炷�֐��AfixedUpdate�ɔz�u�B
    /// </summary>
    private void DecreaseSatiety()
    {
        //�����x��0.02�b(fixedUpdate�̌Ă΂��b��)���炷
        _satiety -= Time.fixedDeltaTime;

        //�Q�[���}�l�[�W���[�Ǘ��̂ق��������Ǝv������
        //�Ƃ肠���������ɒu���Ă����܂��B
        FinishGame();

        //[debug]10�b�����Ƀf�o�b�O���O��\��
        //if ((int)_satiety % 10 == 0) Logger.Log(_satiety);
    }

    /// <summary>
    /// ������莞�ԂŐL�т鏈��
    /// </summary>
    private void GrowStab()
    {
        //�h����c�q�̐���7����������s���Ȃ��B
        if (_Maxdango == 7) return;

        float growTime = 10f;

        time += Time.fixedDeltaTime;

        if (time >= growTime)
        {
            _Maxdango++;
            Logger.Log("������c�q�̐����������I" + _Maxdango);
            _eventText.text = "������c�q�̐����������I(" + _Maxdango + "��)";
            time = 0;
        }
    }

    private void FinishGame()
    {
        int madeCount = 0;
        if (_satiety <= 0)
        {
            var posRoles = dangoRole.GetPosRoles();
            foreach (var posRole in posRoles)
            {
                if (posRole.GetMadeCount() > 0)
                {
                    madeCount++;
                }
            }
            Logger.Log("�����x�F" + GameManager.GameScore * madeCount);
        }
    }

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
    public int GetMaxDango() => _Maxdango;
    public List<DangoColor> GetDangos() => _dangos;
    public void AddDangos(DangoColor d) => _dangos.Add(d);

}
