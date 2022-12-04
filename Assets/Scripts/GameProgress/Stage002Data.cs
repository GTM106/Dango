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
        SoundManager.Instance.PlayBGM(SoundSource.BGM1A_STAGE1);
    }

    protected override void AddQuest()
    {
        QuestManager questManager = QuestManager.Instance;

        List<QuestData> quest = new()
        {
            questManager.Creater.CreateQuestCreateRole(0, new QuestCreateRole.EstablishRole(true, false, DangoColor.Mitarashi), 2, 0, "�݂��炵�c�q���܂�Ŗ���2����", 30f, true, false, new(
                new PortraitTextData.PTextData()),
                1),

            questManager.Creater.CreateQuestCreateRole(1, new QuestCreateRole.UseColorCount(2), 1, 0, "2�F�łł���������", 15f, false, false,new(
                new PortraitTextData.PTextData()),
                3,4),
            questManager.Creater.CreateQuestCreateRole(2, new QuestCreateRole.UseColorCount(1), 1, 0, "1�F�łł���������", 45f, false, false,new(
                new PortraitTextData.PTextData()),
                3,4),

            //D5�㏸
            questManager.Creater.CreateQuestPlayAction(3, QuestPlayAction.PlayerAction.FallAttack, 3, "�}�~���h����3��h��", 0f, true, false,new(
                new PortraitTextData.PTextData()),
                5),
            questManager.Creater.CreateQuestEatDango(4, DangoColor.Beni, 3, 0, true, true, "�g�F�̒c�q��3�H�ׂ�", 15f, true, false,new(
                new PortraitTextData.PTextData()),
                5),

            questManager.Creater.CreateQuestDestination(5, FloorManager.Floor.floor10, false, "����̒��w�Ɍ�����", 30f, false, false,new(
                new PortraitTextData.PTextData()),
                6, 7),

            questManager.Creater.CreateQuestCreateRole(6, new QuestCreateRole.SpecifyTheRole("�אF����"), 2,0, "�אF���ʂ�2����", 60f, true, false,new(
                new PortraitTextData.PTextData()),
                8, 9),
            questManager.Creater.CreateQuestCreateRole(7, new QuestCreateRole.SpecifyTheRole("�։��]��"), 2, 0, "�։��]����2����", 60f, true, false,new(
                new PortraitTextData.PTextData()),
                8, 9),

            //D5�㏸
            questManager.Creater.CreateQuestCreateRole(8, new QuestCreateRole.EstablishRole(true,false,DangoColor.Mitarashi), 2, 0, "�݂��炵�c�q���܂�Ŗ���2����", 30f, true, false,new(
                new PortraitTextData.PTextData()),
                10),
            questManager.Creater.CreateQuestCreateRole(9, new QuestCreateRole.EstablishRole(true,false,DangoColor.Yomogi), 2, 0, "�ΐF�̒c�q���܂�Ŗ���2����", 30f, true, false,new(
                new PortraitTextData.PTextData()),
                10),

            questManager.Creater.CreateQuestDestination(10, FloorManager.Floor.floor11, false, "����̍ŏ�w�Ɍ�����", 0f, false, true,new(
                new PortraitTextData.PTextData()),
                11),
            questManager.Creater.CreateQuestDestination(11, FloorManager.Floor.Max, false, "�N���A�I", 0f, false, true,new(new PortraitTextData.PTextData()), 0),
        };

        QuestData.Clear();
        QuestData.AddRange(quest);
        questManager.ChangeQuest(quest[0], quest[1]);
    }

    protected override PortraitTextData StartPortraitText()
    {
        return new(
            new PortraitTextData.PTextData(0, "�c�q�������Ă���݂������ȁc�c", 2f, PortraitTextData.FacePatturn.Normal),
            new(1, "�H���̂��y���݂����I", 2f, PortraitTextData.FacePatturn.Normal));
    }

    public override List<DangoColor> FloorDangoColors()
    {
        return stageDangoColors;
    }
}