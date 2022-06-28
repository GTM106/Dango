using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �c�q�Ɋւ���}�l�[�W���[�N���X
/// </summary>
public class DangoManager : MonoBehaviour
{
    //�������ԁi10�b�j
    const int DELETE_FRAME = 500;
    const float DELETE_MIN_SPEED = 0.1f;

    //��������
    int _frameCount = DELETE_FRAME;

    //�c�q�����F�f�[�^
    DangoColor _color = DangoColor.None;

    //�c�q�̃R���|�[�l���g
    [SerializeField] Renderer rend = default!;
    public Rigidbody Rb { get; private set; }

    //�I�u�W�F�N�g�v�[���}�l�[�W���[
    DangoPoolManager _poolManager;

    private void Awake()
    {
        Rb = GetComponent<Rigidbody>();
        _poolManager = GameObject.Find("DangoPoolManager").GetComponent<DangoPoolManager>();
    }

    private void FixedUpdate()
    {
        ReleaseDango();
    }

    private void ReleaseDango()
    {
        //�����c�q���J�����O�ŁA���x�����l�ȉ��ł����
        if (!rend.isVisible && Rb.velocity.magnitude < DELETE_MIN_SPEED)
        {
            if (--_frameCount <= 0) _poolManager.DangoPool.Release(this);
        }
        else
        {
            _frameCount = DELETE_FRAME;
        }
    }

    public DangoColor GetDangoColor() => _color;
    public void SetDangoColor(DangoColor type) => _color = type;
}
