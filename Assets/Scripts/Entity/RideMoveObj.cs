using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RideMoveObj : MonoBehaviour
{
    Transform _parant = null;

    private void OnEnable()
    {
        if (transform.parent != null) _parant = transform.parent;
    }

    void OnCollisionEnter(Collision col)
    {
        //�������̂�e�ɂ���
        if (col.gameObject.CompareTag("MoveObj"))
        {
            transform.parent = col.transform;
        }
    }

    void OnCollisionExit(Collision col)
    {
        //�e�̉���
        if (col.gameObject.CompareTag("MoveObj"))
        {
            transform.parent = _parant;
        }
    }
}
