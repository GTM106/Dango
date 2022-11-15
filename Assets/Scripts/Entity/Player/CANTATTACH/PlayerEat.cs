using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TM.Entity.Player
{
    class PlayerEat
    {
        RoleDirectingScript _roleDirecting;
        PlayerUIManager _playerUIManager;
        PlayerKusiScript _playerKusiScript;

        public PlayerEat(RoleDirectingScript roleDirecting, PlayerUIManager playerUIManager,PlayerKusiScript kusiScript)
        {
            _roleDirecting = roleDirecting;
            _playerUIManager = playerUIManager;
            _playerKusiScript = kusiScript;
        }

        public void EatDango(PlayerData parent)
        {
            //SE
            SoundManager.Instance.PlaySE(SoundSource.SE6_CREATE_ROLE_CHARACTER_ANIMATION);

            //�H�ׂ��c�q�̓_�����擾
            float score = DangoRole.instance.CheckRole(parent.GetDangos(), parent.GetCurrentStabCount());

            //���o�֐��̌Ăяo��
            _roleDirecting.Dirrecting(parent.GetDangos());

            //�����x���㏸
            parent.AddSatiety(score);

            //�����N���A�B
            parent.ResetDangos();

            //UI�X�V
            parent.GetDangoUIScript().DangoUISet(parent.GetDangos());
            parent.GetDangoUIScript().RemoveDango(parent.GetDangos());

            //���̒c�q�ύX
            _playerKusiScript.SetDango(parent.GetDangos());

            //�ꕔUI�̔�\��
            _playerUIManager.EatDangoUI_False();
        }

        public bool WaitAnimationComplete(Animator animator)
        {
            return animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f;
        }
    }
}