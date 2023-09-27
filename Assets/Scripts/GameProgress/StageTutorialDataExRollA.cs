using Dango.Quest;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StageTutorialDataExRollA: StageData
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
            questManager.Creater.CreateQuestCreateRole(0, new QuestCreateRole.SpecifyTheRole("一統団結"), 1, 0, "同じ団子だけで団結を作れ", 0f, true, false, new(
                new PortraitTextData.PTextData(0, "オーケーだ！それが一統団結だ", 0f, PortraitTextData.FacePatturn.Happy),
                new(1, "この団結は作るのが難しい分、他の団結よりも腹が膨れるのが特徴だ", 0f, PortraitTextData.FacePatturn.Fun),
                new(2, "それじゃあ練習でもう一度作ってみてくれ、今度は団子4つだ", 0f, PortraitTextData.FacePatturn.Normal)),
                1),

            questManager.Creater.CreateQuestCreateRole(1, new QuestCreateRole.SpecifyTheRole("一統団結"), 1, 0, "同じ団子だけで団結を作れ", 0f, false, true, new(
                new PortraitTextData.PTextData(0, "上出来だ、一統団結はマスターしたな", 0f, PortraitTextData.FacePatturn.Fun),
                new(1, "この団結は基本中の基本、しっかり覚えていってくれよな", 0f, PortraitTextData.FacePatturn.Normal),
                new(2, "それじゃあ、団道完了！", 0f, PortraitTextData.FacePatturn.Fun)),
                2),

            questManager.Creater.CreateQuestDestination(2, FloorManager.Floor.Max, false, "初心者指南完了！", 0f, false, true,new(
                new PortraitTextData.PTextData()),
                0),
        };

        QuestData.Clear();
        QuestData.AddRange(quest);
        questManager.ChangeQuest(quest[0]);
    }

    protected override PortraitTextData StartPortraitText()
    {
        return new(new(0, "初心者指南、団結作り編だぜ！", 0f, PortraitTextData.FacePatturn.Fun),
            new(1, "さて、今回覚えてもらう団結は「一統団結」だ", 0f, PortraitTextData.FacePatturn.Normal),
            new(2, "作り方は簡単、同じ団子だけを刺して集めるんだ", 0f, PortraitTextData.FacePatturn.Normal),
            new(3, "それじゃあ早速やってみてくれ！", 0f, PortraitTextData.FacePatturn.Fun)
            );
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