using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangoInjection : MonoBehaviour
{
    [System.Flags]
    private enum DangoColorChoice
    {
        None = 0,

        Red = 1 << 1,
        Orange = 1 << 2,
        Yellow = 1 << 3,
        Green = 1 << 4,
        Cyan = 1 << 5,
        Blue = 1 << 6,
        Purple = 1 << 7,

        All = 8,
        SET_ALL = ~0,
    }

    [SerializeField] DangoColorChoice colorChoice;

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

    private List<DangoColor> dangoColors = new();

    private void Awake()
    {
        Logger.Assert(verticalRot.x < verticalRot.y);
        Logger.Assert(horizontalRot.x < horizontalRot.y);
        Logger.Assert(colorChoice != DangoColorChoice.None);

        //������
        _poolManager = GameObject.Find("DangoPoolManager").GetComponent<DangoPoolManager>();
        _firstLookAngle = transform.rotation.eulerAngles;
        NextLook();

        for (int i = 1; i < (int)DangoColorChoice.All; i++)
        {
            if (colorChoice.HasFlag((DangoColorChoice)(1 << i)))
            {
                dangoColors.Add((DangoColor)i);
            }
        }
    }

    private void FixedUpdate()
    {
        //�ҋ@���̓A�j���[�V�����A�ҋ@�I���Ŕ���
        if (--_currentWaitFrame <= 0) Injection();
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
        if(dangoColors.Count == 0) return false;
        var color = dangoColors[Random.Range(0, dangoColors.Count)];

        var dango = _poolManager.DangoPool.Get();
        dango.SetDangoColor(color);
        dango.Rend.material.color = dango.GetDangoColor() switch
        {
            DangoColor.Red => Color.red,
            DangoColor.Orange => new Color32(255, 155, 0, 255),
            DangoColor.Yellow => Color.yellow,
            DangoColor.Green => Color.green,
            DangoColor.Cyan => Color.cyan,
            DangoColor.Blue => Color.blue,
            DangoColor.Purple => new Color32(200, 0, 255, 255),
            DangoColor.Other => Color.gray,
            _ => Color.white,
        };

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