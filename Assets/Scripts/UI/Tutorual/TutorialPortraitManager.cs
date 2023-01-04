using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using TM.Easing.Management;
using UnityEngine;
using UnityEngine.UI;

//基本のポートレート表示と概ね同じです。
//しかし、チュートリアル限定処理をいちいち判定するのは冗長と判断したため
//チュートリアル専用のスクリプトとして独立させました。
public class TutorialPortraitManager : MonoBehaviour, IChangePortrait
{
    [SerializeField] Image img;
    [SerializeField] TextUIData text;
    [SerializeField] Sprite[] facePatterns;
    [SerializeField] ImageUIData U11Image;
    [SerializeField] ImageUIData U8Image;

    bool _isChangePortrait;

    const float OFFSET = -1750f;
    const float SLIDETIME = 0.5f;

    //transformのインスタンス取得
    Transform _transform;

    public bool IsChangePortrait => _isChangePortrait;

    private void Awake()
    {
        _transform = transform;
        _transform.localPosition = Vector3.zero.SetX(OFFSET);

        U11Image.ImageData.FlashAlpha(-1f, 0.7f, 0);
    }

    private void ChangePortrait(PortraitTextData.PTextData data)
    {
        if (string.IsNullOrEmpty(data.text))
        {
            text.TextData.SetText();
            return;
        }

        text.TextData.SetText(data.text);
        img.sprite = facePatterns[(int)data.face];
    }

    private async UniTask SlideIn()
    {
        float time = 0;
        try
        {
            //位置の初期化
            _transform.localPosition = Vector3.zero.SetX(OFFSET);

            while (time < SLIDETIME)
            {
                await UniTask.Yield();
                if (!_transform.root.gameObject.activeSelf) continue;

                time += Time.deltaTime;

                _transform.localPosition = Vector3.zero.SetX(OFFSET * (1f - EasingManager.EaseProgress(TM.Easing.EaseType.OutQuart, time, SLIDETIME, 0f, 0)));
            }
        }
        catch (MissingReferenceException)
        {
            return;
        }

        _transform.localPosition = Vector3.zero;
    }

    private async UniTask SlideOut()
    {
        float time = 0;

        //位置の初期化
        _transform.localPosition = Vector3.zero;

        try
        {
            while (time < SLIDETIME)
            {
                await UniTask.Yield();
                if (!_transform.root.gameObject.activeSelf) continue;

                time += Time.deltaTime;

                _transform.localPosition = Vector3.zero.SetX(OFFSET * EasingManager.EaseProgress(TM.Easing.EaseType.InQuart, time, SLIDETIME, 0f, 0));
            }
        }
        catch (MissingReferenceException)
        {
            return;
        }

        _transform.localPosition = Vector3.zero.SetX(OFFSET);
    }

    public async UniTask ChangePortraitText(PortraitTextData questTextData)
    {
        PlayerData.IsClear = false;

        //イベント進行終了まで待機
        while (_isChangePortrait) await UniTask.Yield();

        if (questTextData.TextDataIndex == 0) return;

        //データ初期設定
        ChangePortrait(questTextData.GetQTextData(0));

        //チュートリアルは文字を読ませたいためUIマップに変える
        InputSystemManager.Instance.Input.SwitchCurrentActionMap("UI");

        U8Image.ImageData.SetImageEnabled(false);

        await SlideIn();

        //進行中フラグをオンにする
        _isChangePortrait = true;

        //イベント進行
        for (int i = 0; i < questTextData.TextDataIndex; i++)
        {
            //データの変更
            ChangePortrait(questTextData.GetQTextData(i));

            await UniTask.DelayFrame(50, PlayerLoopTiming.FixedUpdate);

            try
            {
                while (!InputSystemManager.Instance.IsPressChoice)
                {
                    await UniTask.Yield();
                    if (!_transform.root.gameObject.activeSelf) continue;
                }
            }
            catch (MissingReferenceException)
            {
                return;
            }
        }

        //アクションマップをもとに戻す
        InputSystemManager.Instance.Input.SwitchCurrentActionMap("Player");
        U8Image.ImageData.SetImageEnabled(true);

        await SlideOut();

        //進行中フラグをオフにする
        _isChangePortrait = false;
        PlayerData.IsClear = true;
        Logger.Log(PlayerData.IsClear);
    }

}
