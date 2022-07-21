using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    enum State
    {
        normal,//�v���C���[�̌�����O�Ȃ����Ă����܂�
        Lerp,//�v���C���[�̉�]�A�ړ��Ɋ��炩�ɂ��Ă����܂�
        LerpMove,//�v���C���[�̈ړ��݂̂Ɋ��炩�ɂ��Ă����܂�
    }

    #region �����o
    [SerializeField, Tooltip("�Ǐ]�������^�[�Q�b�g")] Transform target = default!;
    [SerializeField] float MinAngle;
    [SerializeField] float MaxAngle;
    [SerializeField, Tooltip("�ǂ̂��炢�̎��ԂŒǂ�����"), Min(0)] float ratio = 1;

    //[SerializeField, Range(0.01f, 1f), Tooltip("�J�����̒Ǐ]�x")]
    //private float smoothSpeed = 0.125f;

    [SerializeField] LayerMask wallLayer;//�}�b�v�̃��C���[�}�X�N

    private Vector3 _prebTargetPos = Vector3.zero;
    private GameObject _terminus = null;

    private PlayerData _playerData;

    private float _roteYSpeed = -100f;


    Vector3 _wallHitPos;//�ǂɂԂ������ۂ̍��W
    RaycastHit _hit;//�ǂ���������Ray

    CameraIsStaying _camIsStaying = null;

    [SerializeField] State state;

#endregion

    private void Start()
    {
        _playerData = target.GetComponent<PlayerData>();
        transform.parent = null;//�������̂ɏ��Ƃ���ɒǏ]����������e�q�֌W�𖳂���
        _terminus = new GameObject("cameraTermiusObject");
        _terminus.transform.position = transform.position;
        _camIsStaying = new(gameObject, _terminus, target);

        _prebTargetPos = target.position;
    }

    private void Update()
    {
        CameraRotate();
        RotateToLookRot();
    }

    //Player�����������ƂɎ��s���邽�߁ALateUpdate�ōs���B
    private void LateUpdate()
    {
        CameraSmoothMove();

        _playerData.SetCameraForward(transform.forward);
    }

    private void CameraSmoothMove()
    {
        Vector3 currentTargetPos = target.position;

        //�J�����̖ڕW�n�_��ύX����
        _terminus.transform.position += currentTargetPos - _prebTargetPos;

        if (WallHitCheck())
        {
            //���������ꏊ�ɔ�΂��ƃJ�������ǂ̒��ɖ��܂�̂Œ����B
            _wallHitPos = _hit.point + (currentTargetPos - _terminus.transform.position).normalized;
            transform.position = _wallHitPos;
        }
        else//�J�����̈ړ�
        {
            transform.position = state switch
            {
                State.normal => _terminus.transform.position,
                _ => Vector3.Lerp(transform.position, _terminus.transform.position, Time.deltaTime*ratio),
            };
        }

        _prebTargetPos = currentTargetPos;
    }
    private void RotateToLookRot()
    {
        if (_playerData.GetRoteAxis().magnitude > 0.1f || _playerData.GetMoveAxis().magnitude > 0.1f)
        {
           _camIsStaying.Reset();
            return;
        }

        _camIsStaying.Update();
    }

    private void CameraRotate()
    {
        //X����]�̊p�x������
        float currentYAngle = transform.eulerAngles.x;

        //X����0�`360�̒l�����Ԃ��Ȃ��̂Œ���
        if (currentYAngle > 180)
        {
            currentYAngle -= 360;
        }

        switch (state)
        {
            case State.normal:
                Rote(_terminus, currentYAngle);
                transform.rotation = _terminus.transform.rotation;
                break;
            case State.Lerp:
                Rote(_terminus, currentYAngle);
                transform.rotation = _terminus.transform.rotation;
                break;
            case State.LerpMove:
                Rote(gameObject, currentYAngle);
                Rote(_terminus, currentYAngle);
                break;
        }

    }

    private void Rote(GameObject obj, float a)
    {
        //�J������roteAxis.x�ɍ��킹�ĉ�]������B
        obj.transform.RotateAround(target.position, Vector3.up, _playerData.GetRoteAxis().x * Time.deltaTime);

        //�c���̐���
        if ((a >= MinAngle && _playerData.GetRoteAxis().y > 0) || (a <= MaxAngle && _playerData.GetRoteAxis().y < 0))
        {
            obj.transform.RotateAround(target.position, obj.transform.right, _playerData.GetRoteAxis().y * Time.deltaTime * _roteYSpeed);
        }

    }

    private bool WallHitCheck()
    {
        return Physics.Raycast(target.position, _terminus.transform.position - target.position, out _hit, Vector3.Distance(_prebTargetPos, _terminus.transform.position), wallLayer, QueryTriggerInteraction.Ignore);
    }
}

