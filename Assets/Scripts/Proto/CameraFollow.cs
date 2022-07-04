using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    enum State
    {
        normal,
        Lerp,
    }
    [SerializeField, Tooltip("�Ǐ]�������^�[�Q�b�g")] Transform target = default!;
    [SerializeField] float MinAngle;
    [SerializeField] float MaxAngle;
    [SerializeField] float t = 1;

    [SerializeField, Range(0.01f, 1f), Tooltip("�J�����̒Ǐ]�x")]
    private float smoothSpeed = 0.125f;

    //[SerializeField, Tooltip("�^�[�Q�b�g����̃J�����̈ʒu")]
    //private Vector3 offset = new Vector3(0f, 1f, -10f);

    //private Vector3 velocity = Vector3.zero;


    private Vector3 targetPos = Vector3.zero;
    private GameObject terminus;

    private PlayerData _playerData;

    private float roteYSpeed = -10f;

    [SerializeField] State state;
    private void Start()
    {
        _playerData = target.GetComponent<PlayerData>();
        transform.parent = null;//�������̂ɏ��Ƃ���ɒǏ]����������e�q�֌W�𖳂���
        terminus = GameObject.Find("CameraTerminusObj");
        terminus.transform.position = transform.position;
    }

    //Player�����������ƂɎ��s���邽�߁ALateUpdate�ōs���B
    private void LateUpdate()
    {
        //�J�����̈ʒu��ύX����
        terminus.transform.position += target.position - targetPos;
        targetPos = target.position;

        //X����]�̊p�x������
        float currentYAngle = transform.eulerAngles.x;

        //X����0�`360�̒l�����Ԃ��Ȃ��̂Œ���
        if (currentYAngle > 180)
        {
            currentYAngle -= 360;
        }
        Rote(terminus, currentYAngle);
        switch (state)
        {

            case State.normal:
                transform.position = terminus.transform.position;
                transform.rotation = terminus.transform.rotation;
                break;
            case State.Lerp:
                transform.rotation = terminus.transform.rotation;
                transform.position = Vector3.Lerp(transform.position, terminus.transform.position, t*Time.deltaTime);
                break;
        }
        //�J�����̈ʒu�����肵�Ă���v���C���[�̌��������߂邱�ƂŁA�J�N�����Ȃ����B
        target.transform.rotation = Quaternion.Euler(0, transform.localEulerAngles.y, 0);

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
}

