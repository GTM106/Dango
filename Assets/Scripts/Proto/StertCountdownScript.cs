using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using TM.Easing.Management;

public class StertCountdownScript : MonoBehaviour
{
    [SerializeField] ImageUIData _countDownImage;
    [SerializeField] Sprite[] words;

    const float MAXTIME = 1f;

    private void Start()
    {
        PlayerData.Event = true;
        StartCountDown();
    }

    private async void StartCountDown()
    {
        Vector2 sizeDelta = new(_countDownImage.ImageData.GetWidth(), _countDownImage.ImageData.GetHeight());

        for (int i = 0; i < words.Length; i++)
        {
            _countDownImage.ImageData.SetSprite(words[i]);

            float currentTime = 0;

            if (i == words.Length - 1)
            {
                SoundManager.Instance.PlaySE(SoundSource.SE6_CREATE_ROLE_CHARACTER_ANIMATION);
                PlayerData.Event = false;

                _countDownImage.ImageData.Fadeout(0.5f, 0.3f).Forget();

                while (currentTime < 1f)
                {
                    currentTime += Time.deltaTime;

                    float t = EasingManager.EaseProgress(TM.Easing.EaseType.OutQuart, currentTime, MAXTIME, 0, 0);

                    _countDownImage.ImageData.SetSizeDelta(sizeDelta *  t);

                    await UniTask.Yield();
                }
            }
            else
            {
                _countDownImage.ImageData.SetAlpha(0);
                _countDownImage.ImageData.Fadein(0.1f).Forget();
                _countDownImage.ImageData.Fadeout(0.2f, 0.7f).Forget();

                while (currentTime < 1f)
                {
                    currentTime += Time.deltaTime;

                    float t = EasingManager.EaseProgress(TM.Easing.EaseType.InQuart, currentTime, MAXTIME, 0, 0);

                    _countDownImage.ImageData.SetSizeDelta(sizeDelta * (1 - t));

                    await UniTask.Yield();
                }
            }
        }

        _countDownImage.ImageData.SetImageEnabled(false);
    }
}
