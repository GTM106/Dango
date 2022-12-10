using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TM.Entity.Player
{
    public class PlayerFallAction
    {
        System.Random _rand = new();

        //�����A�N�V�����萔
        const int FALLACTION_STAY_AIR_FRAME = 10;
        const int FALLACTION_FALL_POWER = 30;
        const int FALLACTION_MOVE_POWER = 10;

        bool _isFallAction = false;

        int _currentTime = FALLACTION_STAY_AIR_FRAME;

        int _mapLayer;

        Action _onFall;
        Action _onFallExit;

        AnimationManager _animationManager;

        CapsuleCollider _collider;

        public PlayerFallAction(CapsuleCollider collider, Action onFall, Action onFallExit, AnimationManager animationManager, int mapLayer)
        {
            _collider = collider;
            _onFall = onFall;
            _onFallExit = onFallExit;
            _animationManager = animationManager;
            _mapLayer = mapLayer;
        }

        public bool ToFallAction(Vector3 playerPos, bool isGround)
        {
            if (isGround) return false;

            Ray ray = new(playerPos, Vector3.down);

            //�߂��ɒn�ʂ����邩(player�̔����̑傫��)����
            return !Physics.Raycast(ray, _collider.height + _collider.height / 2f, 1 << _mapLayer);
        }

        public bool FixedUpdate(Rigidbody rigidbody, SpitManager spitManager)
        {
            if (!IsFallAction) return false;

            _animationManager.ChangeAnimation(AnimationManager.E_Animation.An7A_FallAction, 0.01f);

            if (--_currentTime > 0)
            {
                rigidbody.velocity = rigidbody.velocity.SetX(rigidbody.velocity.x / FALLACTION_MOVE_POWER).SetY(0).SetZ(rigidbody.velocity.z / FALLACTION_MOVE_POWER);
                return false;
            }

            //SE�Đ�
            SoundManager.Instance.PlaySE(_rand.Next((int)SoundSource.VOISE_PRINCE_FALL01, (int)SoundSource.VOISE_PRINCE_FALL02 + 1));
            SoundManager.Instance.PlaySE(SoundSource.SE10_FALLACTION);

            //�����h������(���ʖ����ŗ͂�������)
            rigidbody.AddForce(Vector3.down * FALLACTION_FALL_POWER, ForceMode.VelocityChange);
            spitManager.IsSticking = true;

            return true;
        }

        public bool IsFallAction
        {
            get => _isFallAction;
            set
            {
                if (!value)
                {
                    _onFallExit?.Invoke();
                    _currentTime = FALLACTION_STAY_AIR_FRAME;
                }
                else
                {
                    _onFall?.Invoke();
                }

                _isFallAction = value;
            }
        }
    }
}