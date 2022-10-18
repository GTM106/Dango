using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpitManager : MonoBehaviour
{
    [SerializeField] PlayerData player = default!;
    [SerializeField] CapsuleCollider _capsuleCollider = default!;
    DangoUIScript DangoUISC;

    private void Awake()
    {
        DangoUISC = player.GetDangoUIScript();
        _capsuleCollider.enabled = false;
    }

    private bool _isSticking;
    private bool _isInWall;

    /// <summary>
    /// �˂��h���{�^���������ꂽ�Ƃ���true�ɂȂ�B
    /// </summary>
    public bool IsSticking
    {
        get => _isSticking;
        set
        {
            _capsuleCollider.enabled = value;
            _isSticking = value;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<DangoData>() == null) return;
        
        //�h�����Ԃł͂Ȃ��Ȃ���s���Ȃ�
        if (!IsSticking) return;
        if (_isInWall) return;

        if (LayerMask.LayerToName(other.gameObject.layer) == "Map")
        {
            _isInWall = true;
            return;
        }

        if (player.GetDangos().Count >= player.GetMaxDango())
        {
            if (!player.PlayerFall.IsFallAction) return;

            //�}�~�����Ȃ�c�q��e��
            Logger.Log("�ۂ�[��");
            other.GetComponent<Rigidbody>().AddForce((other.transform.position - player.transform.position) * 50f, ForceMode.Impulse); ;
            return;
        }

        if (other.gameObject.TryGetComponent(out DangoData dango))
        {
            //SE
            SoundManager.Instance.PlaySE(SoundSource.SE14_STAB_DANGO);

            //�����A�N�V�������ɍs������
            OnFallAction();

            //�c�q���h��
            player.AddDangos(dango.GetDangoColor());

            //�t�B�[���h�ɂ���c�q������
            dango.ReleaseDangoPool();

            //UI�̍X�V
            DangoUISC.DangoUISet(player.GetDangos());

            //�h����������
            IsSticking = false;
        }
        else
        {
            SoundManager.Instance.PlaySE(SoundSource.SE13_ATTACK);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!_isInWall) return;

        if (LayerMask.LayerToName(other.gameObject.layer) == "Map")
        {
            _isInWall = false;
            return;
        }
    }

    private void OnFallAction()
    {
        if (!player.PlayerFall.IsFallAction) return;

        //�����A�N�V�������񂵂�n�N�G�X�g�̔���
        QuestManager.Instance.SucceedChecker.CheckQuestPlayActionSucceed(QuestManager.Instance, Dango.Quest.QuestPlayAction.PlayerAction.FallAttack);

        Logger.Log("�����A�N�V�������Ɏh�����I");
    }
}
