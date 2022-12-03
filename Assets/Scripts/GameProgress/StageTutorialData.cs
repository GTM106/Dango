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
    }

    protected override void AddQuest()
    {
        QuestManager questManager = QuestManager.Instance;

        List<QuestData> quest = new()
        {
            questManager.Creater.CreateQuestPlayAction(0,QuestPlayAction.PlayerAction.Stab,1,"�c�q������",0,false,false,new(new PortraitTextData.PTextData(0,"�������B�h�����c�q�͉�ʉE�ɏo�Ă邩���",5f,PortraitTextData.FacePatturn.Normal),new(1,"���ɏW�߂��c�q��H�ׂĂ݂�B�c�q���W�߂āw�H�ׂ�x�{�^����",10f,PortraitTextData.FacePatturn.Normal)),1),

            questManager.Creater.CreateQuestEatDango(1, 3, 0, true, true, "�c�q��3�H�ׂ�", 0f, true, false,new(new PortraitTextData.PTextData(0,"�����[�������I�ǂ����q��",5f,PortraitTextData.FacePatturn.Normal)), 2,3),

            questManager.Creater.CreateQuestCreateRole(2, new QuestCreateRole.EstablishRole(true, false), 1, 0, "�K�����������ׂāw�c���x�����", 0, false, false,new(new PortraitTextData.PTextData(0,"�����I�c�q�͊�{�I�Ɂw�c���x������ĐH�ׂ�����������������",10f,PortraitTextData.FacePatturn.Normal),new(0,"���́w�����сx�{�^���ō����Ƃ���ɍs���Ă݂邼",10f,PortraitTextData.FacePatturn.Normal)), 4),
            questManager.Creater.CreateQuestCreateRole(3, new QuestCreateRole.EstablishRole(false, false), 1, 0, "�����Ƀq���g���o���Ă݂�Ƃ��������ȁI", 0, false, false,new(new PortraitTextData.PTextData(0,"�ɂ����ȁA����������ƋK�����������ׂĂ݂�I",8f,PortraitTextData.FacePatturn.Normal)), 2, 3),

            questManager.Creater.CreateQuestDestination(4, FloorManager.Floor.floor2, false, "����Ɍ�����", 30f, true, false,new(new PortraitTextData.PTextData()), 5),

            questManager.Creater.CreateQuestPlayAction(5, QuestPlayAction.PlayerAction.FallAttack, 1, "�}�~���Œc�q���h��", 0f, true, true,new(new PortraitTextData.PTextData()), 6),
            questManager.Creater.CreateQuestDestination(6, FloorManager.Floor.Max, false, "�N���A�I", 0f, false, true,new(new PortraitTextData.PTextData()), 0),
        };

        QuestData.Clear();
        QuestData.AddRange(quest);
        questManager.ChangeQuest(quest[0]);
    }

    protected override PortraitTextData StartPortraitText()
    {
        return new(new(0, "�c���̊�{���̊�{�́A�c�q���h���ďW�߂�Ƃ��납�炾�B", 2f, PortraitTextData.FacePatturn.Normal), new(0, "�w�˂��h���x�{�^���Œc�q���h���Ă݂�I", 2f, PortraitTextData.FacePatturn.Normal));
    }

    public override List<DangoColor> FloorDangoColors()
    {
        return stageDangoColors;
    }
}