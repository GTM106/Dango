using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class PlayerBGMChanger
{
    [Header("ピンチBGMになる際に使用する変数")]
    [SerializeField, Tooltip("BGMを変更する残り制限時間[s]"), Min(0)] float _timeToChangeBGM = default!;
    [SerializeField, Tooltip("ピンチBGM開始までのクールタイム[s]"), Min(0)] float _coolTimeToStartPinchBGM = default!;

    [Header("通常BGMに戻る際に使用する変数")]
    [SerializeField, Tooltip("ピンチBGMのフェードアウト時間[s]"), Min(0)] float _fadeoutTimeForPinchBGM = default!;
    [SerializeField, Tooltip("通常BGM開始までのクールタイム[s]"), Min(0)] float _coolTimeToStartNormalBGM = default!;
    [SerializeField, Tooltip("通常BGMのフェードイン時間[s]"), Min(0)] float _fadeinTimeForNormalBGM = default!;

    bool _isPlayingPinchBGM;

    //通常BGMを同じ位置から再生させるのに使用
    float _bgmPlayTime;

    //シリアライズ以外でのインスタンス生成を禁止する
    private PlayerBGMChanger()
    {
    }

    public async void PinchBGMChanger(float currentTimelimit)
    {
        //通常時
        if (currentTimelimit > _timeToChangeBGM)
        {
            if (!_isPlayingPinchBGM) return;

            _isPlayingPinchBGM = false;

            SoundManager.Instance.StopBGM(_fadeoutTimeForPinchBGM);

            await UniTask.Delay((int)(1000f * (_fadeoutTimeForPinchBGM + _coolTimeToStartNormalBGM)));

            SoundManager.Instance.PlayBGM(SoundSource.BGM1A_STAGE1_Loop, _fadeinTimeForNormalBGM, _bgmPlayTime);
        }
        //ピンチ時
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
