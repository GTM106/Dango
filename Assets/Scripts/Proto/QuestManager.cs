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

    /// <summary>
    /// �N�G�X�g���N���A���������肷��֐�
    /// </summary>
    /// <param name="quest">���肵�����N�G�X�g</param>
    /// <param name="role">�������</param>
    /// <returns></returns>
    public bool CheckQuestSucceed(QuestCreateRole quest, Role<int> role)
    {
        //�s���ȃA�N�Z�X�ł���Βe��
        if (quest.QuestType != QuestType.CreateRole) return false;
        if (role == null) return false;

        //������������N�G�X�g�ƍ��v���Ă��邩����
        if (role.GetRolename() != quest.RoleName) return false;

        //�������ׂăN���A�����ꍇ�A�N�G�X�g�����Ƃ��ĕԋp
        Logger.Log(quest.QuestName + " �N�G�X�g�N���A�I");
        QuestSucceed();
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
            Logger.Log(quest.QuestName + " �N�G�X�g�N���A�I");
            QuestSucceed();
            return true;
        }

        //�N�G�X�g���s�Ƃ��ĕԋp
        return false;
    }

    private void QuestSucceed()
    {
        //�����ɃN�G�X�g�N���A���ɍs������
    }

}

