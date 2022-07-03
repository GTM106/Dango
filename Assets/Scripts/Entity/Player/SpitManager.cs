using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpitManager : MonoBehaviour
{
    [SerializeField] PlayerData player = default!;
    DangoUIScript DangoUISC;

    private void Start()
    {
        DangoUISC = player.GetDangoUIScript();
    }

    /// <summary>
    /// �˂��h���{�^���������ꂽ�Ƃ���true�ɂȂ�B
    /// </summary>
    public bool isSticking = false;

    private void OnTriggerStay(Collider other)
    {
        //�h�����Ԃł͂Ȃ��Ȃ���s���Ȃ�
        if (!isSticking) return;
        if (player.GetDangos().Count >= player.GetMaxDango()) return;

        if (other.gameObject.TryGetComponent(out DangoManager dango))
        {
            //SE
            GameManager.SoundManager.PlaySE(SoundSource.SE_STAB_DANGO);

            //�����A�N�V�������ɍs������
            OnFallAction();

            //�c�q���h��
            player.AddDangos(dango.GetDangoColor());

            //�t�B�[���h�ɂ���c�q������
            other.gameObject.SetActive(false);

            //UI�̍X�V
            DangoUISC.DangoUISet(player.GetDangos());

            //�h����������
            isSticking = false;
        }
    }

    private void OnFallAction()
    {
        if (!player.PlayerFall.IsFallAction) return;

        Logger.Log("�����A�N�V�������Ɏh�����I");
    }
}
