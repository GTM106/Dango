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

    FloorManager.Floor _floor;
    FloorManager _floorManager;

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
        if (_rigidbody.velocity.magnitude < 10)
        {
            //�w�肵���X�s�[�h���猻�݂̑��x�������ĉ����͂����߂�
            float currentSpeed = 10 - _rigidbody.velocity.magnitude;
            //�������ꂽ�����͂ŗ͂�������
            _rigidbody.AddForce(transform.forward * currentSpeed);
        }

        //�����ŉ�]�����ł����
        if (_rigidbody.velocity.magnitude < 0.01f) transform.Rotate(0, Random.Range(90f, 270f), 0);

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
        //�����̒c�q�������ւ炷
        _floorManager.FloorArrays[(int)_floor].RemoveDangoCount(1);

        _poolManager.DangoPool[(int)_color - 1].Release(this);
    }

    public DangoColor GetDangoColor() => _color;

    public void SetDangoColor(DangoColor type)
    {
        _color = type;
    }

    public FloorManager.Floor Floor => _floor;
    public void SetFloor(FloorManager.Floor floor) => _floor = floor;
    public void SetFloorManager(FloorManager floorManager) => _floorManager = floorManager;
}
