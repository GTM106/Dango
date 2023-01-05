using Dango.Quest;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StageTutorialDataExRollE : StageData
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
            questManager.Creater.CreateQuestCreateRole(0, new QuestCreateRole.SpecifyTheRole("三面華鏡"), 1, 0, "二分割で団結を作れ", 0f, true, false, new(
                new PortraitTextData.PTextData(0, "最高だぜ！完璧だ！", 0f, PortraitTextData.FacePatturn.Happy),
                new(1, "この団結は串の長さが「6個」の時だけ作れる", 0f, PortraitTextData.FacePatturn.Fun),
                new(2, "とても珍しく、作るのが難しい役だ", 0f, PortraitTextData.FacePatturn.Perplexed),
                new(3, "けど、しっかり作り方を覚えてくれよな", 0f, PortraitTextData.FacePatturn.Normal),
                new(4, "さて、これで初心者指南は全て完了だ", 0f, PortraitTextData.FacePatturn.Normal),
                new(5, "ここまで来れたお前はもう団道マスターだな！", 0f, PortraitTextData.FacePatturn.Fun),
                new(6, "それじゃあ名残惜しいが、続きはステージで頼むぜ！", 0f, PortraitTextData.FacePatturn.Fun),
                new(7, "団道完了！", 0f, PortraitTextData.FacePatturn.Happy)),
                1),

            questManager.Creater.CreateQuestDestination(1, FloorManager.Floor.Max, false, "初心者指南完了！", 0f, false, true,new(
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
            new(1, "さて、最後に覚えてもらう団結は「三面華鏡」だ", 0f, PortraitTextData.FacePatturn.Normal),
            new(2, "団子を三分割に分けて並べる団結だ", 0f, PortraitTextData.FacePatturn.Normal),
            new(3, "例えば「紅団子」「紅団子」「よもぎ団子」「よもぎ団子」「ゆず団子」「ゆず団子」って感じだな", 0f, PortraitTextData.FacePatturn.Normal),
            new(4, "かなり難しいと思うが、チャレンジしてみてくれ", 0f, PortraitTextData.FacePatturn.Perplexed),
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