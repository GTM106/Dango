using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static FloorManager;

public class FloorData : MonoBehaviour
{
    [SerializeField] FloorManager floorManager;
    [SerializeField] Floor floor;

    Mesh _mesh;

    //�t���A�Ɍ������Ă���c�q�̐�
    int _dangoCount;

    private void Awake()
    {
        CreateInvertedMeshCollider();
    }

    private void OnTriggerEnter(Collider other)
    {
        DangoTriggerEnter(other);
        PlayerTriggerEnter(other);
    }

    private void OnTriggerExit(Collider other)
    {
        DangoTriggerExit(other);
        PlayerTriggerExit(other);
    }

    private void DangoTriggerEnter(Collider other)
    {
        //�c�q�ȊO��e��
        if (other.GetComponentInParent<DangoData>() == null) return;

        _dangoCount++;
        floorManager.CheckDangoIsFull(other, floor);
    }
    private void PlayerTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;

        QuestManager.Instance.SucceedChecker.CheckQuestDestinationSucceed(floor, true);
    }

    private void DangoTriggerExit(Collider other)
    {
        //�c�q�ȊO��e��
        if (other.GetComponentInParent<DangoData>() == null) return;

        _dangoCount--;
        floorManager.CheckDangoIsNotFull(other, floor, 1);
    }
    private void PlayerTriggerExit(Collider other)
    {
        if (other.GetComponentInParent<PlayerData>() == null) return;

        QuestManager.Instance.SucceedChecker.CheckQuestDestinationSucceed(floor, false);
    }

    private void CreateInvertedMeshCollider()
    {
        InvertMesh();

        GameObject obj = new GameObject();
        obj.transform.parent = transform;
        obj.AddComponent<MeshCollider>().sharedMesh = _mesh;
        obj.layer = 8;
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.identity;
    }

    private void InvertMesh()
    {
        _mesh = GetComponent<MeshFilter>().mesh;
        _mesh.triangles = _mesh.triangles.Reverse().ToArray();
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