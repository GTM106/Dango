using Dango.Quest;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Stage002Data : StageData
{
    static readonly List<DangoColor> stageDangoColors = new() { DangoColor.An, DangoColor.Beni, DangoColor.Mitarashi, DangoColor.Nori, DangoColor.Shiratama, DangoColor.Yomogi };

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
            questManager.Creater.CreateQuestCreateRole(0, new QuestCreateRole.EstablishRole(true, false, DangoColor.An), 2, 0, "あん団子を含んで団結を2回作れ", 30f, false, false, new(
                new PortraitTextData.PTextData(0, "これがあん団子かぁ！", 5f, PortraitTextData.FacePatturn.Normal),
                new(1, "小豆の甘みが口に広がって最高だなぁ！", 5f, PortraitTextData.FacePatturn.Normal)),
                1),

            questManager.Creater.CreateQuestCreateRole(1, new QuestCreateRole.UseColorCount(2), 1, 0, "2種類の団子でできる団結を作る", 15f, false, false,new(
                new PortraitTextData.PTextData(0, "この食べ合わせ……アリ、だな！", 5f, PortraitTextData.FacePatturn.Normal)),
                3,4),
            questManager.Creater.CreateQuestCreateRole(2, new QuestCreateRole.UseColorCount(1), 1, 0, "1種類の団子でできる団結を作る", 45f, false, false,new(
                new PortraitTextData.PTextData(0, "同じ団子だけで食べる、団道の基本中の基本だな！", 5f, PortraitTextData.FacePatturn.Normal)),
                3,4),

            //D5上昇
            questManager.Creater.CreateQuestPlayAction(3, QuestPlayAction.PlayerAction.FallAttack, 3, "急降下刺しで3回刺す", 0f, true, false,new(
                new PortraitTextData.PTextData(0, "刺し方にもこだわるのが、流儀ってもんだ！", 5f, PortraitTextData.FacePatturn.Normal)),
                5),
            questManager.Creater.CreateQuestEatDango(4, DangoColor.Beni, 3, 0, true, true, "紅色の団子を3つ食べる", 15f, true, false,new(
                new PortraitTextData.PTextData(0, "紅一点、全くこいつらはべっぴんさんだな！", 5f, PortraitTextData.FacePatturn.Normal)),
                5),

            questManager.Creater.CreateQuestDestination(5, FloorManager.Floor.floor10, false, "城内の中層に向かえ", 30f, false, false,new(
                new PortraitTextData.PTextData(0, "まだまだ上を目指せるぜ！", 5f, PortraitTextData.FacePatturn.Normal)),
                6, 7),

            questManager.Creater.CreateQuestCreateRole(6, new QuestCreateRole.SpecifyTheRole("隣色鏡面"), 2,0, "二分割で団結を2回作れ", 60f, true, false,new(
                new PortraitTextData.PTextData(0, "隣合わせ、鏡合わせ、規則正しく食すのみ！", 5f, PortraitTextData.FacePatturn.Normal)),
                8, 9),
            questManager.Creater.CreateQuestCreateRole(7, new QuestCreateRole.SpecifyTheRole("輪廻転生"), 2, 0, "ループで団結を2回作れ", 60f, true, false,new(
                new PortraitTextData.PTextData(0, "輪廻の如く流転する、哲学的な団結だぜ", 5f, PortraitTextData.FacePatturn.Normal)),
                8, 9),

            //D5上昇
            questManager.Creater.CreateQuestCreateRole(8, new QuestCreateRole.EstablishRole(true,false,DangoColor.Shiratama), 2, 0, "しらたま団子を含んで団結を2回作れ", 30f, true, false,new(
                new PortraitTextData.PTextData(0, "真珠のように輝いてるぜ、食べちまうのが勿体ねぇなぁ", 5f, PortraitTextData.FacePatturn.Normal)),
                10),
            questManager.Creater.CreateQuestCreateRole(9, new QuestCreateRole.EstablishRole(true,false,DangoColor.Yomogi), 2, 0, "緑色の団子を含んで団結を2回作れ", 30f, true, false,new(
                new PortraitTextData.PTextData(0, "よもぎ団子か、仄かな苦味が食欲をそそるな", 5f, PortraitTextData.FacePatturn.Normal)),
                10),

            questManager.Creater.CreateQuestDestination(10, FloorManager.Floor.floor11, false, "城内の上層に向かえ", 0f, false, true,new(
                new PortraitTextData.PTextData(0, "もうすぐ最上階。ちっとここらで一休みしておくか", 5f, PortraitTextData.FacePatturn.Normal),
                new(1, "団道完了っと！", 5f, PortraitTextData.FacePatturn.Normal)),
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
            new PortraitTextData.PTextData(0, "団子が増えているみたいだな……", 5f, PortraitTextData.FacePatturn.Normal),
            new(1, "食うのが楽しみだぜ！", 5f, PortraitTextData.FacePatturn.Normal));
    }

    public override List<DangoColor> FloorDangoColors()
    {
        return stageDangoColors;
    }

    public override void OnStageSucceed()
    {
        Release(Stage.Stage3);
    }
}