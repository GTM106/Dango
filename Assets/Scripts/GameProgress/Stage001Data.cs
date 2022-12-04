using Dango.Quest;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Stage001Data : StageData
{
    static readonly List<DangoColor> stageDangoColors = new() {  DangoColor.Beni, DangoColor.Mitarashi, DangoColor.Nori, DangoColor.Shiratama, DangoColor.Yomogi };

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
            questManager.Creater.CreateQuestCreateRole(0, new QuestCreateRole.EstablishRole(true, false), 1, 0, "���炩�̒c���𐬗�������", 30f, false, false,new(
                new PortraitTextData.PTextData(0,"�c����������ȁI",2f,PortraitTextData.FacePatturn.Normal),
                new(0,"�������c��Ĉ�Γ񒹁I���̒��q�ł������I",2f,PortraitTextData.FacePatturn.Normal)),
                2),
            questManager.Creater.CreateQuestCreateRole(1, new QuestCreateRole.EstablishRole(false, false), 1, 0, "�c���𐬗��������ɒc�q��H�ׂ�", 15f, false, false,new(
                new PortraitTextData.PTextData(0,"�������I����ωh�s�̒c�q�͈Ⴄ�˂�",2f,PortraitTextData.FacePatturn.Normal),
                new(0,"�܊p�Ȃ玟�͒c��������ĐH�ׂĂ݂邩�c�c",2f,PortraitTextData.FacePatturn.Normal)),
                2),

            //D5�㏸
            questManager.Creater.CreateQuestCreateRole(2, new QuestCreateRole.SpecifyTheRole("�S�V����"), 0, 2, "���Ώ̂Œc����2����", 30f, true, false, new(
                new PortraitTextData.PTextData()),
                3),

            questManager.Creater.CreateQuestDestination(3, FloorManager.Floor.floor8, false, "��Ɍ�����", 30f, false, false, new(
                new PortraitTextData.PTextData()),
                4,5),

            questManager.Creater.CreateQuestCreateRole(4, new QuestCreateRole.EstablishRole(true, false, DangoColor.Mitarashi), 2, 0, "�݂��炵�c�q���܂�Œc����2����", 30f, false, false, new(
                new PortraitTextData.PTextData()),
                6),
            questManager.Creater.CreateQuestCreateRole(5, new QuestCreateRole.EstablishRole(true, false, DangoColor.Nori), 2, 0, "�C�ےc�q���܂�Œc����2����", 30f, false, false, new(
                new PortraitTextData.PTextData()),
                6),

            questManager.Creater.CreateQuestCreateRole(6, new QuestCreateRole.CreateSameRole(false),0,2,"�قȂ�c����2��A���ō��", 30f,true,false,new(
                new PortraitTextData.PTextData()),
                7),

            questManager.Creater.CreateQuestDestination(7, FloorManager.Floor.floor9, false, "��Ɍ�����", 0f, false, true,new(new PortraitTextData.PTextData()), 8),
            questManager.Creater.CreateQuestDestination(8, FloorManager.Floor.Max, false, "�N���A�I", 0f, false, true,new(new PortraitTextData.PTextData()), 0),
        };

        QuestData.Clear();
        QuestData.AddRange(quest);
        questManager.ChangeQuest(quest[0], quest[1]);
    }

    protected override PortraitTextData StartPortraitText()
    {
        return new(new PortraitTextData.PTextData(0, "�c�����n�߂邺�I", 2f, PortraitTextData.FacePatturn.Normal), new(0, "�܂��̓N�G�X�g�̊m�F���炾�ȁc", 2f, PortraitTextData.FacePatturn.Normal), new(0, "L�{�^���������Ă݂悤���I", 10f, PortraitTextData.FacePatturn.Normal));
    }

    public override List<DangoColor> FloorDangoColors()
    {
        return stageDangoColors;
    }
}