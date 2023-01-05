using Dango.Quest;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StageTutorialDataExRollD: StageData
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

            questManager.Creater.CreateQuestCreateRole(0, new QuestCreateRole.SpecifyTheRole("隣色鏡面"), 1, 0, "二分割で団結を作れ", 0f, true, false, new(
                new PortraitTextData.PTextData(0, "見事！ちゃんと作れてるな！", 0f, PortraitTextData.FacePatturn.Happy),
                new(1, "この団結は串の長さが「4個」と「6個」の時だけ作れる", 0f, PortraitTextData.FacePatturn.Fun),
                new(2, "そこは輪廻転生と同じだが、それよりも作るのは難しい", 0f, PortraitTextData.FacePatturn.Perplexed),
                new(3, "それじゃあ、次の練習のために一度串を伸ばすぞ", 0f, PortraitTextData.FacePatturn.Normal),
                new(3, "適当に団子を刺すか、L+Rでスキップしてくれ", 0f, PortraitTextData.FacePatturn.Fun)),
                1),

            questManager.Creater.CreateQuestPlayAction(1, QuestPlayAction.PlayerAction.Stab, 1, "団子を刺す", 0f, true, false, new(
                new PortraitTextData.PTextData(0, "串が伸びたな、それじゃあもう一度「輪廻転生」を作ってみてくれ", 0f, PortraitTextData.FacePatturn.Fun)),
                2),

            questManager.Creater.CreateQuestCreateRole(2, new QuestCreateRole.SpecifyTheRole("隣色鏡面"), 1, 0, "二分割で団結を作れ", 0f, false, true, new(
                new PortraitTextData.PTextData(0, "素晴らしい！二分割の「隣色鏡面」もマスターしたな！", 0f, PortraitTextData.FacePatturn.Happy),
                new(1, "繰り返すが串の長さが「3個」「5個」「7個」の時は作れないから気を付けてくれ", 0f, PortraitTextData.FacePatturn.Perplexed),
                new(2, "それじゃあ、団道完了！", 0f, PortraitTextData.FacePatturn.Fun)),
                3),

            questManager.Creater.CreateQuestDestination(3, FloorManager.Floor.Max, false, "初心者指南完了！", 0f, false, true,new(
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
            new(1, "さて、今回覚えてもらう団結は「隣色鏡面」だ", 0f, PortraitTextData.FacePatturn.Normal),
            new(2, "団子を二分割に分けて並べる団結だ", 0f, PortraitTextData.FacePatturn.Normal),
            new(3, "例えば「紅団子」「紅団子」「よもぎ団子」「よもぎ団子」って感じだな", 0f, PortraitTextData.FacePatturn.Normal),
            new(4, "全天鏡面こと線対称とよく似ているから注意してくれ", 0f, PortraitTextData.FacePatturn.Perplexed),
            new(5, "それじゃあ早速作ってみよう！", 0f, PortraitTextData.FacePatturn.Fun)
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