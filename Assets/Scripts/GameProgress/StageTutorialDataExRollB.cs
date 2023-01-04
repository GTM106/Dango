using Dango.Quest;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StageTutorialDataExRollB : StageData
{
    static readonly List<DangoColor> stageDangoColors = new() { DangoColor.Beni, DangoColor.Yuzu, DangoColor.Yomogi };

    protected override void Start()
    {
        base.Start();
        SoundManager.Instance.StopBGM();
        SoundManager.Instance.PlayBGM(SoundSource.BGM1C_TUTORIAL);
    }

    protected override void AddQuest()
    {
        QuestManager questManager = QuestManager.Instance;

        List<QuestData> quest = new()
        {

            questManager.Creater.CreateQuestDestination(0, FloorManager.Floor.Max, false, "初心者指南完了！", 0f, false, true,new(
                new PortraitTextData.PTextData()),
                0),
        };

        QuestData.Clear();
        QuestData.AddRange(quest);
        questManager.ChangeQuest(quest[0]);
    }

    protected override PortraitTextData StartPortraitText()
    {
        return new(new(0, "まだ準備中だぜ！", 0f, PortraitTextData.FacePatturn.Normal),
            new(0, "また来てくれよな！", 0f, PortraitTextData.FacePatturn.Happy));
    }

    public override List<DangoColor> FloorDangoColors()
    {
        return stageDangoColors;
    }

    public override void OnStageSucceed()
    {
        Release(Stage.Stage1);
    }
}