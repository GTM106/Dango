using Dango.Quest;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StageTutorialDataEx : StageData
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
            questManager.Creater.CreateQuestCreateRole(0, new QuestCreateRole.SpecifyTheRole("全天鏡面"), 1, 0, "線対称で団結を作れ", 0f,true, false, new(
                new PortraitTextData.PTextData(0, "よし！ばっちりだな！", 0f, PortraitTextData.FacePatturn.Happy),
                new(1, "団道を達成すると、串の長さが伸びることがある", 0f, PortraitTextData.FacePatturn.Normal),
                new(2, "串が伸びると、高跳びアクションの上昇量が増えるぜ", 0f, PortraitTextData.FacePatturn.Normal),
                new(3, "これで行けなかった場所に行けるようになったりするんだ", 0f, PortraitTextData.FacePatturn.Fun),
                new(4, "ただ……団結の作り方も変わって難しくなるから一長一短だ", 0f, PortraitTextData.FacePatturn.Perplexed),
                new(5, "さて、高く跳べるようになったなら「急降下刺し」を教えておかないとな", 0f, PortraitTextData.FacePatturn.Normal),
                new(6, "やり方は簡単、空中で突き刺しアクションをするだけだ", 0f, PortraitTextData.FacePatturn.Normal),
                new(7, "これで団子を刺してみてくれ！", 0f, PortraitTextData.FacePatturn.Fun)),
            1),

            questManager.Creater.CreateQuestPlayAction(1, QuestPlayAction.PlayerAction.FallAttack, 1, "急降下刺しで1回刺す", 0f, true, false, new(
                new PortraitTextData.PTextData(0, "やるねぇ！", 0f, PortraitTextData.FacePatturn.Fun),
                new(1, "これが使いこなせれば、より効率的に団子を刺すことが出来るぜ", 0f, PortraitTextData.FacePatturn.Normal),
                new(2, "じゃあ次はちょっとした小技の紹介だ", 0f, PortraitTextData.FacePatturn.Normal),
                new(3, "団子外しアクションは便利だが……外すのに少し時間が掛かるのが難点だよな", 0f, PortraitTextData.FacePatturn.Perplexed),
                new(4, "だが、空中で団子外しアクションをすると即座に団子を外すことが出来る", 0f, PortraitTextData.FacePatturn.Normal),
                new(5, "試しにやってみてくれ、空中で団子外しアクションだ", 0f, PortraitTextData.FacePatturn.Fun)),
            2),

            questManager.Creater.CreateQuestPlayAction(2, QuestPlayAction.PlayerAction.RemoveInTheAir, 3, "空中で3回団子外しアクションを行う", 0f, false, true, new(
                new PortraitTextData.PTextData(0, "見事！完璧にマスターしたな！", 0f, PortraitTextData.FacePatturn.Happy),
                new(1, "空中団子外しアクションは少しだけ浮遊する効果がある", 0f, PortraitTextData.FacePatturn.Normal),
                new(2, "これを使えば普通の高跳びじゃ届かない場所に移動できたりするかもな", 0f, PortraitTextData.FacePatturn.Normal),
                new(3, "さて、応用操作編は以上だ", 0f, PortraitTextData.FacePatturn.Fun),
                new(4, "他にも色々な団結の作り方の初心者指南もあるから不安な人はやっておくといいぜ", 0f, PortraitTextData.FacePatturn.Normal),
                new(5, "それじゃあ以上！団道完了だ！", 0f, PortraitTextData.FacePatturn.Happy)),
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
        return new(new(0, "初心者指南、応用操作編だぜ", 0f, PortraitTextData.FacePatturn.Normal),
            new(0, "これをマスターすれば団道の操作はばっちりだ！気張ってこうぜ！", 0f, PortraitTextData.FacePatturn.Happy),
            new(0, "まずはおさらいだ、線対称で並べて『全天鏡面』を作ってみてくれ", 0f, PortraitTextData.FacePatturn.Normal)
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