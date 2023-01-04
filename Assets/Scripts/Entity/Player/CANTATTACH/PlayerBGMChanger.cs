using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class PlayerBGMChanger
{
    [Header("�s���`BGM�ɂȂ�ۂɎg�p����ϐ�")]
    [SerializeField, Tooltip("BGM��ύX����c�萧������[s]"), Min(0)] float _timeToChangeBGM = default!;
    [SerializeField, Tooltip("�s���`BGM�J�n�܂ł̃N�[���^�C��[s]"), Min(0)] float _coolTimeToStartPinchBGM = default!;

    [Header("�ʏ�BGM�ɖ߂�ۂɎg�p����ϐ�")]
    [SerializeField, Tooltip("�s���`BGM�̃t�F�[�h�A�E�g����[s]"), Min(0)] float _fadeoutTimeForPinchBGM = default!;
    [SerializeField, Tooltip("�ʏ�BGM�J�n�܂ł̃N�[���^�C��[s]"), Min(0)] float _coolTimeToStartNormalBGM = default!;
    [SerializeField, Tooltip("�ʏ�BGM�̃t�F�[�h�C������[s]"), Min(0)] float _fadeinTimeForNormalBGM = default!;

    bool _isPlayingPinchBGM;

    //�ʏ�BGM�𓯂��ʒu����Đ�������̂Ɏg�p
    float _bgmPlayTime;

    //�V���A���C�Y�ȊO�ł̃C���X�^���X�������֎~����
    private PlayerBGMChanger()
    {
    }

    public async void PinchBGMChanger(float currentTimelimit)
    {
        //�ʏ펞
        if (currentTimelimit > _timeToChangeBGM)
        {
            if (!_isPlayingPinchBGM) return;

            _isPlayingPinchBGM = false;

            SoundManager.Instance.StopBGM(_fadeoutTimeForPinchBGM);

            await UniTask.Delay((int)(1000f * (_fadeoutTimeForPinchBGM + _coolTimeToStartNormalBGM)));

            SoundManager.Instance.PlayBGM(SoundSource.BGM1A_STAGE1_Loop, _fadeinTimeForNormalBGM, _bgmPlayTime);
        }
        //�s���`��
        else
        {
            if (_isPlayingPinchBGM) return;

            _isPlayingPinchBGM = true;
            SoundManager.Instance.PlaySE(SoundSource.SE21_PINCHSE, true);

            await UniTask.Delay((int)(1000f * _coolTimeToStartPinchBGM));

            _bgmPlayTime = SoundManager.Instance.BGMLoopTime;

            SoundManager.Instance.PlayBGM(SoundSource.BGM1B_STAGE1_PINCHBGM);
        }
    }
}
