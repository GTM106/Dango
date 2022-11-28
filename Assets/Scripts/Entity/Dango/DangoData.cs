using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �c�q�Ɋւ���}�l�[�W���[�N���X
/// </summary>
public class DangoData : MonoBehaviour
{
    bool _isMoveable = true;

    //�c�q�����F�f�[�^
    DangoColor _color = DangoColor.None;

    FloorManager.Floor _floor;
    FloorManager _floorManager;

    [SerializeField] Renderer _rend;
    [SerializeField] Rigidbody _rigidbody;
    [SerializeField] Animator _animator;

    public Renderer Rend => _rend;
    public Rigidbody Rb => _rigidbody;
    public Animator Animator => _animator;

    //�I�u�W�F�N�g�v�[���}�l�[�W���[
    DangoPoolManager _poolManager;

    //�~�ϒc�q���������Ă���t���A
    FloorArray _salvationFloor;
    List<FloorArray> _canShotList = new();

    static bool[] completedInitialization = new bool[5];

    private void Awake()
    {
        _poolManager = GameObject.Find("DangoPoolManager").GetComponent<DangoPoolManager>();
    }

    public void OnEnable()
    {
        //�ړ��\�ɂ���
        _isMoveable = true;
        gameObject.SetLayerIncludeChildren(0);
    }

    private void FixedUpdate()
    {
        MoveAndRotation();
    }

    private void MoveAndRotation()
    {
        if (!_isMoveable) return;

        if (_rigidbody.velocity.magnitude < 10)
        {
            //�w�肵���X�s�[�h���猻�݂̑��x�������ĉ����͂����߂�
            float currentSpeed = 10 - _rigidbody.velocity.magnitude;
            //�������ꂽ�����͂ŗ͂�������
            _rigidbody.AddForce(transform.forward * currentSpeed);
        }

        //�����ŉ�]�����ł����
        if (_rigidbody.velocity.magnitude < 0.01f) transform.Rotate(0, Random.Range(90f, 270f), 0);
    }

    public void ReleaseDangoPool(int stabCount)
    {
        //�����̒c�q�������ւ炷
        _floorManager.FloorArrays[(int)_floor].RemoveDangoCount(1, _color);

        //���̒c�q���~�ϒc�q�ł����
        if (_salvationFloor != null)
        {
            //�����̓o�^��������
            _salvationFloor.SetSalvationDango(null);

            //���̒c�q�̓o�^��������
            _salvationFloor = null;
        }

        if (completedInitialization[stabCount - 3])
        {
            //�~�Ϗ���
            Salvation(stabCount, _color);
        }
        else
        {
            for (DangoColor i = DangoColor.None + 1; i < DangoColor.Other - 1; i++)
            {
                Salvation(stabCount, i);
            }
            completedInitialization[stabCount - 3] = true;
        }
        //�v�[���ɕԋp����
        _poolManager.DangoPool[(int)_color - 1].Release(this);
    }

    private void Salvation(int stabCount, DangoColor color)
    {
        //�N���\�t���A�̃e�[�u������A���̃e�[�u���ɂ��鍡��������F�̌����i�[
        for (int i = 0; i <= stabCount - 3; i++)
        {
            foreach (var d5 in _floorManager.IntrudableTable[stabCount - 3 - i])
            {
                //�ЂƂł����݂��Ă�����ȍ~�͍s��Ȃ�
                if (d5.DangoCounts[(int)color - 1] > 0) return;
            }
        }

        //�~�σV���b�g���ł��郊�X�g�̒ǉ�
        _canShotList.Clear();

        foreach (var list in _floorManager.SalvationTable[stabCount - 3])
        {
            if (list.AlreadyExistSavlationDango()) continue;

            _canShotList.Add(list);
        }

#if UNITY_EDITOR
        //���̏����̓f�o�b�O�p�Ȃ��߃r���h�ɂ͒ʂ��܂���
        //�������ˉ\�ȃt���A�����݂��Ȃ�������ȍ~�͍s��Ȃ�
        if (_canShotList.Count == 0)
        {
            Logger.Warn("�~�ϔ��ˉ̂��G���A������܂���B�ݒ���������Ă��������B" + "\n" + "���݂�D5�F" + stabCount);
            return;
        }
#endif

        //���ˉ\�ȃ����_���ȃt���A��I�����A�~�σV���b�g
        int rand = Random.Range(0, _canShotList.Count);
        var salvationDango = _canShotList[rand].DangoInjections[0].EnforcementShot(color);
        salvationDango._salvationFloor = _canShotList[rand];
        _canShotList[rand].SetSalvationDango(salvationDango);
        Logger.Log("�~�ϔ���" + color + "\n" + "�t���A�F" + _canShotList[rand].FloorDatas[0].name);
    }

    public DangoColor GetDangoColor() => _color;

    public void SetDangoColor(DangoColor type)
    {
        _color = type;
    }

    public FloorManager.Floor Floor => _floor;
    public void SetFloor(FloorManager.Floor floor) => _floor = floor;
    public void SetFloorManager(FloorManager floorManager) => _floorManager = floorManager;
    public void SetIsMoveable(bool enable) => _isMoveable = enable;
}
