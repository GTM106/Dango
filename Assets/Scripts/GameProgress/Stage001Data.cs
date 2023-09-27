using Dango.Quest;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Stage001Data : StageData
{
    static readonly List<DangoColor> stageDangoColors = new() {  DangoColor.Beni, DangoColor.Mitarashi, DangoColor.Nori, DangoColor.Yuzu, DangoColor.Yomogi };

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
            questManager.Creater.CreateQuestCreateRole(0, new QuestCreateRole.EstablishRole(true, false), 1, 0, "何らかの団結を成立させる", 30f, false, false,new(
                new PortraitTextData.PTextData(0,"団結を作ったな！",5f,PortraitTextData.FacePatturn.Fun),
                new(0,"お腹も膨れて一石二鳥！この調子でいこう！",5f,PortraitTextData.FacePatturn.Fun)),
                2),
            questManager.Creater.CreateQuestCreateRole(1, new QuestCreateRole.EstablishRole(false, false), 1, 0, "団結を成立させずに団子を食べる", 15f, false, false,new(
                new PortraitTextData.PTextData(0,"美味い！やっぱ栄都の団子は違うねぇ",5f,PortraitTextData.FacePatturn.Happy),
                new(0,"折角なら次は団結を作って食べてみるか……",5f,PortraitTextData.FacePatturn.Normal)),
                2),

            //D5上昇
            questManager.Creater.CreateQuestCreateRole(2, new QuestCreateRole.SpecifyTheRole("全天鏡面"), 2, 0, "線対称で団結を2回作れ", 30f, true, false, new(
                new PortraitTextData.PTextData(0, "しんめとりい、ってやつだな！", 5f, PortraitTextData.FacePatturn.Fun),
                new(1, "串が伸びたぜ。これで上に行けるようになったか……？", 5f, PortraitTextData.FacePatturn.Perplexed)),
                3),

            questManager.Creater.CreateQuestDestination(3, FloorManager.Floor.floor8, false, "上に向かえ", 30f, false, false, new(
                new PortraitTextData.PTextData(0, "ふぅー、高いねぇ！", 5f, PortraitTextData.FacePatturn.Fun),
                new(1, "まだまだ上がありそうだな", 5f, PortraitTextData.FacePatturn.Perplexed)),
                4,5),

            questManager.Creater.CreateQuestCreateRole(4, new QuestCreateRole.EstablishRole(true, false, DangoColor.Mitarashi), 2, 0, "みたらし団子を含んで団結を2回作れ", 30f, false, false, new(
                new PortraitTextData.PTextData(0, "みたらし団子は美味いなぁ！", 5f, PortraitTextData.FacePatturn.Happy),
                new(1, "ついつい食べ過ぎちまうぜ", 5f, PortraitTextData.FacePatturn.Normal)),
                6),
            questManager.Creater.CreateQuestCreateRole(5, new QuestCreateRole.EstablishRole(true, false, DangoColor.Nori), 2, 0, "海苔団子を含んで団結を2回作れ", 30f, false, false, new(
                new PortraitTextData.PTextData(0, "海苔団子か、磯の香りが芳しいぜ", 5f, PortraitTextData.FacePatturn.Normal)),
                6),

            questManager.Creater.CreateQuestCreateRole(6, new QuestCreateRole.CreateSameRole(false),1,2,"異なる団結を2回連続で作る", 30f,true,false,new(
                new PortraitTextData.PTextData(0, "同じ団結ばっかってのも味気ねぇ", 5f, PortraitTextData.FacePatturn.Normal),
                new(1, "折角なら色々な食べ合わせを楽しまねぇとな！", 5f, PortraitTextData.FacePatturn.Fun)),
                7),

            questManager.Creater.CreateQuestDestination(7, FloorManager.Floor.floor9, false, "上に向かえ", 0f, false, true,new(
                new PortraitTextData.PTextData(0, "ふぅー……登った登った", 5f, PortraitTextData.FacePatturn.Normal),
                new(1, "ここらで少し休憩にするか！", 5f, PortraitTextData.FacePatturn.Fun),
                new(2, "団道完了！", 5f, PortraitTextData.FacePatturn.Happy)),
            8),

            questManager.Creater.CreateQuestDestination(8, FloorManager.Floor.Max, false, "クリア！", 0f, false, true,new(new PortraitTextData.PTextData()), 0),
        };

        QuestData.Clear();
        QuestData.AddRange(quest);
        questManager.ChangeQuest(quest[0], quest[1]);
    }

    protected override PortraitTextData StartPortraitText()
    {
        return new(new PortraitTextData.PTextData(0, "団道を始めるぜ！", 5f, PortraitTextData.FacePatturn.Fun),
            new(0, "まずはクエストの確認からだな…", 5f, PortraitTextData.FacePatturn.Normal),
            new(0, "Lボタンを押してみようぜ！", 10f, PortraitTextData.FacePatturn.Fun));
    }

    public override List<DangoColor> FloorDangoColors()
    {
        return stageDangoColors;
    }

    public override void OnStageSucceed()
    {
        Release(Stage.Stage2);
    }
}