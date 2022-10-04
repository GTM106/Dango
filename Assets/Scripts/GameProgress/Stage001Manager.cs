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
            _questManager.Creater.CreateQuestCreateRole(0,dangoColors,true,false,1,0,0,"���炩�̖��𐬗�������",30f,true,false,new int[]{2,3 }),
            _questManager.Creater.CreateQuestCreateRole(1,dangoColors,false,false,1,0,0,"���𐬗��������ɒc�q��H�ׂ�",10f,false,false,new int[]{2,3}),
            _questManager.Creater.CreateQuestCreateRole(2,dangoColors,true,false,1,0,0,"���𐬗�������2",30f,false,false,new int[]{0,1 }),
            _questManager.Creater.CreateQuestCreateRole(3,dangoColors,false,false,1,0,0,"���𐬗������Ȃ�2",10f,false,false,0),
        };

        QuestData.AddRange(quest);
        _questManager.ChangeQuest(quest[0], quest[1]);
    }
}
