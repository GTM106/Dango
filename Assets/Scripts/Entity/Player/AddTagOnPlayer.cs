using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddTagOnPlayer : MonoBehaviour
{
    //---------------------------------------------------------
    //�V�l�}�V�[�����g�p�����ۂɓ��ꏈ���Ƃ��ăR���C�_�[�𖳎�����^�O��t�^���邽�߂̃X�N���v�g�ł��B
    //�o�O�̌������킩�炸���}���u�I�Ȃ��ƂɂȂ��Ă��邽�߁A�������킩�莟��폜�����ɂȂ�܂��B
    //---------------------------------------------------------

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            gameObject.tag = "StageCollider";
        }

    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            gameObject.tag = "Untagged";
        }
    }
}
