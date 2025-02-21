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
        //動く物体を親にする
        if (col.gameObject.CompareTag("MoveObj"))
        {
            transform.parent = col.transform;
        }
    }

    void OnCollisionExit(Collision col)
    {
        //親の解除
        if (col.gameObject.CompareTag("MoveObj"))
        {
            transform.parent = _parant;
        }
    }
}
