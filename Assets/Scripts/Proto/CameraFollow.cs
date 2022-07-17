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
    [SerializeField, Tooltip("�Ǐ]�������^�[�Q�b�g")] Transform target = default!;
    [SerializeField] float MinAngle;
    [SerializeField] float MaxAngle;
    [SerializeField, Tooltip("�ǂ̂��炢�̎��ԂŒǂ�����")] float ratio = 1;

    [SerializeField, Range(0.01f, 1f), Tooltip("�J�����̒Ǐ]�x")]
    private float smoothSpeed = 0.125f;

    [SerializeField] GameObject TerminusObjct;

    [SerializeField] LayerMask wallLayer;//�}�b�v�̃��C���[�}�X�N

    //[SerializeField, Tooltip("�^�[�Q�b�g����̃J�����̈ʒu")]
    //private Vector3 offset = new Vector3(0f, 1f, -10f);

    //private Vector3 velocity = Vector3.zero;


    private Vector3 targetPos = Vector3.zero;
    private GameObject terminus;

    private PlayerData _playerData;

    private float roteYSpeed = -10f;

    Vector3 wallHitPos;//�ǂɂԂ������ۂ̍��W
    RaycastHit hit;//�ǂ���������Ray

    [SerializeField] State state;
    private void Start()
    {
        _playerData = target.GetComponent<PlayerData>();
        transform.parent = null;//�������̂ɏ��Ƃ���ɒǏ]����������e�q�֌W�𖳂���
        terminus = Instantiate(TerminusObjct);
        terminus.transform.position = transform.position;
    }

    //Player�����������ƂɎ��s���邽�߁ALateUpdate�ōs���B
    private void LateUpdate()
    {

        _playerData = target.GetComponent<PlayerData>();

        //�J�����̈ʒu��ύX����
        if(target.position - targetPos != target.position)
        terminus.transform.position += target.position - targetPos;

        //X����]�̊p�x������
        float currentYAngle = transform.eulerAngles.x;

        //X����0�`360�̒l�����Ԃ��Ȃ��̂Œ���
        if (currentYAngle > 180)
        {
            currentYAngle -= 360;
        }

        if (WallHitCheck())
        {
            //switch (state)
            //{

            //    case State.normal:
                    transform.position = wallHitPos;//Lerp�����g���ƈړ��̍ۂɕǂ̒��������邽�ߌŒ�
            //        break;
            //    case State.Lerp:
            //        transform.position = Vector3.Lerp(transform.position, wallHitPos, ratio*Time.deltaTime);
            //        break;
            //    case State.LerpMove:
            //        transform.position = Vector3.Lerp(transform.position, wallHitPos, ratio * Time.deltaTime);
            //        break;
            //}
        }
        else//�J�����̈ړ�
        {
            switch(state)
            {
                case State.normal:
                    transform.position = terminus.transform.position;
                    break;
                case State.Lerp:
                    transform.position = Vector3.Lerp(transform.position, terminus.transform.position, ratio * Time.deltaTime);
                    break;
                case State.LerpMove:
                    transform.position = Vector3.Lerp(transform.position, terminus.transform.position, ratio * Time.deltaTime);
                    break;
            }
        }

        switch (state)
        {

            case State.normal:
                Rote(terminus, currentYAngle);
                //transform.position = terminus.transform.position;
                transform.rotation = terminus.transform.rotation;
                break;
            case State.Lerp:
                Rote(terminus, currentYAngle);
                transform.rotation = terminus.transform.rotation;
                //transform.position = Vector3.Lerp(transform.position, terminus.transform.position, ratio);
                break;
            case State.LerpMove:
                //transform.position = Vector3.Lerp(transform.position, terminus.transform.position, ratio);
                Rote(gameObject, currentYAngle);
                Rote(terminus, currentYAngle);
                break;
        }

        //�J�����̈ʒu�����肵�Ă���v���C���[�̌��������߂邱�ƂŁA�J�N�����Ȃ����B
        target.transform.rotation = Quaternion.Euler(0, transform.localEulerAngles.y, 0);
        targetPos = target.position;

    }
    private void Rote(GameObject obj, float a)
    {
        //�J������roteAxis.x�ɍ��킹�ĉ�]������B
        obj.transform.RotateAround(target.position, Vector3.up, _playerData.GetRoteAxis().x * Time.deltaTime);

        //�c���̐���
        if ((a >= MinAngle && _playerData.GetRoteAxis().y > 0) || (a <= MaxAngle && _playerData.GetRoteAxis().y < 0))
        {
            obj.transform.RotateAround(target.position, obj.transform.right, _playerData.GetRoteAxis().y * Time.deltaTime * roteYSpeed);
        }

    }
    protected bool WallHitCheck()
    {
        if (Physics.Raycast(targetPos, terminus.transform.position - targetPos, out hit, Vector3.Distance(targetPos, terminus.transform.position), wallLayer, QueryTriggerInteraction.Ignore))
        {
            wallHitPos = hit.point * 0.95f;//���������ꏊ�ɔ�΂��ƃJ�������ǂ̒��ɖ��܂�̂Œ����B
            Logger.Log(hit.point);
            return true;
        }
        else
        {
            return false;
        }
    }
}

