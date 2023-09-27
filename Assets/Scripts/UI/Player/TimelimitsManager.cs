using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimelimitsManager : MonoBehaviour
{
    [SerializeField] List<ImageUIData> _timelimits;
    [SerializeField] TextUIData _timeText;
    [SerializeField] List<float> _timelimit;
    [SerializeField] PlayerData _playerData;

    [SerializeField] TextUIData _alwaysTimeText;
    [SerializeField] bool _isEX; 

    const float OFFSET = 50f;

    bool _isDirecting;

    private void Awake()
    {
        _timeText.TextData.SetAlpha(0);
    }

    private void Update()
    {
        float currentTimelimit = _playerData.GetSatiety();

        if (_isEX)
        {
            _alwaysTimeText.TextData.SetText("" + (int)currentTimelimit);
        }

        for (int i = 0; i < _timelimits.Count; i++)
        {
            if (currentTimelimit > _timelimit[i])
            {
                float diff = currentTimelimit - _timelimit[i];

                if (i == _timelimit.Count - 1)
                {
                    _timelimits[i].ImageData.SetFillAmount(diff / _timelimit[i]);
                }
                else
                {
                    _timelimits[i].ImageData.SetFillAmount(diff / (_timelimit[i] - _timelimit[i + 1]));
                }

                for (int j = i + 1; j < _timelimits.Count; j++)
                {
                    _timelimits[j].ImageData.SetFillAmount(1f);
                }

                break;
            }
            else
            {
                _timelimits[i].ImageData.SetFillAmount(0);
            }
        }
    }


    public async void AddTimelimit(float time)
    {
        //0ˆÈ‰º‚È‚ç“®ì‚µ‚È‚¢
        if (time <= 0) return;

        while (_isDirecting) await UniTask.Yield();

        _isDirecting = true;

        _timeText.TextData.SetAlpha(1f);
        _timeText.TextData.SetText("+" + time);
        _timeText.TextData.Fadeout(1f).Forget();

        float currentTime = 0;

        while (currentTime < 1f)
        {
            await UniTask.Yield();
            currentTime += Time.deltaTime;

            _timeText.TextData.SetPosition(new(0, 144f + OFFSET * currentTime, 0));
        }

        _isDirecting = false;
    }
}