using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveStageObjScript : MonoBehaviour
{
    [SerializeField, Tooltip("�ړ���")] Vector3[] amountMove;
    [SerializeField, Tooltip("�X�s�[�h")] float moveSpeed;
    [SerializeField, Tooltip("��~����")] float delayTime;

    //�ړ�����ڕW�n�_���W
    Vector3[] DestPos;

    //�n�_
    int point = 0;

    bool isStay = false;

    // Start is called before the first frame update
    void Start()
    {
        DestPos = new Vector3[amountMove.Length + 1];
        DestPos[0] = transform.position;

        for (int i = 0; i < amountMove.Length; i++)
        {
            DestPos[i + 1] = amountMove[i] + DestPos[i];//�s���̖ڕW�n�_
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (OnPositionComparison(DestPos[point + 1]))
        {
            StartCoroutine(DelayMove());
        }

        Move(amountMove[point]);
    }

    private void Move(Vector3 point)//�ړ�
    {
        if (isStay) return;

        transform.Translate(moveSpeed * Time.deltaTime * point.normalized);
    }

    private bool OnPositionComparison(Vector3 point)//�����̈ʒu�ƖڕW�n�_��pos���r
    {
        return Vector3.Distance(transform.position, point) <= 0.1f;
    }

    private IEnumerator DelayMove()
    {
        isStay = true;

        point = (point + 1) % amountMove.Length;
        transform.position = DestPos[point];

        yield return new WaitForSeconds(delayTime);

        isStay = false;
    }
}
