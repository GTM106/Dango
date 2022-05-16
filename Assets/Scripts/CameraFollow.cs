using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Tooltip("�Ǐ]�������^�[�Q�b�g")]
    public Transform target = default!;

    [SerializeField, Range(0.01f, 1f),Tooltip("�J�����̒Ǐ]�x")]
    private float smoothSpeed = 0.125f;

    [SerializeField,Tooltip("�^�[�Q�b�g����̃J�����̈ʒu")]
    private Vector3 offset = new Vector3(0f, 1f, -10f);

    private Vector3 velocity = Vector3.zero;
    private Vector3 targetPos;
    private Plater1 P1;

    private void Start()
    {
        P1 = target.GetComponent<Plater1>();
    }
    //�����̕������v���C���[�̓����ƈ�v�����Ȃ��ƃK�N�K�N����B
    private void FixedUpdate()
    {
        //Vector3 desiredPosition = target.position + offset;

        //transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothSpeed);
        transform.position += target.position - targetPos;
        targetPos = target.position;
        transform.RotateAround(target.position, Vector3.up, P1.angle*Time.deltaTime);

    }
}
