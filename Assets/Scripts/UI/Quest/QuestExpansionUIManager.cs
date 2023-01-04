using Cysharp.Threading.Tasks;
using Dango.Quest;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestExpansionUIManager : MonoBehaviour
{
    [SerializeField] ImageUIData[] _currentQuests;
    [SerializeField] TextUIData[] _currentQuestTexts;

    [SerializeField] Sprite[] _currentQuestSprites;

    [SerializeField] ImageUIData _firstgaugeMask;
    [SerializeField] ImageUIData _secondGaugeMask;
    [SerializeField] ImageUIData[] _firstGaugeLines;
    [SerializeField] ImageUIData[] _secondGaugeLines;

    [SerializeField] Canvas _canvas;

    QuestData _questData1;
    QuestData _questData2;

    private void Awake()
    {
        _canvas.enabled = false;
    }

    private void Start()
    {
        InputSystemManager.Instance.onExpansionUIPerformed += OnExpansionPerformed;
        InputSystemManager.Instance.onExpansionUICanceled += OnExpansionCanceled;
        ChangeQuest();
    }

    private void OnDestroy()
    {
        InputSystemManager.Instance.onExpansionUIPerformed -= OnExpansionPerformed;
        InputSystemManager.Instance.onExpansionUICanceled -= OnExpansionCanceled;
    }

    public void ChangeQuest()
    {
        if (this == null) return;

        //進行度の初期化。最初は0だと決め打っています
        _firstgaugeMask.ImageData.SetFillAmount(0);
        _secondGaugeMask.ImageData.SetFillAmount(0);

        _questData1 = QuestManager.Instance.GetQuest(0);

        SetLine(_questData1, _firstGaugeLines);

        _currentQuestTexts[0].TextData.SetText(_questData1.QuestName);

        _questData2 = QuestManager.Instance.GetQuest(1);

        if (_questData2 == null)
        {
            _currentQuests[1].gameObject.SetActive(false);

            _currentQuests[0].ImageData.SetSprite(_currentQuestSprites[(int)_questData1.QuestType]);
            _currentQuests[0].ImageData.SetPositionY(-60f);
        }
        else
        {
            SetLine(_questData2, _secondGaugeLines);

            _currentQuestTexts[1].TextData.SetText(_questData2.QuestName);
            _currentQuests[1].gameObject.SetActive(true);

            _currentQuests[0].ImageData.SetSprite(_currentQuestSprites[(int)_questData1.QuestType]);
            _currentQuests[1].ImageData.SetSprite(_currentQuestSprites[(int)_questData2.QuestType]);

            _currentQuests[0].ImageData.SetPositionY(-180f);
            _currentQuests[1].ImageData.SetPositionY(60f);
        }
    }

    private void SetLine(QuestData quest, ImageUIData[] gaugeLines)
    {
        foreach (var line in gaugeLines)
        {
            line.ImageData.SetImageEnabled(false);
        }

        //予め計算した値の位置に中間ラインを配置
        if (quest is QuestEatDango questEa)
        {
            switch (questEa.ContinueCount <= 1 ? questEa.SpecifyCount : questEa.ContinueCount)
            {
                case 2:
                    SetLineSpCountIsTwo(gaugeLines);
                    break;
                case 3:
                    SetLineSpCountIsThree(gaugeLines);
                    break;
                case 4:
                    SetLineSpCountIsFour(gaugeLines);
                    break;
                default: break;
            }
        }
        else if (quest is QuestPlayAction questPa)
        {
            switch (questPa.SpecifyCount)
            {
                case 2:
                    SetLineSpCountIsTwo(gaugeLines);
                    break;
                case 3:
                    SetLineSpCountIsThree(gaugeLines);
                    break;
                case 4:
                    SetLineSpCountIsFour(gaugeLines);
                    break;
                default: break;
            }
        }
        else if (quest is QuestCreateRole questCr)
        {
            switch (questCr.ContinueCount <= 1 ? questCr.SpecifyCount : questCr.ContinueCount)
            {
                case 2:
                    SetLineSpCountIsTwo(gaugeLines);
                    break;
                case 3:
                    SetLineSpCountIsThree(gaugeLines);
                    break;
                case 4:
                    SetLineSpCountIsFour(gaugeLines);
                    break;
                default: break;
            }
        }
        else if (quest is QuestDestination questDe)
        {
            //DoNothing...
        }
    }

    private void SetLineSpCountIsTwo(ImageUIData[] gaugeLines)
    {
        gaugeLines[0].ImageData.SetImageEnabled(true);
    }
    private void SetLineSpCountIsThree(ImageUIData[] gaugeLines)
    {
        gaugeLines[1].ImageData.SetImageEnabled(true);
        gaugeLines[1].ImageData.SetPositionX(-86.66666f);
        gaugeLines[2].ImageData.SetImageEnabled(true);
        gaugeLines[2].ImageData.SetPositionX(86.66666f);
    }
    private void SetLineSpCountIsFour(ImageUIData[] gaugeLines)
    {
        gaugeLines[0].ImageData.SetImageEnabled(true);
        gaugeLines[1].ImageData.SetImageEnabled(true);
        gaugeLines[1].ImageData.SetPositionX(-130f);
        gaugeLines[2].ImageData.SetImageEnabled(true);
        gaugeLines[2].ImageData.SetPositionX(130f);
    }

    public void OnNext(QuestData questData,float progress)
    {
        //どちらかと一致しているか確認
        Logger.Assert(questData == _questData1 || questData == _questData2);

        SoundManager.Instance.PlaySE(SoundSource.SE13_ATTACK);

        //float progress = 0;

        //if (questData is QuestEatDango questEa)
        //{
        //    progress = questEa.Progress();
        //}
        //else if (questData is QuestPlayAction questPa)
        //{
        //    progress = questPa.Progress();
        //}
        //else if (questData is QuestCreateRole questCr)
        //{
        //    progress = questCr.Progress();
        //}
        //else if (questData is QuestDestination questDe)
        //{
        //    progress = questDe.Progress();
        //}

        Logger.Assert(progress is >= 0 and <= 1f);

        //一致していた方のゲージを進める
        ImageUIData gaugeMask = questData == _questData1 ? _firstgaugeMask : _secondGaugeMask;

        gaugeMask.ImageData.SetFillAmount(progress);
    }

    private void OnExpansionPerformed()
    {
        if (PlayerData.Event) return;
        _canvas.enabled = true;
    }
    private void OnExpansionCanceled()
    {
        _canvas.enabled = false;
    }
}
