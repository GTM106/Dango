using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �c�q�Ɋւ���}�l�[�W���[�N���X
/// </summary>
public class DangoData : MonoBehaviour
{
    //�������ԁi10�b�j
    const int DELETE_FRAME = 500;
    const float DELETE_MIN_SPEED = 0.1f;

    //��������
    int _frameCount = DELETE_FRAME;

    //�c�q�����F�f�[�^
    DangoColor _color = DangoColor.None;

    [SerializeField] Renderer _rend;
    [SerializeField] Rigidbody _rigidbody;

    public Renderer Rend => _rend;
    public Rigidbody Rb => _rigidbody;

    //�I�u�W�F�N�g�v�[���}�l�[�W���[
    DangoPoolManager _poolManager;

    private void Awake()
    {
        _poolManager = GameObject.Find("DangoPoolManager").GetComponent<DangoPoolManager>();
    }

    private void FixedUpdate()
    {
        ReleaseDango();
    }

    private void ReleaseDango()
    {
        if (Rend == null || Rb == null) return;

        //�����c�q���J�����O�ŁA���x�����l�ȉ��ł����
        if (!Rend.isVisible && Rb.velocity.magnitude < DELETE_MIN_SPEED)
        {
            if (--_frameCount <= 0) ReleaseDangoPool();
        }
        else
        {
            _frameCount = DELETE_FRAME;
        }
    }

    public void ReleaseDangoPool()
    {
        _poolManager.DangoPool[(int)_color - 1].Release(this);
    }
    public DangoColor GetDangoColor() => _color;

    public void SetDangoColor(DangoColor type)
    {
        _color = type;
    }
}
