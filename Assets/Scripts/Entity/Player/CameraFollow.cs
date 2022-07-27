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

    //�ǂɂԂ������Ƃ��̂���
    Vector3 _wallHitPos;//�ǂɂԂ������ۂ̍��W
    RaycastHit _hit;//�ǂ���������Ray

    //�J�����Î~���ɍs������
    CameraIsStaying _camIsStaying = null;

    [SerializeField] State state;

    Camera _cam;
    [SerializeField] GameObject eatCamPos;

    const float DEFAULT_CAMERA_VIEW = 60f;
    const float CAMERA_REMOVETIME = 0.3f;

    Vector3 _firstCameraPos;
    Quaternion _firstCameraRot;

    #endregion

    private void Start()
    {
        _playerData = target.GetComponent<PlayerData>();
        transform.parent = null;//�������̂ɏ��Ƃ���ɒǏ]����������e�q�֌W�𖳂���
        _terminus = new GameObject("cameraTermiusObject");
        _terminus.transform.position = transform.position;
        _camIsStaying = new(gameObject, _terminus, target);
        _cam = GetComponent<Camera>();

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
                _ => Vector3.Lerp(transform.position, _terminus.transform.position, Time.deltaTime * ratio),
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

    public void OnChargeCameraMoving()
    {
        _cam.fieldOfView -= 10f * Time.deltaTime;
    }

    public IEnumerator ResetCameraView()
    {
        float view = _cam.fieldOfView;
        float hokann = DEFAULT_CAMERA_VIEW - view;
        while (_cam.fieldOfView <= DEFAULT_CAMERA_VIEW)
        {
            _cam.fieldOfView += (hokann / CAMERA_REMOVETIME) * Time.deltaTime;
            yield return null;
        }
        _cam.fieldOfView = DEFAULT_CAMERA_VIEW;
    }

    public void EatStateCamera()
    {
        //�ړ��J�n�O�̏����ʒu��ۑ�����
        _firstCameraPos = transform.position;
        _firstCameraRot = transform.rotation;

        //player to camera vec
        Vector3 pToC = transform.position - _playerData.transform.position;

        //player to target vec
        Vector3 pToT = eatCamPos.transform.position - _playerData.transform.position;

        //�J�����̏u�Ԉړ�
        transform.position = eatCamPos.transform.position;
        transform.rotation = Quaternion.Euler(new(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + RotateAngle(pToC, pToT), transform.rotation.eulerAngles.z));
        _terminus.transform.position = eatCamPos.transform.position;
        _terminus.transform.rotation = Quaternion.Euler(new(_terminus.transform.rotation.eulerAngles.x, _terminus.transform.rotation.eulerAngles.y + RotateAngle(pToC, pToT), _terminus.transform.rotation.eulerAngles.z));

        //�J���������Ƃɖ߂������i�C���K�v�j
        Invoke("RemoveCamera", 0.3f);
    }

    private float RotateAngle(Vector3 from, Vector3 to)
    {
        //�@��N
        Vector3 n = Vector3.up;

        Vector3 planeFrom = Vector3.ProjectOnPlane(from, n);
        Vector3 planeTo = Vector3.ProjectOnPlane(to, n);

        return Vector3.SignedAngle(planeFrom, planeTo, n);
    }

    private void RemoveCamera()
    {
        transform.position = _firstCameraPos;
        transform.rotation = _firstCameraRot;
    }
}