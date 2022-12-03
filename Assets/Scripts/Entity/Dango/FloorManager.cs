using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FloorManager : MonoBehaviour
{
    public enum Floor
    {
        floor1,
        floor2,
        floor3,
        floor4,
        floor5,
        floor6,
        floor7,
        floor8,
        floor9,
        floor10,
        floor11,
        floor12,
        floor13,
        floor14,
        floor15,
        floor16,
        floor17,
        floor18,
        floor19,
        floor20,
        floor21,
        floor22,
        floor23,
        floor24,
        floor25,
        floor26,
        floor27,
        floor28,
        floor29,
        floor30,
        floor31,
        floor32,
        floor33,
        floor34,

        [InspectorName("")]
        Max,
    }

    //�񎟌��z��̓V���A���C�Y�ł��Ȃ����߁A�ʃN���X��p���ĉ��z�I�ɓ񎟌��z��ɂ��Ă���B
    [SerializeField] FloorArray[] floorArrays = new FloorArray[(int)Floor.Max];
    [SerializeField] StageData _stageData;

    //nd5�ŋ~�ω\�ȃt���A�ꗗ
    List<FloorArray> s3d5 = new();
    List<FloorArray> s4d5 = new();
    List<FloorArray> s5d5 = new();
    List<FloorArray> s6d5 = new();
    List<FloorArray> s7d5 = new();

    //nd5�ŐN���\�ȃt���A�ꗗ
    List<FloorArray> i3d5 = new();
    List<FloorArray> i4d5 = new();
    List<FloorArray> i5d5 = new();
    List<FloorArray> i6d5 = new();
    List<FloorArray> i7d5 = new();

    int[] dangoCounts = new int[(int)DangoColor.Other];

    //�~�σt���A�̃e�[�u��
    static readonly List<List<FloorArray>> _salvationTable = new();
    //�N���\�t���A�̃e�[�u��
    static readonly List<List<FloorArray>> _intrudableTable = new();

    private void Awake()
    {
        //������
        InitSalvationTable();
        InitIntrudableTable();
        InitDangoInjectionFloor();
    }

    private void InitSalvationTable()
    {
        //�����[�h���ɏ�񂪎c���Ă��邽�ߔj��
        _salvationTable.Clear();
        s3d5.Clear();
        s4d5.Clear();
        s5d5.Clear();
        s6d5.Clear();
        s7d5.Clear();

        _salvationTable.Add(s3d5);
        _salvationTable.Add(s4d5);
        _salvationTable.Add(s5d5);
        _salvationTable.Add(s6d5);
        _salvationTable.Add(s7d5);

        //�S�t���A�ɑ΂��Ď��s
        for (int i = 0; i < floorArrays.Length; i++)
        {
            //���ׂĂ�D5�ɑ΂��Ď��s
            for (int j = 0; j < _salvationTable.Count; j++)
            {
                //����D5�̋~�σt���O�������Ă���Ȃ�
                if (floorArrays[i].HasFlagSalvageableD5(j + 1))
                {
                    //�~�ϗp�̃e�[�u���ɓo�^
                    _salvationTable[j].Add(floorArrays[i]);
                }
            }
        }
    }

    private void InitIntrudableTable()
    {
        //�����[�h���ɏ�񂪎c���Ă��邽�ߔj��
        _intrudableTable.Clear();
        i3d5.Clear();
        i4d5.Clear();
        i5d5.Clear();
        i6d5.Clear();
        i7d5.Clear();

        _intrudableTable.Add(i3d5);
        _intrudableTable.Add(i4d5);
        _intrudableTable.Add(i5d5);
        _intrudableTable.Add(i6d5);
        _intrudableTable.Add(i7d5);

        //�S�t���A�ɑ΂��Ď��s
        for (int i = 0; i < floorArrays.Length; i++)
        {
            for (int j = 0; j < _intrudableTable.Count; j++)
            {
                //����D5�̐N���t���O�������Ă���Ȃ�
                if (floorArrays[i].HasFlagIntrudableD5(j + 1))
                {
                    _intrudableTable[j].Add(floorArrays[i]);
                }
            }
        }
    }

    private void InitDangoInjectionFloor()
    {
        //�S�t���A�ɑ΂��Ď��s
        for (int i = 0; i < floorArrays.Length; i++)
        {
            //���ˑ��u�̃t���A��o�^
            foreach (var injection in floorArrays[i].DangoInjections)
            {
                injection.SetFloor((Floor)i);
            }
        }
    }

    public List<List<FloorArray>> SalvationTable => _salvationTable;
    public List<List<FloorArray>> IntrudableTable => _intrudableTable;
    public void AddDangoCount(int num, DangoColor color) => dangoCounts[(int)color] += num;
    public void ResetAllDangoCount()
    {
        for (int i = 0; i < dangoCounts.Length; i++)
        {
            dangoCounts[i] = 0;
        }
    }

    public FloorArray[] FloorArrays => floorArrays;
    public StageData StageData => _stageData;
}