using Dango.Quest;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Stage003Data : StageData
{
    static readonly List<DangoColor> stageDangoColors = new() { DangoColor.An, DangoColor.Beni, DangoColor.Mitarashi, DangoColor.Nori, DangoColor.Yuzu, DangoColor.Yomogi };

    protected override void Start()
    {
        base.Start();
        SoundManager.Instance.StopBGM();
        SoundManager.Instance.PlayBGM(SoundSource.BGM1A_STAGE1_Intro, SoundSource.BGM1A_STAGE1_Loop);
    }

    protected override void AddQuest()
    {
        QuestManager questManager = QuestManager.Instance;

        List<QuestData> quest = new()
        {
            questManager.Creater.CreateQuestCreateRole(0, new QuestCreateRole.CreateSameRole(false), 1, 4, "異なる団結を4回連続で作る", 0f, false, false, new(
                new PortraitTextData.PTextData(0, "すごいぞ！色々な団結を作れるようになったな", 5f, PortraitTextData.FacePatturn.Fun)),
                1,2),

            questManager.Creater.CreateQuestPlayAction(1, QuestPlayAction.PlayerAction.FallAttack, 2, "急降下刺しで2回刺す", 0f, false, false, new(
                new PortraitTextData.PTextData(0, "少し遠回りするぐらいが丁度いい", 5f, PortraitTextData.FacePatturn.Normal),
                new(1, "それが俺の団道、流儀ってもんだ！", 5f, PortraitTextData.FacePatturn.Fun)),
                3),
            questManager.Creater.CreateQuestDestination(2, FloorManager.Floor.floor8, false, "上に向かえ", 0f, false, false, new(
                new PortraitTextData.PTextData(0, "どんどん行くぜ、目指せ最上階！", 5f, PortraitTextData.FacePatturn.Fun)),
                3),

            //D5上昇
            questManager.Creater.CreateQuestCreateRole(3, new QuestCreateRole.SpecifyTheRole("一統団結"), 1, 0, "一種類だけで団結を作れ", 30f, true, false, new(
                new PortraitTextData.PTextData(0, "一意専心。ただ真っ直ぐに、俺の道を征く！", 5f, PortraitTextData.FacePatturn.Fun)),
                4),

            //D5上昇
            questManager.Creater.CreateQuestCreateRole(4, new QuestCreateRole.EstablishRole(true, false, DangoColor.An), 3, 0, "あん団子を含んで団結を3回作れ", 0f, true, false, new(
                new PortraitTextData.PTextData(0, "あん団子で団結を作るのも慣れてきたな", 5f, PortraitTextData.FacePatturn.Normal)),
                5),

            questManager.Creater.CreateQuestDestination(5, FloorManager.Floor.floor12, false, "城内の上層に向かえ", 0f, false, false, new(
                new PortraitTextData.PTextData(0, "あと少しで最上階！待ってろよ伝説の団子！", 5f, PortraitTextData.FacePatturn.Happy)),
                6,7),

            questManager.Creater.CreateQuestCreateRole(6, new QuestCreateRole.CreateSameRole(true), 1, 2, "同じ団結を2回連続で作る", 20f, false, false, new(
                new PortraitTextData.PTextData(0, "見つけたぜ、自分の団結ってやつを", 5f, PortraitTextData.FacePatturn.Fun)),
                8),
            questManager.Creater.CreateQuestCreateRole(7, new QuestCreateRole.CreateSameRole(false), 1, 2, "異なる団結を2回連続で作る", 20f, false, false, new(
                new PortraitTextData.PTextData(0, "変化のない人生なんてつまらねぇよな！", 5f, PortraitTextData.FacePatturn.Happy)),
                8),

            //D5上昇
            questManager.Creater.CreateQuestCreateRole(8, new QuestCreateRole.SpecifyTheRole("三面華鏡"), 2, 0, "三分割で団結を2回作れ", 0f, true, false, new(
                new PortraitTextData.PTextData(0, "こんな団結もあったんだな！俺もまだまだだぜ", 5f, PortraitTextData.FacePatturn.Fun)),
                9),

            questManager.Creater.CreateQuestCreateRole(0, new QuestCreateRole.EstablishRole(true, false), 1, 0, "何らかの団結を成立させる", 0f, false, false, new(
                new PortraitTextData.PTextData(0, "流儀を以て団子を食す、それ即ち団道なり！", 5f, PortraitTextData.FacePatturn.Fun)),
                10),

            questManager.Creater.CreateQuestDestination(10, FloorManager.Floor.floor15, false, "城内の最上層に向かえ", 0f, false, true, new(
                new PortraitTextData.PTextData(0, "遂に辿り着いたぜ！最上階！", 5f, PortraitTextData.FacePatturn.Fun),
                new(1, "長いようで短かったが、最高に満足だぜ！", 5f, PortraitTextData.FacePatturn.Happy),
                new(1, "それじゃ、これで団道完了……だぜ！", 5f, PortraitTextData.FacePatturn.Fun)),
                11),

            questManager.Creater.CreateQuestDestination(11, FloorManager.Floor.Max, false, "クリア！", 0f, false, true,new(new PortraitTextData.PTextData()), 0),
        };

        QuestData.Clear();
        QuestData.AddRange(quest);
        questManager.ChangeQuest(quest[0]);
    }

    protected override PortraitTextData StartPortraitText()
    {
        return new(
            new PortraitTextData.PTextData(0, "今回は刀の長さが団子4つからのスタートみたいだな", 5f, PortraitTextData.FacePatturn.Normal),
            new(1, "流儀を以て、団子を食す……いざ！", 5f, PortraitTextData.FacePatturn.Fun));
    }

    public override List<DangoColor> FloorDangoColors()
    {
        return stageDangoColors;
    }

    public override void OnStageSucceed()
    {
        //TODO:ステージクリア時の処理
    }
}