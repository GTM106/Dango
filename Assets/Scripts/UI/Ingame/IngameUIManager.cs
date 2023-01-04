using Cysharp.Threading.Tasks;
using Dango.Quest.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class IngameUIManager : MonoBehaviour
{
    [SerializeField] CanvasGroup[] _canvasGroups;
    [SerializeField] ImageUIData[] _imageUIDatas;

    bool _duringEndProduction = false;

    const float FADETIME = 0.1f;
    const float WAITTIME = 0.6f;

    public bool DuringEndProduction => _duringEndProduction;

    private void Start()
    {
        foreach (var image in _imageUIDatas)
        {
            image.ImageData.SetAlpha(0);
        }
    }

    public async UniTask EraseUIs()
    {
        _duringEndProduction = true;

        float alpha = 1;

        while (alpha > 0)
        {
            await UniTask.Yield();
            alpha -= Time.deltaTime;
            alpha = Mathf.Max(alpha, 0);

            foreach (var canvasGroup in _canvasGroups)
            {
                canvasGroup.alpha = alpha;
            }
        }
    }

    public async UniTask TextAnimation()
    {
        await _imageUIDatas[0].ImageData.Fadein(FADETIME, WAITTIME);
        await _imageUIDatas[1].ImageData.Fadein(FADETIME, WAITTIME);
        await _imageUIDatas[2].ImageData.Fadein(FADETIME, WAITTIME);
        await _imageUIDatas[3].ImageData.Fadein(FADETIME, WAITTIME);
    }
}