using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangoInjection : MonoBehaviour
{
    [SerializeField] GameObject spawner = default!;

    [SerializeField] private int defalutInjectionCount = 1;
    [SerializeField] private int defalutContinueFrame = 0;
    [SerializeField] private int waitFrame = 50;

    [SerializeField] private float shotPower = 10f;

    [SerializeField, Tooltip("�c�����̉���")] Vector2 verticalRot;
    [SerializeField, Tooltip("�������̉���")] Vector2 horizontalRot;

    private DangoPoolManager _poolManager = default!;

    private Vector3 _lookAngle = default;
    private Vector3 _nextLookAngle = default;
    private Vector3 _firstLookAngle = default;

    private int _injectionCount = default;
    private int _continueFrame = default;
    private int _currentWaitFrame = default;


    private void Awake()
    {
        Logger.Assert(verticalRot.x < verticalRot.y);
        Logger.Assert(horizontalRot.x < horizontalRot.y);

        //������
        _poolManager = GameObject.Find("DangoPoolManager").GetComponent<DangoPoolManager>();
        _firstLookAngle = transform.rotation.eulerAngles;
        NextLook();
    }

    private void FixedUpdate()
    {
        //�ҋ@���̓A�j���[�V�����A�ҋ@�I���Ŕ���
        if (--_currentWaitFrame <= 0)Injection();
        else SmoothLookRotation(_currentWaitFrame);
    }

    private void Injection()
    {
        //�����m��
        transform.rotation = Quaternion.Euler(_lookAngle);

        //�A���ł���FRAME���Ǘ�
        if (--_continueFrame > 0) return;

        //�c�q����
        if (!ShotDango()) return;

        //���̔��˒n�_������
        NextLook();
    }

    private bool ShotDango()
    {
        var dango = _poolManager.DangoPool.Get();
        dango.transform.position = spawner.transform.position;
        dango.Rb.AddForce(transform.forward.normalized * shotPower, ForceMode.Impulse);
        _continueFrame = defalutContinueFrame;

        return --_injectionCount <= 0;
    }

    /// <summary>
    /// �A�j���[�V�����̊֐��B�ςȂ��ߕύX�����B
    /// </summary>
    /// <param name="frame"></param>
    private void SmoothLookRotation(int frame)
    {
        Vector3 interpolatedValue = _nextLookAngle - _lookAngle;

        transform.rotation = Quaternion.Euler(_nextLookAngle + (interpolatedValue / frame));
    }

    private void NextLook()
    {
        _currentWaitFrame = waitFrame;
        _nextLookAngle = transform.rotation.eulerAngles;
        _continueFrame = 0;
        _injectionCount = defalutInjectionCount;

        _lookAngle = _firstLookAngle + new Vector3(Random.Range(verticalRot.x, verticalRot.y), Random.Range(horizontalRot.x, horizontalRot.y), 0);
    }
}