using Dango.Quest;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Stage001Data
{
    public static Stage001Data Instance = new();

    private Stage001Data()
    {
    }

    public List<QuestData> QuestData = new();

    public void AddQuest()
    {
        QuestManager questManager = QuestManager.Instance;

        List<QuestData> quest = new()
        {
            questManager.Creater.CreateQuestCreateRole(0, new QuestCreateRole.EstablishRole(true, false), 1, 0, "���炩�̖��𐬗�������", 30f, false, false, 2, 3),
            questManager.Creater.CreateQuestCreateRole(1, new QuestCreateRole.EstablishRole(false, false), 1, 0, "���𐬗��������ɒc�q��H�ׂ�", 15f, false, false, 2, 3),

            questManager.Creater.CreateQuestCreateRole(2, new QuestCreateRole.UseColorCount(2), 1, 0, "2�F�łł���������", 15f, false, false, 4, 5),
            questManager.Creater.CreateQuestCreateRole(3, new QuestCreateRole.UseColorCount(1), 1, 0, "1�F�łł���������", 45f, false, false, 4, 5),

            //D5�㏸
            questManager.Creater.CreateQuestPlayAction(4, QuestPlayAction.PlayerAction.FallAttack, 3, "�}�~���h����3��h��", 0f, true, false, 6),
            questManager.Creater.CreateQuestEatDango(5, DangoColor.Red, 3, 0, true, true, "�ԐF�̒c�q��3�H�ׂ�", 15f, true, false, 6),

            questManager.Creater.CreateQuestDestination(6, FloorManager.Floor.floor10, false, "����̒��w�Ɍ�����", 30f, false, false, 7, 8),

            questManager.Creater.CreateQuestCreateRole(7, new QuestCreateRole.SpecifyTheRole("�אF����"), 2,0, "�אF���ʂ�2����", 60f, true, false, 9, 10),
            questManager.Creater.CreateQuestCreateRole(8, new QuestCreateRole.SpecifyTheRole("�։��]��"), 2, 0, "�։��]����2����", 60f, true, false, 9, 10),

            //D5�㏸
            questManager.Creater.CreateQuestCreateRole(9, new QuestCreateRole.EstablishRole(true,false,DangoColor.Orange), 2, 0, "��F�̒c�q���܂�Ŗ���2����", 30f, true, false, 11),
            questManager.Creater.CreateQuestCreateRole(10, new QuestCreateRole.EstablishRole(true,false,DangoColor.Green), 2, 0, "�ΐF�̒c�q���܂�Ŗ���2����", 30f, true, false, 11),

            questManager.Creater.CreateQuestDestination(11, FloorManager.Floor.floor11, false, "����̍ŏ�w�Ɍ�����", 0f, false, true, 0),
        };

        QuestData.AddRange(quest);
        questManager.ChangeQuest(quest[0], quest[1]);
    }
}