using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TM.Entity.Player
{
    class PlayerRemoveDango
    {
        List<DangoColor> _dangos;
        DangoUIScript _dangoUIScript;
        PlayerData _playerData;
        Animator _animator;

        const float ANIMATION_TIME = 1f;
        
        //���V��
        const float FLOATING_POWER = 3f;

        bool _isPressFire;
        bool _isCoolDown;

        public bool IsCoolDown => _isCoolDown;

        public PlayerRemoveDango(List<DangoColor> dangos, DangoUIScript dangoUIScript, PlayerData playerData, Animator animator)
        {
            _dangos = dangos;
            _dangoUIScript = dangoUIScript;
            _playerData = playerData;
            _animator = animator;
        }

        //�c�q�e(���O��)
        public async void Remove()
        {
            //���ɉ����Ȃ���������s���Ȃ��B
            if (_dangos.Count == 0) return;

            await CoolTime();

            _isCoolDown = false;

            if (!_isPressFire) return;

            //AN8B�Đ�
            _animator.CrossFade("DangoRemoving", 0f);

            //[Debug]�������������킩����
            //���܂ł́Adangos[dangos.Count - 1]�Ƃ��Ȃ���΂Ȃ�܂���ł������A
            //C#8.0�ȍ~�ł͈ȉ��̂悤�ɏȗ��ł���悤�ł��B
            //���́A�����m��Ȃ��l���ǂނƂ킯��������Ȃ��B
            Logger.Log(_dangos[^1]);

            //SE
            SoundManager.Instance.PlaySE(SoundSource.SE9_REMOVE_DANGO);

            //���������B
            _dangos.RemoveAt(_dangos.Count - 1);

            //UI�X�V
            _dangoUIScript.DangoUISet(_dangos);
        }

        private async UniTask CoolTime()
        {
            _isPressFire = InputSystemManager.Instance.IsPressFire;

            if (!_playerData.IsGround)
            {
                //�󒆎��ɍs������
                _playerData.Rb.velocity = _playerData.Rb.velocity.SetY(FLOATING_POWER);

                return;
            }

            _isCoolDown = true;

            //AN8A�Đ�
            _animator.SetTrigger("IsPressFireTrigger");

            float time = ANIMATION_TIME;

            while (time > 0)
            {
                if (!InputSystemManager.Instance.IsPressFire)
                {
                    _isPressFire = InputSystemManager.Instance.IsPressFire;
                    _animator.SetTrigger("IsReleaseFireTrigger");
                    return;
                }

                await UniTask.Yield();

                time -= Time.deltaTime;
            }
        }
    }
}