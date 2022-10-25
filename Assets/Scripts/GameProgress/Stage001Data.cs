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

    static readonly DangoColor[] dangoColors = { DangoColor.Red, DangoColor.Orange, DangoColor.Yellow, DangoColor.Green, DangoColor.Cyan, DangoColor.Blue, DangoColor.Purple };

    public void AddQuest()
    {
        List<QuestData> quest = new()
        {
            _questManager.Creater.CreateQuestCreateRole(0, dangoColors, true, false, 1, 0, 0, "���炩�̖��𐬗�������", 30f, false, false, new int[] { 2, 3 }),
            _questManager.Creater.CreateQuestCreateRole(1, dangoColors, false, false, 1, 0, 0, "���𐬗��������ɒc�q��H�ׂ�", 15f, false, false, new int[] { 2, 3 }),

            _questManager.Creater.CreateQuestCreateRole(2, dangoColors, true, false, 1, 0, 2, "2�F�łł���������", 30f, false, false, new int[] { 5 }),
            _questManager.Creater.CreateQuestCreateRole(3, dangoColors, true, false, 1, 0, 1, "1�F�łł���������", 0f, true, false, new int[] { 4 }),

            _questManager.Creater.CreateQuestPlayAction(4, QuestPlayAction.PlayerAction.FallAttack, 3, "�}�~���h����3��h��", 0f, true, false, new int[] { 6, 7 }),
            _questManager.Creater.CreateQuestEatDango(5, DangoColor.Red, 3, 0, true, true, "�ԐF�̒c�q��3�H�ׂ�", 15f, false, false, new int[] { 6, 7 }),

            //Cube001-20�t��
            _questManager.Creater.CreateQuestDestination(6, false, "��̓쐼�̒���Ɍ�����", 30f, true, false, new int[] { 8, 9 }),

            //Cube001-13�t��
            _questManager.Creater.CreateQuestDestination(7, false, "��̖k���̒���Ɍ�����", 30f, true, false, new int[] { 8, 9 }),

            _questManager.Creater.CreateQuestCreateRole(8, DangoColor.Orange, true, true, 3, 0, 0, "��F�̒c�q���܂�Ŗ���3����", 30f, false, false, new int[] { 10 }),
            _questManager.Creater.CreateQuestCreateRole(9, DangoColor.Green, true, true, 3, 0, 0, "�ΐF�̒c�q���܂�Ŗ���3����", 30f, false, false, new int[] { 10 }),

            _questManager.Creater.CreateQuestDestination(10, false, "��̕󕨌ɂ֌�����", 0f, false, true, 0),




        };

        QuestData.AddRange(quest);
        _questManager.ChangeQuest(quest[0], quest[1]);
    }
}