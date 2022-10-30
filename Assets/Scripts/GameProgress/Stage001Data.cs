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

    QuestManager _questManager = QuestManager.Instance;

    public List<QuestData> QuestData = new();

    public void AddQuest()
    {
        List<QuestData> quest = new()
        {
            _questManager.Creater.CreateQuestCreateRole(0, new QuestCreateRole.EstablishRole(true,false), 1, 0, "���炩�̖��𐬗�������", 30f, false, false, 2, 3 ),
            _questManager.Creater.CreateQuestCreateRole(1, new QuestCreateRole.EstablishRole(false,false), 1, 0, "���𐬗��������ɒc�q��H�ׂ�", 15f, false, false,2, 3 ),

            _questManager.Creater.CreateQuestCreateRole(2, new QuestCreateRole.UseColorCount(2), 1, 0, "2�F�łł���������", 30f, false, false, 5),
            _questManager.Creater.CreateQuestCreateRole(3, new QuestCreateRole.UseColorCount(1), 1, 0, "1�F�łł���������", 0f, true, false, 4),

            _questManager.Creater.CreateQuestPlayAction(4, QuestPlayAction.PlayerAction.FallAttack, 3, "�}�~���h����3��h��", 0f, true, false, 6, 7),
            _questManager.Creater.CreateQuestEatDango(5, DangoColor.Red, 3, 0, true, true, "�ԐF�̒c�q��3�H�ׂ�", 15f, false, false, 6, 7),

            //Cube001-20�t��
            _questManager.Creater.CreateQuestDestination(6, FloorManager.Floor.floor1,false, "��̓쐼�̒���Ɍ�����", 30f, true, false, 8, 9),
            //Cube001-13�t��
            _questManager.Creater.CreateQuestDestination(7, FloorManager.Floor.floor1, false, "��̖k���̒���Ɍ�����", 30f, true, false, 8, 9),

            _questManager.Creater.CreateQuestCreateRole(8, new QuestCreateRole.EstablishRole(true,false,DangoColor.Orange), 3, 0, "��F�̒c�q���܂�Ŗ���3����", 30f, false, false,10),
            _questManager.Creater.CreateQuestCreateRole(9, new QuestCreateRole.EstablishRole(true,false,DangoColor.Green), 3, 0, "�ΐF�̒c�q���܂�Ŗ���3����", 30f, false, false,10),

            _questManager.Creater.CreateQuestDestination(10, FloorManager.Floor.floor1, false, "��̕󕨌ɂ֌�����", 0f, false, true, 0),
        };

        QuestData.AddRange(quest);
        _questManager.ChangeQuest(quest[0], quest[1]);
    }
}