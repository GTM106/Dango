using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Tooltip("�Ǐ]�������^�[�Q�b�g")]
    public Transform target = default!;
    [SerializeField]private float MinAngle;

    [SerializeField]private float MaxAngle;

    ////[SerializeField, Range(0.01f, 1f),Tooltip("�J�����̒Ǐ]�x")]
    //private float smoothSpeed = 0.125f;

    //[SerializeField,Tooltip("�^�[�Q�b�g����̃J�����̈ʒu")]
    //private Vector3 offset = new Vector3(0f, 1f, -10f);

    //private Vector3 velocity = Vector3.zero;
    private Vector3 targetPos = Vector3.zero;
    private Player1 P1;
    private float roteYSpeed=-10f;
    private Vector3 oldRote=new Vector3(0,0,0);
    private void Start()
    {
        P1 = target.GetComponent<Player1>();
        transform.parent = null;//�������̂ɏ��Ƃ���ɒǏ]����������e�q�֌W�𖳂���
    }

    //Player�����������ƂɎ��s���邽�߁ALateUpdate�ōs���B
    private void LateUpdate()
    {
        //�J�����̈ʒu��ύX����
        transform.position += target.position - targetPos;
        targetPos = target.position;

        //�J������roteAxis.x�ɍ��킹�ĉ�]������B
        transform.RotateAround(target.position, Vector3.up, P1.GetRoteAxis().x * Time.deltaTime);

        //X����]�̊p�x������
        float currentYAngle = transform.eulerAngles.x;
        //X����0�`360�̒l�����Ԃ��Ȃ��̂Œ���
        if (currentYAngle > 180)
        {
            currentYAngle = currentYAngle - 360;
        }

        //�c���̐���
        if ((currentYAngle >= MinAngle&&P1.GetRoteAxis().y>0) || (currentYAngle <= MaxAngle&&P1.GetRoteAxis().y < 0))
        {
            transform.RotateAround(target.position, transform.right, P1.GetRoteAxis().y * Time.deltaTime * roteYSpeed);
        }

        //�J�����̈ʒu�����肵�Ă���v���C���[�̌��������߂邱�ƂŁA�J�N�����Ȃ����B
        target.transform.rotation = Quaternion.Euler(0, transform.localEulerAngles.y, 0);

    }
}
