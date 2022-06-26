using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField, Tooltip("�Ǐ]�������^�[�Q�b�g")] Transform target = default!;
    [SerializeField] float MinAngle;
    [SerializeField] float MaxAngle;

    ////[SerializeField, Range(0.01f, 1f),Tooltip("�J�����̒Ǐ]�x")]
    //private float smoothSpeed = 0.125f;

    //[SerializeField,Tooltip("�^�[�Q�b�g����̃J�����̈ʒu")]
    //private Vector3 offset = new Vector3(0f, 1f, -10f);

    //private Vector3 velocity = Vector3.zero;


    private Vector3 targetPos = Vector3.zero;

    private PlayerData _playerData;

    private float roteYSpeed = -10f;
    private void Start()
    {
        _playerData = target.GetComponent<PlayerData>();
        transform.parent = null;//�������̂ɏ��Ƃ���ɒǏ]����������e�q�֌W�𖳂���
    }

    //Player�����������ƂɎ��s���邽�߁ALateUpdate�ōs���B
    private void LateUpdate()
    {
        //�J�����̈ʒu��ύX����
        transform.position += target.position - targetPos;
        targetPos = target.position;

        //�J������roteAxis.x�ɍ��킹�ĉ�]������B
        transform.RotateAround(target.position, Vector3.up, _playerData.GetRoteAxis().x * Time.deltaTime);

        //X����]�̊p�x������
        float currentYAngle = transform.eulerAngles.x;
        //X����0�`360�̒l�����Ԃ��Ȃ��̂Œ���
        if (currentYAngle > 180)
        {
            currentYAngle -= 360;
        }

        //�c���̐���
        if ((currentYAngle >= MinAngle && _playerData.GetRoteAxis().y > 0) || (currentYAngle <= MaxAngle && _playerData.GetRoteAxis().y < 0))
        {
            transform.RotateAround(target.position, transform.right, _playerData.GetRoteAxis().y * Time.deltaTime * roteYSpeed);
        }

        //�J�����̈ʒu�����肵�Ă���v���C���[�̌��������߂邱�ƂŁA�J�N�����Ȃ����B
        target.transform.rotation = Quaternion.Euler(0, transform.localEulerAngles.y, 0);

    }
}
