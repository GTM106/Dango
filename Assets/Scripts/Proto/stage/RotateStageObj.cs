using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateStageObj : MonoBehaviour
{
    [SerializeField, Tooltip("�����N�_�ɂȂ�I�u�W�F�N�g������΂���")] GameObject pointObj;
    [SerializeField, Tooltip("��]�̒��S")] Vector3 point;
    [SerializeField, Tooltip("���S����ǂ̂��炢����邩")] Vector3 distance;
    [SerializeField] float speed;

    // Start is called before the first frame update
    void Start()
    {
        if (pointObj != null)
            point = pointObj.transform.position;

        transform.position = point + distance;
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(point.normalized, Vector3.up, speed * Time.deltaTime);
    }
}
