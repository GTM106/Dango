using Dango.Quest;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Stage001Data : StageData.IAddQuest, StageData.IPortrait
{
    public static Stage001Data Instance = new();

    private Stage001Data()
    {
    }

    public List<QuestData> QuestData = new();
    static readonly List<DangoColor> stageDangoColors = new() { DangoColor.Red, DangoColor.Orange, DangoColor.Yellow, DangoColor.Green, DangoColor.Cyan, DangoColor.Blue, DangoColor.Purple };

    public void AddQuest()
    {
        QuestManager questManager = QuestManager.Instance;

        List<QuestData> quest = new()
        {
            questManager.Creater.CreateQuestCreateRole(0, new QuestCreateRole.EstablishRole(true, false), 1, 0, "���炩�̖��𐬗�������", 30f, false, false,new(new PortraitTextData.PTextData(0,"�����A�����������ȁI",2f,PortraitTextData.FacePatturn.Normal),new(0,"���̒��q�ŗ��ނ��I",2f,PortraitTextData.FacePatturn.Normal)), 2, 3),
            questManager.Creater.CreateQuestCreateRole(1, new QuestCreateRole.EstablishRole(false, false), 1, 0, "���𐬗��������ɒc�q��H�ׂ�", 15f, false, false,new(new PortraitTextData.PTextData(0,"����ϒc�q�͔������ȁI",2f,PortraitTextData.FacePatturn.Normal),new(0,"���̓L���C�ȏ��ԂŐH���Ă݂邩�c",2f,PortraitTextData.FacePatturn.Normal)), 2, 3),

            questManager.Creater.CreateQuestCreateRole(2, new QuestCreateRole.UseColorCount(2), 1, 0, "2�F�łł���������", 15f, false, false,new(new PortraitTextData.PTextData()), 4, 5),
            questManager.Creater.CreateQuestCreateRole(3, new QuestCreateRole.UseColorCount(1), 1, 0, "1�F�łł���������", 45f, false, false,new(new PortraitTextData.PTextData()), 4, 5),

            //D5�㏸
            questManager.Creater.CreateQuestPlayAction(4, QuestPlayAction.PlayerAction.FallAttack, 3, "�}�~���h����3��h��", 0f, true, false,new(new PortraitTextData.PTextData()), 6),
            questManager.Creater.CreateQuestEatDango(5, DangoColor.Red, 3, 0, true, true, "�ԐF�̒c�q��3�H�ׂ�", 15f, true, false,new(new PortraitTextData.PTextData()), 6),

            questManager.Creater.CreateQuestDestination(6, FloorManager.Floor.floor10, false, "����̒��w�Ɍ�����", 30f, false, false,new(new PortraitTextData.PTextData()), 7, 8),

            questManager.Creater.CreateQuestCreateRole(7, new QuestCreateRole.SpecifyTheRole("�אF����"), 2,0, "�אF���ʂ�2����", 60f, true, false,new(new PortraitTextData.PTextData()), 9, 10),
            questManager.Creater.CreateQuestCreateRole(8, new QuestCreateRole.SpecifyTheRole("�։��]��"), 2, 0, "�։��]����2����", 60f, true, false,new(new PortraitTextData.PTextData()), 9, 10),

            //D5�㏸
            questManager.Creater.CreateQuestCreateRole(9, new QuestCreateRole.EstablishRole(true,false,DangoColor.Orange), 2, 0, "��F�̒c�q���܂�Ŗ���2����", 30f, true, false,new(new PortraitTextData.PTextData()), 11),
            questManager.Creater.CreateQuestCreateRole(10, new QuestCreateRole.EstablishRole(true,false,DangoColor.Green), 2, 0, "�ΐF�̒c�q���܂�Ŗ���2����", 30f, true, false,new(new PortraitTextData.PTextData()), 11),

            questManager.Creater.CreateQuestDestination(11, FloorManager.Floor.floor11, false, "����̍ŏ�w�Ɍ�����", 0f, false, true,new(new PortraitTextData.PTextData()), 12),
            questManager.Creater.CreateQuestDestination(12, FloorManager.Floor.Max, false, "�N���A�I", 0f, false, true,new(new PortraitTextData.PTextData()), 0),
        };

        QuestData.Clear();
        QuestData.AddRange(quest);
        questManager.ChangeQuest(quest[0], quest[1]);
    }

    public PortraitTextData GetPortraitQuest()
    {
        return new(new PortraitTextData.PTextData(0, "�c�����n�߂邺�I", 2f, PortraitTextData.FacePatturn.Normal), new(0, "�܂��̓N�G�X�g�̊m�F���炾�ȁc", 2f, PortraitTextData.FacePatturn.Normal), new(0, "L�{�^���������Ă݂悤���I", 10f, PortraitTextData.FacePatturn.Normal));
    }

    public bool IsContainsStageDangoColor(DangoColor color)
    {
        return stageDangoColors.Contains(color);
    }
}