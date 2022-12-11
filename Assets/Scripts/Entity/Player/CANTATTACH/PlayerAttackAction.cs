using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace TM.Entity.Player
{
    public class PlayerAttackAction
    {
        public enum AttackPattern
        {
            NormalAttack,
            FallAttack
        }

        //�ǂ̂��炢�̊p�x���T�[�`���邩�B[�P��:�x���@]
        //���̒l�����łȂ��AUI�̕ύX���K�v�B
        const float SEARCH_ANGLE = 30f;

        //�c�q�܂ł��ǂ蒅������
        const float RUSH_TIME = 0.2f;

        const float ANIMATION_FLAME = 11f / 30f;

        Animator _animator;
        SpitManager _spitManager;

        List<DangoData> _targetDangoList = new();
        DangoData _highlightDango;

        bool _hasStabDango = false;

        public PlayerAttackAction(Animator animator, SpitManager spitManager)
        {
            _animator = animator;
            _spitManager = spitManager;
        }

        public void FixedUpdate(Transform transform)
        {
            if (_targetDangoList.Count == 0)
            {
                ResetHighlightDango();
                return;
            }

            float min = float.MaxValue;
            int index = -1;

            //�ł��߂��̒c�q���^�[�Q�b�g����
            for (int i = 0; i < _targetDangoList.Count; i++)
            {
                Vector3 vec = _targetDangoList[i].transform.position - transform.position;

                //���C���΂��āA�Ԃɕǂ����邩����
                if (Physics.Raycast(transform.position, vec, out RaycastHit hit, vec.magnitude, ~1 << LayerMask.NameToLayer("MapCollider")))
                {
                    if (hit.collider.GetComponent<DangoData>() == null) continue;
                }

                float targetAngle = Vector3.Angle(transform.forward, vec);

                Debug.DrawRay(transform.position, vec, Color.blue);

                //�T�[�`����p�x���傫���������߂�
                if (targetAngle >= SEARCH_ANGLE) continue;

                float distance = Vector3.Distance(transform.position, _targetDangoList[i].transform.position);

                if (distance < min)
                {
                    min = distance;
                    index = i;
                }
            }

            if (index == -1)
            {
                ResetHighlightDango();
                return;
            }

            //�n�C���C�g�\��
            SetHighlightDango(_targetDangoList[index]);
        }

        public bool ChangeState(AttackPattern attack)
        {
            return attack == AttackPattern.NormalAttack ? IsWaitingNormalAttack() : IsWaitingFallAttackAnimation();
        }

        //�����A�N�V�������̓˂��h��
        private bool IsWaitingFallAttackAnimation()
        {
            var stateInfo = _animator.GetCurrentAnimatorStateInfo(0);

            if (stateInfo.IsName("AN1")) return true;
            if (!stateInfo.IsName("AN7B")) return false;

            _spitManager.IsSticking = false;
            return stateInfo.normalizedTime >= 0.7f;
        }

        //�ʏ�̓˂��h��
        private bool IsWaitingNormalAttack()
        {
            var stateInfo = _animator.GetCurrentAnimatorStateInfo(0);

            if (!stateInfo.IsName("AN5")) return false;

            return stateInfo.normalizedTime >= 0.9f;
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out DangoData dango))
            {
                _targetDangoList.Add(dango);
            }
        }

        public void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out DangoData dango))
            {
                _targetDangoList.Remove(dango);
            }
        }

        public void WasPressedAttackButton(Rigidbody rb)
        {
            if (_highlightDango is null)
            {
                //�m�[�^�[�Q�b�g�ł��O�i����
                RushStart(rb, 2f);
                return;
            }

            //�n�C���C�g�����c�q�����Ɏh����悤�ɂ���
            _highlightDango.SetIsMoveable(false);
            _highlightDango.Rb.velocity = Vector3.zero;

            _hasStabDango = false;

            float distance = Vector3.Distance(_highlightDango.transform.position, rb.transform.position);

            //�������Ɍ�����
            rb.transform.LookAt(_highlightDango.transform.position.SetY(rb.position.y));

            //�O�i(������������Ƃ���Ηǂ�)
            RushStart(rb, distance);
        }

        private async void RushStart(Rigidbody rb, float distance)
        {
            //�ʃA�j���[�V�������I��肫���Ă��Ȃ���ΏI���܂őҋ@
            if (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime > ANIMATION_FLAME)
            {
                while (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime > ANIMATION_FLAME)
                {
                    await UniTask.Yield();
                }
            }

            //�O�̂��߂̃o�O�`�F�b�N
            Logger.Assert(_animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= ANIMATION_FLAME);

            //�O�i����t���[���܂őҋ@
            while (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= ANIMATION_FLAME)
            {
                await UniTask.Yield();

            }

            //�˂��h�����Ԃɂ���
            _spitManager.IsSticking = true;

            //��ւ̉^����f���؂肽�����ߒ��ڏ�������
            rb.velocity = (rb.transform.forward.normalized * distance / RUSH_TIME).SetY(0);

            //�������ːi���h���������~����ɂ��
            OnRush(rb);
        }

        private async void OnRush(Rigidbody rigidbody)
        {
            try
            {
                while (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1f)
                {
                    await UniTask.Yield();

                    //�ːi���Ɏh��������}��~
                    if (_hasStabDango)
                    {
                        _hasStabDango = false;
                        rigidbody.velocity = Vector3.zero;

                        //�^�[�Q�b�g�ꗗ���獡�̒c�q�������B
                        ResetHighlightDango();
                        return;
                    }
                }
            }
            catch (MissingReferenceException)
            {
                return;
            }
        }

        public void SetHasStabDango()
        {
            _hasStabDango = true;
        }

        private void SetHighlightDango(DangoData dango)
        {
            //���݃n�C���C�g���̒c�q�ƈ�v���Ă�����e��
            if (_highlightDango == dango) return;

            //�n�C���C�g���̒c�q�����ɑ��݂��Ă�����n�C���C�g����߂�
            ResetHighlightDango();

            //�Z�b�g
            _highlightDango = dango;
            _highlightDango.gameObject.SetLayerIncludeChildren(9);
        }

        private void ResetHighlightDango()
        {
            if (_highlightDango == null) return;

            _highlightDango.gameObject.SetLayerIncludeChildren(0);
            _highlightDango = null;
        }

        public void RemoveTargetDango(DangoData dango)
        {
            bool succees = _targetDangoList.Remove(dango);
            Logger.Assert(succees);
        }

        public void SetSpitManager(SpitManager spitManager) => _spitManager = spitManager;
    }
}