using System.Collections.Generic;
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

    //�d�����Ȃ������擾�p
    List<int> _nums = new();

    public void CheckDangoIsFull(Collider other, Floor floor)
    {
        //�c�q�ȊO��e��
        if (other.GetComponent<DangoData>() == null) return;

        //�c�q�̐����t���A�̍ő�l�ɓ��B���Ă��Ȃ�������e��
        if (!CheckFloorDangoIsFull((int)floor)) return;

        //�ő吔�Ȃ炷�ׂĎˏo�֎~�ɂ���
        foreach (var dI in floorArrays[(int)floor].DangoInjections)
        {
            dI.SetCanShot(false);
        }
    }

    public void CheckDangoIsNotFull(Collider other, Floor floor, int shotValue)
    {
        //�c�q�ȊO��e��
        if (other.GetComponent<DangoData>() == null) return;

        //�c�q�̐����t���A�̍ő�l�ɓ��B���Ă�����e��
        if (CheckFloorDangoIsFull((int)floor)) return;

        //��U���ׂĎˏo�֎~�ɂ���
        foreach (var dI in floorArrays[(int)floor].DangoInjections)
        {
            dI.SetCanShot(false);
        }

        //�����p�̃C���f�b�N�X�ԍ����擾
        for(int i = 0; i < floorArrays[(int)floor].DangoInjections.Length; i++)
        {
            _nums.Add(i);
        }

        //�����_���ȑ��u��I������
        for (int i = 0; i < shotValue; i++)
        {
            //�C���f�b�N�X�ԍ����擾
            int index = Random.Range(0, _nums.Count);

            //�d�����Ȃ������_���Ȕ��ˑ��u�̔��˃t���O�𗧂Ă�
            floorArrays[(int)floor].DangoInjections[_nums[index]].SetCanShot(true);
            
            //����擾�����ԍ���I��������r��
            _nums.RemoveAt(index);
        }

        //����p�ɃN���A����
        _nums.Clear();
    }

    bool CheckFloorDangoIsFull(int index)
    {
        int count = 0;

        //�����t���A�ɕ���Data�����݂���ꍇ���Z����
        for (int i = 0; i < floorArrays[index].FloorDatas.Length; i++)
        {
            count += floorArrays[index].FloorDatas[i].DangoCount;
        }

        return count >= floorArrays[index].MaxDangoCount;
    }
}
