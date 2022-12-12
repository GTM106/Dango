using Cinemachine;
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

    [SerializeField] Vector3 EatCameraPos;

    [SerializeField] GameObject parent;
    private Vector3 _prebTargetPos = Vector3.zero;
    private GameObject _terminus = null;

    private PlayerData _playerData;

    private float _roteYSpeed = -100f;

    private Vector3 rayStartPos;

    private bool Event = false;

    [SerializeField] CinemachineVirtualCamera _cinemachineVirtualCamera;


    Vector3 _wallHitPos;//�ǂɂԂ������ۂ̍��W
    RaycastHit _hit;//�ǂ���������Ray

    CameraIsStaying _camIsStaying = null;

    [SerializeField] State state;

    #endregion

    private void Start()
    {
        _playerData = target.GetComponent<PlayerData>();
        transform.parent = parent.transform;//�������̂ɏ��Ƃ���ɒǏ]����������e�q�֌W�𖳂���
        _terminus = new GameObject("cameraTermiusObject");
        _terminus.transform.parent = parent.transform;
        _terminus.transform.position = _cinemachineVirtualCamera.transform.position;
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
    }

    private void CameraSmoothMove()
    {
        Vector3 currentTargetPos = target.position;

        //�J�����̖ڕW�n�_��ύX����
        _terminus.transform.position += currentTargetPos - _prebTargetPos;
        if (!Event)
        {
            if (WallHitCheck())
            {
                ////���������ꏊ�ɔ�΂��ƃJ�������ǂ̒��ɖ��܂�̂Œ����B
                //_wallHitPos = _hit.point + (currentTargetPos - _terminus.transform.position).normalized;
                //transform.position = _wallHitPos;

                _cinemachineVirtualCamera.transform.position = state switch
                {
                    State.normal => _terminus.transform.position,
                    _ => Vector3.Lerp(_cinemachineVirtualCamera.transform.position, _terminus.transform.position, Time.deltaTime * ratio),
                };
            }
            else//�J�����̈ړ�
            {
                _cinemachineVirtualCamera.transform.position = state switch
                {
                    State.normal => _terminus.transform.position,
                    _ => Vector3.Lerp(_cinemachineVirtualCamera.transform.position, _terminus.transform.position, Time.deltaTime * ratio),
                };
            }
        }
        _prebTargetPos = currentTargetPos;
    }

    private void RotateToLookRot()
    {
        if (!Event)
        {
            if (InputSystemManager.Instance.LookAxis.magnitude > 0.1f || _playerData.Rb.velocity.magnitude > 0.1f)
            {
                _camIsStaying.Reset();
                return;
            }
        }
        _camIsStaying.Update();
    }

    private void CameraRotate()
    {
        //X����]�̊p�x������
        float currentYAngle = _cinemachineVirtualCamera.transform.eulerAngles.x;

        //X����0�`360�̒l�����Ԃ��Ȃ��̂Œ���
        if (currentYAngle > 180)
        {
            currentYAngle -= 360;
        }

        switch (state)
        {
            case State.normal:
                Rote(_terminus, currentYAngle);
                _cinemachineVirtualCamera.transform.rotation = _terminus.transform.rotation;
                break;
            case State.Lerp:
                Rote(_terminus, currentYAngle);
                _cinemachineVirtualCamera.transform.rotation = _terminus.transform.rotation;
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
        obj.transform.RotateAround(target.position, Vector3.up, InputSystemManager.Instance.LookAxis.x * (DataManager.configData.cameraVerticalOrientation ? -1 : 1) * DataManager.configData.cameraRotationSpeed / 100f * Time.deltaTime);

        //�c���̐���
        if ((a >= MinAngle && InputSystemManager.Instance.LookAxis.y > 0) || (a <= MaxAngle && InputSystemManager.Instance.LookAxis.y < 0))
        {
            obj.transform.RotateAround(target.position, obj.transform.right, InputSystemManager.Instance.LookAxis.y * (DataManager.configData.cameraRotationSpeed / 100f) * Time.deltaTime * _roteYSpeed);
        }
    }

    private bool WallHitCheck()
    {
        rayStartPos = target.position + new Vector3(0, 0.005f, 0);
        return Physics.Raycast(rayStartPos, _terminus.transform.position - target.position, out _hit, Vector3.Distance(_prebTargetPos, _terminus.transform.position), wallLayer, QueryTriggerInteraction.Ignore);
    }

}

