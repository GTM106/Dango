using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dango.Quest;

class QuestManager
{
    public QuestCreateRole CreateQuestCreateRole(string role_name, int count, string quest_name)
    {
        return new(role_name, count, quest_name);
    }
    public QuestIncludeColor CreateQuestIncludeColor(DangoColor color, int count, string quest_name)
    {
        return new(color, count, quest_name);
    }
    public QuestPlayAction CreateQuestPlayAction(int count, string quest_name)
    {
        return new(count, quest_name);
    }
    public QuestGetScore CreateQuestGetScore(int score, string quest_name)
    {
        return new(score, quest_name);
    }
    public QuestEatSpecialDango CreateQuestEatSpecialDango()
    {
        return new();
    }

    public bool CheckQuestSucceed(QuestCreateRole quest, Role<int> role)
    {
        //�s���ȃA�N�Z�X�ł���Βe��
        if (quest.QuestType != QuestType.CreateRole) return false;
        if (role == null) return false;

        //������������N�G�X�g�ƍ��v���Ă��邩����
        if (role.GetRolename() != quest.RoleName) return false;

        //�������ׂăN���A�����ꍇ�A�N�G�X�g�����Ƃ��ĕԋp
        QuestSucceed(quest.QuestName);
        return true;
    }

    public bool CheckQuestSucceed(QuestIncludeColor quest, List<DangoColor> color)
    {
        //�s���ȃA�N�Z�X�ł���Βe��
        if (quest.QuestType != QuestType.IncludeColor) return false;
        if (color == null) return false;

        //������������N�G�X�g�w��̐F���܂܂�Ă��邩����
        foreach (var colorItem in color)
        {
            if (colorItem != quest.Color) continue;

            //������񐔂��J�E���g���āc
            quest.AddMadeCount();

            //�w��񐔍���������肵��
            if (quest.SpecifyCount != quest.MadeCount) break;//����Ă��Ȃ��Ȃ炱���ŃJ�b�g

            //����Ă����琬���Ƃ��ĕԋp
            QuestSucceed(quest.QuestName);
            return true;
        }

        //�N�G�X�g���s�Ƃ��ĕԋp
        return false;
    }

    public bool CheckQuestSucceed(QuestPlayAction quest)
    {
        //�s���ȃA�N�Z�X�ł���Βe��
        if (quest.QuestType != QuestType.PlayAction) return false;


        //�N�G�X�g���s�Ƃ��ĕԋp
        return false;
    }

    public bool CheckQuestSucceed(QuestGetScore quest, int score)
    {
        //�s���ȃA�N�Z�X�ł���Βe��
        if (quest.QuestType != QuestType.GetScore) return false;

        //�X�R�A��ǉ�����
        quest.AddScore(score);

        //���v�X�R�A���~�b�V�����w��l�𒴂��Ă��邩����
        if (quest.Score < quest.ClearScore) return false;

        //�����Ă����琬���Ƃ��ĕԋp
        QuestSucceed(quest.QuestName);
        return true;
    }

    private void QuestSucceed(string quest_name)
    {
        QuestUI.OnGUIQuestSucceed(quest_name);

        //�Ȃ�炩�̃N�G�X�g�������̏���
        Logger.Log(quest_name + " �N�G�X�g�N���A�I");
    }

}

