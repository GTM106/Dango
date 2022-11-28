using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
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

        public void Update(Transform transform)
        {
            if (_targetDangoList.Count == 0) return;

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

                //�T�[�`����p�x���傫���������߂�
                if (targetAngle >= SEARCH_ANGLE) continue;

                float distance = Vector3.Distance(transform.position, _targetDangoList[i].transform.position);

                min = Mathf.Min(min, distance);
                index = i;
            }

            if (index == -1)
            {
                if (_highlightDango != null)
                {
                    _highlightDango.gameObject.SetLayerIncludeChildren(0);
                    _highlightDango = null;
                }
                return;
            }

            //�n�C���C�g�\��
            if (_highlightDango != _targetDangoList[index])
            {
                if (_highlightDango != null) _highlightDango.gameObject.SetLayerIncludeChildren(0);
                _highlightDango = _targetDangoList[index];
                _highlightDango.gameObject.SetLayerIncludeChildren(9);
            }
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

        public async void WasPressedAttackButton(Rigidbody rb)
        {
            if (_highlightDango is null) return;
            _hasStabDango = false;

            float distance = Vector3.Distance(_highlightDango.transform.position, rb.transform.position);

            //�������Ɍ�����
            rb.transform.LookAt(_highlightDango.transform.position.SetY(rb.position.y));

            //�O�i(����/���Ԃ�������Ƃ���Ηǂ�)
            while (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1f)
            {
                await UniTask.WaitForFixedUpdate();

                //�ːi���Ɏh��������}��~
                if (_hasStabDango)
                {
                    _hasStabDango = false;
                    rb.velocity = Vector3.zero;
                    break;
                }

                //��ւ̉^����f���؂肽�����ߒ��ڏ�������
                rb.velocity = (rb.transform.forward.normalized * distance / RUSH_TIME).SetY(0);
            }

            //�^�[�Q�b�g�ꗗ���獡�̒c�q�������B(��������ĕʂ̒c�q���h�������Ƃ��ɂ��̒c�q���ēx�^�[�Q�b�g����ɂ���)
            _targetDangoList.Remove(_highlightDango);
        }

        public void SetHasStabDango()
        {
            _hasStabDango = true;
        }
    }
}