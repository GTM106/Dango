using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraIsStaying
{
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

            return parent.InitControl() ? IState.E_State.Unchanged : IState.E_State.Stay;
        }
        public IState.E_State Update(CameraIsStaying parent)
        {
            return parent.LookPlayerBack() ? IState.E_State.Stay : IState.E_State.Unchanged;
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

        Logger.Log(_currentState);

        if (nextState != IState.E_State.Unchanged)
        {
            //���ɑJ��
            _currentState = nextState;
            InitState();
        }
    }


    //1s�Ɉړ�����p�x�̒萔
    const float MOVE_ANGLE = 45f;

    //�ҋ@�b���萔
    const float MAX_STAY_TIME = 2f;

    float _currentTime = MAX_STAY_TIME;

    GameObject _playerCamera;
    GameObject _terminus;
    Transform _playerTrans;

    Vector3 _playerPos;
    Vector3 _cameraPos;
    Vector3 _targetPos;
    float _distancePtoC;
    float _distanceCtoT;
    float _angle;
    bool _isLeft;

    public CameraIsStaying(GameObject playerCamera, GameObject terminus, Transform playerTrans)
    {
        _playerCamera = playerCamera;
        _terminus = terminus;
        _playerTrans = playerTrans;
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

    private bool IsStaying()
    {
        _currentTime -= Time.deltaTime;

        return _currentTime > 0;
    }

    private bool InitControl()
    {
        //Y���W�𖳎������v���C���[�̍��W
        _playerPos = new(_playerTrans.position.x, _playerCamera.transform.position.y, _playerTrans.position.z);

        //�J�������W
        _cameraPos = _playerCamera.transform.position;

        //playerPos�ƃJ�������W�̋����i�x�N�g���̑傫���j
        _distancePtoC = Vector3.Distance(_cameraPos, _playerPos);

        //Player�̍����ɃJ���������邩�O�ς�y������Ĕ���
        _isLeft = Vector3.Cross(_playerTrans.forward, _cameraPos - _playerPos).y < 0 ? true : false;

        //���ʂ̋t�x�N�g���ɋ����������ăv���C���[�̍��W�iY�����j�𑫂�������
        //���ꂪ�ڕW�n�_
        _targetPos = _playerPos + (-_playerTrans.forward) * _distancePtoC;

        //�]���藝�Ŋp�x�����߂�
        _distanceCtoT = Vector3.Distance(_targetPos, _cameraPos);
        float angle = Mathf.Acos((_distancePtoC * _distancePtoC + _distancePtoC * _distancePtoC - _distanceCtoT * _distanceCtoT) / (2 * _distancePtoC * _distancePtoC));

        //�ʓx�@����x���@�ɕϊ�
        _angle = angle * 180f / Mathf.PI;

        //�[�����Z�P�A�i�����������B���Ԃ�0�Ȃ�ړ����Ȃ��Ƃ������Ɓj
        if (_angle == 0) return false;

        return true;
    }

    private bool LookPlayerBack()
    {
        //�p�x��萔�Ŋ���B�萔�l��1�Ƃ����Ƃ����b�ŖڕW�n�_�ɓ��B���邩���Ęb
        float arrivalTime = _angle / MOVE_ANGLE;
        _currentTime += Time.deltaTime;

        //���݂̌o�ߎ��Ԃ𑫂�
        //�o�ߎ��Ԃ�0-1�ŕ\����������A���������߂����b�œ��B���邩�Ŋ���
        float x = _currentTime;

        x /= arrivalTime;

        float y = 0;
        if (x < 0.5f)
        {
            y = 4f * x * x * x;
        }
        else if (x is >= 0.5f and <= 1f)
        {
            x = Mathf.Abs(x - 1);
            y = 4f * x * x * x;
        }

        _playerCamera.transform.RotateAround(_playerTrans.position, Vector3.up, (_isLeft ? -y : y) * 360f * Time.deltaTime);
        _terminus.transform.RotateAround(_playerTrans.position, Vector3.up, (_isLeft ? -y : y) * 360f * Time.deltaTime);
        //_playerCamera.transform.RotateAround(_playerTrans.position, Vector3.up, (_isLeft ? -MOVE_ANGLE : MOVE_ANGLE) * Time.deltaTime);
        //_terminus.transform.RotateAround(_playerTrans.position, Vector3.up, (_isLeft ? -MOVE_ANGLE : MOVE_ANGLE) * Time.deltaTime);

        return _currentTime >= arrivalTime;
    }
}
