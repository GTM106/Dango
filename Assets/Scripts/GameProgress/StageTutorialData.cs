using Dango.Quest;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StageTutorialData : StageData
{
    static readonly List<DangoColor> stageDangoColors = new() { DangoColor.Beni, DangoColor.Shiratama, DangoColor.Yomogi };

    protected override void Start()
    {
        base.Start();
        SoundManager.Instance.StopBGM();
        SoundManager.Instance.PlayBGM(SoundSource.BGM1E_TUTORIAL);
    }

    protected override void AddQuest()
    {
        QuestManager questManager = QuestManager.Instance;

        List<QuestData> quest = new()
        {

            questManager.Creater.CreateQuestDestination(0, FloorManager.Floor.floor2, false, "移動して先に進め", 0f, false, false, new(
                new PortraitTextData.PTextData(0, "お見事！団道達成だ！このゲームはこんな風に、団道を達成していくのが目的になる", 0f, PortraitTextData.FacePatturn.Happy),
                new(1, "現在の目標はYボタンかLボタンで確認できるからな", 0f, PortraitTextData.FacePatturn.Normal),
                new(1, "じゃあ次は、視点操作の練習だ。右スティックを動かして「社」を探してみてくれ", 0f, PortraitTextData.FacePatturn.Normal)),
                1),

            questManager.Creater.CreateQuestPlayAction(1,QuestPlayAction.PlayerAction.Look,0,"「社」を探せ",0f,false,false,new(
                new PortraitTextData.PTextData(0, "視点操作もばっちりだな！", 0f, PortraitTextData.FacePatturn.Happy),
                new(1, "次は高跳びアクションの練習だ。Bボタンで出来るぜ", 0f, PortraitTextData.FacePatturn.Normal),
                new(1, "それであの高台まで登ってみてくれ", 0f, PortraitTextData.FacePatturn.Normal)),
                2),

            questManager.Creater.CreateQuestDestination(2, FloorManager.Floor.floor3, false, "高台に向かえ", 0f, false, false, new(
                new PortraitTextData.PTextData(0, "やるねぇ！簡単すぎたか？", 0f, PortraitTextData.FacePatturn.Normal),
                new PortraitTextData.PTextData(1, "ジャンプの高さは串の長さで決まる。串が伸びればより高いところに行けるって覚えておけ", 0f, PortraitTextData.FacePatturn.Normal),
                new PortraitTextData.PTextData(2, "さて、次が一番重要な突き刺しアクションだ", 0f, PortraitTextData.FacePatturn.Normal),
                new PortraitTextData.PTextData(3, "団子を正面に捉えてAボタンで団子を刺すことが出来るぜ！", 0f, PortraitTextData.FacePatturn.Normal),
                new PortraitTextData.PTextData(4, "試しに3回、団子を刺してみろ", 0f, PortraitTextData.FacePatturn.Normal)),
                3),


            questManager.Creater.CreateQuestPlayAction(3,QuestPlayAction.PlayerAction.Stab,3,"3回団子を刺す", 0f,false, false,new(
                new PortraitTextData.PTextData(0,"団子を3つ刺せたな！美味そうだぜ！",0f,PortraitTextData.FacePatturn.Happy),
                new(1,"さて、このまま食べても良いんだが、団子ってのは規則正しく並べて食べた方が美味で腹持ちがいい",0f,PortraitTextData.FacePatturn.Normal),
                new(2, "規則正しく団子を並べたものを『団結』って言うんだ", 0f, PortraitTextData.FacePatturn.Normal),
                new(3, "まずは基本の『全天鏡面』こと線対称の団結作りからだ", 0f, PortraitTextData.FacePatturn.Normal),
                new(4, "と思ったが、今は串がいっぱいだな、Rトリガー長押しで団子を外してみようぜ", 0f, PortraitTextData.FacePatturn.Normal)),
                4),

            questManager.Creater.CreateQuestPlayAction(4, QuestPlayAction.PlayerAction.RemoveAnyPlace, 1, "1回団子を外す", 0f, false, false, new(
                new PortraitTextData.PTextData(0, "オーケーだ", 5f, PortraitTextData.FacePatturn.Normal),
                new PortraitTextData.PTextData(1, "それじゃあ線対称を作るために、最後の一個の団子を刺してみてくれ", 5f, PortraitTextData.FacePatturn.Normal),
                new PortraitTextData.PTextData(2, "例えば、紅団子、よもぎ団子、紅団子、みたいに左右対称にするんだぜ", 5f, PortraitTextData.FacePatturn.Normal),
                new PortraitTextData.PTextData(3, "完成したらLトリガー長押しで食べる！団結が出来てれば成功だ！", 5f, PortraitTextData.FacePatturn.Normal)),
                5),

            questManager.Creater.CreateQuestCreateRole(5, new QuestCreateRole.SpecifyTheRole("全天鏡面"), 1, 0, "線対称で団結を作れ", 0f, false, true, new(
                new PortraitTextData.PTextData(0, "くぅー美味い！見事な団結だぜ！", 0f, PortraitTextData.FacePatturn.Happy),
                new(1, "団結を作れば腹も膨れて制限時間がもらえる、だからどんどん団結を作ってみてくれ", 0f, PortraitTextData.FacePatturn.Normal),
                new(1, "基本操作は以上だ、それじゃあ頑張れよ！", 0f, PortraitTextData.FacePatturn.Happy)),
                6),

            questManager.Creater.CreateQuestDestination(6, FloorManager.Floor.Max, false, "初心者指南完了！", 0f, false, true,new(
                new PortraitTextData.PTextData()),
                0),
        };

        QuestData.Clear();
        QuestData.AddRange(quest);
        questManager.ChangeQuest(quest[0]);
    }

    protected override PortraitTextData StartPortraitText()
    {
        return new(new(0, "流儀を以て団子を食す、ようこそ！団道へ", 2f, PortraitTextData.FacePatturn.Normal),
            new(0, "まずは基本的な操作説明からだ。左スティックで移動、先に進んでみてくれ", 2f, PortraitTextData.FacePatturn.Normal));
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