using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpitManager : MonoBehaviour
{
    [SerializeField] PlayerData player = default!;
    [SerializeField] CapsuleCollider _capsuleCollider = default!;
    DangoUIScript DangoUISC;

    [SerializeField] FloorManager _floorManager;
    [SerializeField] PlayerKusiScript kusiScript;
    [SerializeField] PlayerUIManager _playerUIManager;
    [SerializeField] Transform _dangoParent;

    //�q�b�g�X�g�b�v�̒�~�t���[���ł��B������3d5,4d5...7d5�ł�
    static readonly List<int> hitStopFrameTable = new() { 0, 0, 0, 0, 0 };

    private bool _isSticking;
    private bool _isInWall;
    private bool _isHitStop;

    private void Awake()
    {
        DangoUISC = player.GetDangoUIScript();
        _capsuleCollider.enabled = false;
    }

    public bool IsSticking
    {
        get => _isSticking;
        set
        {
            _capsuleCollider.enabled = value;
            if (player.IsGround) _capsuleCollider.radius = 0.1f;
            else _capsuleCollider.radius = 0.5f;

            _isSticking = value;
        }
    }

    public bool IsHitStop => _isHitStop;

    private async void OnTriggerEnter(Collider other)
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

            //�h����������
            IsSticking = false;

            QuestManager.Instance.SucceedChecker.CheckQuestPlayActionSucceed(Dango.Quest.QuestPlayAction.PlayerAction.Stab);

            //�����A�N�V�������ɍs�������E�q�b�g�X�g�b�v�O�łȂ��Ƌ}�~���̓�Փx���オ���Ă��܂�
            OnFallAction();

            //�q�b�g�X�g�b�v
            await HitStop(dango);

            //�c�q���h��
            player.AddDangos(dango);

            //�c�q�̃p�����[�^�𒲐�
            dango.Animator.speed = 0f;
            dango.SetIsMoveable(false);

            //�t�B�[���h�ɂ���c�q������
            dango.StabAnimation(player.GetAnimator(), player.GetCurrentStabCount(), _dangoParent);

            //UI�̍X�V
            DangoUISC.DangoUISet(player.GetDangos());
            DangoUISC.AddDango(player.GetDangos());

            //���̒c�q�ύX
            kusiScript.SetDango(player.GetDangos());

            //MAX�x��
            int maxCurrent = player.GetDangos().Count;
            int current = player.GetCurrentStabCount();
            if (maxCurrent == current)
                _playerUIManager.MAXDangoSet(true);

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

    private async UniTask HitStop(DangoData dango)
    {
        if (hitStopFrameTable[player.GetCurrentStabCount() - 3] == 0) return;

        _isHitStop = true;

        float pSpeed = player.GetAnimator().speed;
        float dSpeed = dango.Animator.speed;

        Vector3 playerVelocity = player.Rb.velocity;

        //�A�j���[�V�������ꎞ��~
        player.GetAnimator().speed = 0f;
        dango.Animator.speed = 0f;

        //�ړ����ꎞ��~
        player.Rb.velocity = Vector3.zero;
        player.Rb.isKinematic = true;
        player.SetIsMoveable(false);
        dango.Rb.velocity = Vector3.zero;
        dango.Rb.isKinematic = true;
        dango.SetIsMoveable(false);

        await UniTask.DelayFrame(hitStopFrameTable[player.GetCurrentStabCount() - 3], PlayerLoopTiming.FixedUpdate);

        //�ړ���A�j���[�V���������ɖ߂�
        player.GetAnimator().speed = pSpeed;
        player.Rb.isKinematic = false;
        player.SetIsMoveable(true);
        player.Rb.velocity = playerVelocity;
        dango.Animator.speed = dSpeed;
        dango.Rb.isKinematic = false;

        _isHitStop = false;
    }

    private void OnFallAction()
    {
        if (!player.PlayerFall.IsFallAction) return;

        //�����A�N�V�������񂵂�n�N�G�X�g�̔���
        QuestManager.Instance.SucceedChecker.CheckQuestPlayActionSucceed(Dango.Quest.QuestPlayAction.PlayerAction.FallAttack);

        Logger.Log("�����A�N�V�������Ɏh�����I");
    }
}
