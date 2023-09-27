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

            questManager.Creater.CreateQuestCreateRole(0, new QuestCreateRole.SpecifyTheRole("全天鏡面"), 1, 0, "線対称で団結を作れ", 0f, true, false, new(
                new PortraitTextData.PTextData(0, "しっかり覚えてるな！えらいぞ！", 0f, PortraitTextData.FacePatturn.Happy),
                new(1, "この団結は串の長さに関わらずいつでも作ることが出来る団結だ", 0f, PortraitTextData.FacePatturn.Fun),
                new(2, "組み合わせの数もとても多いから、ずっとお世話になることになるな", 0f, PortraitTextData.FacePatturn.Normal),
                new(3, "それじゃあ、串が伸びた状態でもう一回やってみてくれ", 0f, PortraitTextData.FacePatturn.Fun)),
                1),

            questManager.Creater.CreateQuestCreateRole(1, new QuestCreateRole.SpecifyTheRole("全天鏡面"), 1, 0, "線対称で団結を作れ", 0f, false, true, new(
                new PortraitTextData.PTextData(0, "お見事、もう完璧だな！", 0f, PortraitTextData.FacePatturn.Happy),
                new(1, "全天鏡面は簡単に作れる反面、異なる団結を作らないとならねぇ時には気を付けなきゃならねぇ", 0f, PortraitTextData.FacePatturn.Perplexed),
                new(2, "一つの団結に拘らず、色々な団結を作ってみてくれよな！", 0f, PortraitTextData.FacePatturn.Fun),
                new(3, "それじゃあ、団道完了！", 0f, PortraitTextData.FacePatturn.Fun)),
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
            new(1, "さて、今回覚えてもらう団結は「全天鏡面」だ", 0f, PortraitTextData.FacePatturn.Normal),
            new(2, "基本操作編でも作った線対称の団結だな", 0f, PortraitTextData.FacePatturn.Normal),
            new(3, "早速だが、試しに作ってみてくれ", 0f, PortraitTextData.FacePatturn.Fun)
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