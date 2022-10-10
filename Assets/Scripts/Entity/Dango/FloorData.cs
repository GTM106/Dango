using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static FloorManager;

public class FloorData : MonoBehaviour
{
    [SerializeField] FloorManager floorManager;
    [SerializeField] Floor floor;

    //�t���A�Ɍ������Ă���c�q�̐�
    int _dangoCount;

    private void OnTriggerEnter(Collider other)
    {
        //�c�q�ȊO��e��
        if (other.GetComponent<DangoData>() == null) return;

        _dangoCount++;
        floorManager.CheckDangoIsFull(other, floor);
    }

    private void OnTriggerExit(Collider other)
    {
        //�c�q�ȊO��e��
        if (other.GetComponent<DangoData>() == null) return;

        _dangoCount--;
        floorManager.CheckDangoIsNotFull(other, floor, 1);
    }

    public void AddDangoCount() => _dangoCount++;
    public void RemoveDangoCount() => _dangoCount--;
    public int DangoCount => _dangoCount;
}

[Serializable]
public class FloorArray
{
    [SerializeField, Tooltip("�G���A�̒�`")] FloorData[] floorDatas;
    [SerializeField, Tooltip("�G���A�ɑ��݂���c�q�ˏo���u")] DangoInjection[] dangoInjections;
    [SerializeField, Tooltip("�G���A�ɑ��݂ł���ő�̒c�q�̐�"), Min(0)] int maxDangoCount;

    public FloorData[] FloorDatas => floorDatas;
    public DangoInjection[] DangoInjections => dangoInjections;
    public int MaxDangoCount => maxDangoCount;
}