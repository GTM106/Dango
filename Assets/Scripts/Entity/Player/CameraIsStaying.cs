using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraIsStaying
{
    #region StatePattern
    interface IState
    {
        public enum E_State
        {
            Control,
            Stay,

            Max,

            Unchanged,
        }
        E_State Initialize(CameraIsStaying parent);
        E_State Update(CameraIsStaying parent);
    }

    //��ԊǗ�
    private IState.E_State _currentState = IState.E_State.Stay;
    private static readonly IState[] states = new IState[(int)IState.E_State.Max]
    {
        new ControlState(),
        new StayState(),
    };

    class ControlState : IState
    {
        public IState.E_State Initialize(CameraIsStaying parent)
        {
            parent._currentTime = 0;
            parent._timeX = parent._timeY = 0;

            return (parent.InitControlX() && parent.InitControlY()) ? IState.E_State.Unchanged : IState.E_State.Stay;
        }
        public IState.E_State Update(CameraIsStaying parent)
        {
            parent.LookPlayerBackH();

            return parent.LookPlayerBackV() ? IState.E_State.Stay : IState.E_State.Unchanged;
        }
    }
    class StayState : IState
    {
        public IState.E_State Initialize(CameraIsStaying parent)
        {
            parent._currentTime = MAX_STAY_TIME;
            return IState.E_State.Unchanged;
        }
        public IState.E_State Update(CameraIsStaying parent)
        {
            return parent.IsStaying() ? IState.E_State.Unchanged : IState.E_State.Control;
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

    #endregion

    #region �����o
    static readonly Vector3 OFFSET = new(0, 2f, -10f);

    //1s�Ɉړ�����p�x�̒萔
    const float MOVE_ANGLE = 45f;

    //�ҋ@�b���萔
    const float MAX_STAY_TIME = 2f;

    float _currentTime = MAX_STAY_TIME;

    GameObject _playerCamera;
    GameObject _terminus;
    Transform _playerTrans;

    float _arrivalTime;
    float _arrivalX;
    float _arrivalY;

    float _angleX;
    float _angleY;
    bool _isLeft;
    bool _isUp;

    float _tAngle;

    float _timeX;
    float _timeY;
    #endregion

    public CameraIsStaying(GameObject playerCamera, GameObject terminus, Transform playerTrans)
    {
        _playerCamera = playerCamera;
        _terminus = terminus;
        _playerTrans = playerTrans;

        Vector3 v = new(0, 0, OFFSET.z);
        float targetAngle = Mathf.Acos(Vector3.Dot(v, OFFSET) / (v.magnitude * OFFSET.magnitude));
        _tAngle = targetAngle * 180f / Mathf.PI;
    }

    public void Update()
    {
        UpdateState();
    }

    public void Reset()
    {
        _currentTime = MAX_STAY_TIME;
        _currentState = IState.E_State.Stay;
    }

    public void Start(float duration)
    {
        _currentTime = 0;
        _currentState = IState.E_State.Control;
        _arrivalTime = duration;
    }

    private bool IsStaying()
    {
        _currentTime -= Time.deltaTime;

        return _currentTime > 0;
    }

    private bool InitControlX()
    {
        //Y���W�𖳎������v���C���[�̍��W
        Vector3 playerPos = new(_playerTrans.position.x, _playerCamera.transform.position.y, _playerTrans.position.z);

        //�J�������W
        Vector3 cameraPos = _playerCamera.transform.position;

        //playerPos�ƃJ�������W�̋����i�x�N�g���̑傫���j
        float distancePtoC = Vector3.Distance(cameraPos, playerPos);

        //Player->Camera�̃x�N�g��
        Vector3 pcVec = cameraPos - playerPos;

        //Player�̍����ɃJ���������邩�O�ς�y������Ĕ���
        _isLeft = Vector3.Cross(_playerTrans.forward, pcVec).y < 0;

        //���ʂ̋t�x�N�g���ɋ����������ăv���C���[�̍��W�iY�����j�𑫂�������
        //���ꂪ�ڕW�n�_
        Vector3 targetPos = playerPos + (-_playerTrans.forward) * distancePtoC;

        //�]���藝����p�x�����߂�
        float distanceCtoT = Vector3.Distance(targetPos, cameraPos);
        float angleX = Mathf.Acos((distancePtoC * distancePtoC + distancePtoC * distancePtoC - distanceCtoT * distanceCtoT) / (2 * distancePtoC * distancePtoC));

        //�ʓx�@����x���@�ɕϊ�
        _angleX = angleX * 180f / Mathf.PI;

        //�[�����Z�P�A�i�����������B���Ԃ�0�Ȃ�ړ����Ȃ��Ƃ������Ɓj
        if (_angleX == 0) return false;

        float aTime = _angleX / MOVE_ANGLE;
        _arrivalTime = Mathf.Max(aTime, _arrivalTime);
        _arrivalX = aTime / _arrivalTime;

        return true;
    }
    private bool InitControlY()
    {
        //�v���C���[�̍��W
        Vector3 playerPos = _playerTrans.position;

        //�J���������ɒn�ʂƕ��s�i�v���C���[�̊p�x�j�ȃx�N�g��
        Vector3 cameraVec = new(_playerCamera.transform.position.x, _playerTrans.position.y, _playerCamera.transform.position.z);

        //Player->Camera�̃x�N�g��
        Vector3 pcVec = _playerCamera.transform.position - playerPos;
        Vector3 pcVecAtGround = cameraVec - playerPos;

        //�ڕW�n�_�̏�ɃJ���������邩y������Ĕ���
        _isUp = OFFSET.y < _terminus.transform.position.y;

        float currentAngle = Mathf.Acos(Vector3.Dot(pcVec, pcVecAtGround) / (pcVec.magnitude * pcVecAtGround.magnitude));
        float cAngle = currentAngle * 180f / Mathf.PI;

        _angleY = (cAngle - _tAngle);

        //�[�����Z�P�A�i�����������B���Ԃ�0�Ȃ�ړ����Ȃ��Ƃ������Ɓj
        if (_angleY == 0) return false;

        float aTime = _angleY / MOVE_ANGLE;

        _arrivalTime = Mathf.Max(aTime, _arrivalTime);
        _arrivalY = aTime / _arrivalTime;

        return true;
    }

    private bool LookPlayerBackV()
    {
        //���݂̌o�ߎ��Ԃ𑫂�
        _timeY += Time.deltaTime;

        //�o�ߎ��Ԃ�0-1�ŕ\����������A���������߂����b�œ��B���邩�Ŋ���
        float x = _timeY;
        x /= _arrivalTime;

        EaseInOutCubic(x, out float y, out float dx);

        _playerCamera.transform.RotateAround(_playerTrans.position, _playerCamera.transform.right.normalized, (_isUp ? -y : y) * MOVE_ANGLE * _arrivalY / dx * Time.deltaTime);
        _terminus.transform.RotateAround(_playerTrans.position, _playerCamera.transform.right.normalized, (_isUp ? -y : y) * MOVE_ANGLE * _arrivalY / dx * Time.deltaTime);

        return _timeY >= _arrivalTime;
    }

    private bool LookPlayerBackH()
    {
        //���݂̌o�ߎ��Ԃ𑫂�
        _timeX += Time.deltaTime;

        //�o�ߎ��Ԃ�0-1�ŕ\����������A���������߂����b�œ��B���邩�Ŋ���
        float x = _timeX;
        x /= _arrivalTime;

        EaseInOutCubic(x, out float y, out float dx);

        _playerCamera.transform.RotateAround(_playerTrans.position, Vector3.up, (_isLeft ? -y : y) * MOVE_ANGLE * _arrivalX / dx * Time.deltaTime);
        _terminus.transform.RotateAround(_playerTrans.position, Vector3.up, (_isLeft ? -y : y) * MOVE_ANGLE * _arrivalX / dx * Time.deltaTime);

        return _timeX >= _arrivalTime;
    }

    private void EaseInOutCubic(float x, out float y, out float dx)
    {
        y = 0;

        if (x < 0.5f)
        {
            y = 4f * x * x * x;
        }
        else if (x is >= 0.5f and <= 1f)
        {
            x = Mathf.Abs(x - 1);
            y = 4f * x * x * x;
        }

        //0����1/2�Œ�ϕ������l�i��̊֐��̖ʐρj��2�{�������́����ꂪ���ς̑��x�ɂȂ�B�����ς̑��x��MOVE_ANGLE�ɂȂ�΂悢�B
        dx = (0.5f * 0.5f * 0.5f * 0.5f) * 2f;
    }
}
