using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dango.Quest.UI;

namespace Dango.Quest
{
    class QuestSucceedChecker
    {
        public bool CheckQuestCreateRoleSucceed(QuestManager questManager, Role<int> posRole)
        {
            for (int i = 0; i < questManager.QuestsCount; i++)
            {
                //�L���X�g�\�����m�F�i�s�\�ȏꍇ�G���[���N���邽�߂��̏����͕K�{�j
                if (questManager.GetQuest(i) is QuestCreateRole questCr)
                {
                    if (CheckQuestSucceed(questCr, posRole)) return true;
                }
            }

            return false;
        }

        public bool CheckQuestIncludeColorSucceed(QuestManager questManager, List<DangoColor> color)
        {
            for (int i = 0; i < questManager.QuestsCount; i++)
            {
                //�L���X�g�\�����m�F�i�s�\�ȏꍇ�G���[���N���邽�߂��̏����͕K�{�j
                if (questManager.GetQuest(i) is QuestIncludeColor questInc)
                {
                    if (CheckQuestSucceed(questInc, color)) return true;
                }
            }

            return false;
        }

        private bool CheckQuestSucceed(QuestCreateRole quest, Role<int> role)
        {
            //�s���ȃA�N�Z�X�ł���Βe��
            if (quest.QuestType != QuestType.CreateRole) return false;
            if (role == null) return false;

            //������������N�G�X�g�ƍ��v���Ă��邩����
            if (role.GetRolename() != quest.RoleName) return false;

            //������񐔂��J�E���g���āc
            quest.AddMadeCount();

            //�w��񐔍����������
            if (quest.SpecifyCount != quest.MadeCount) return false;

            //�������ׂăN���A�����ꍇ�A�N�G�X�g�����Ƃ��ĕԋp
            QuestSucceed(quest.QuestName);
            return true;
        }

        private bool CheckQuestSucceed(QuestIncludeColor quest, List<DangoColor> color)
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

        private bool CheckQuestSucceed(QuestPlayAction quest)
        {
            //�s���ȃA�N�Z�X�ł���Βe��
            if (quest.QuestType != QuestType.PlayAction) return false;


            //�N�G�X�g���s�Ƃ��ĕԋp
            return false;
        }

        private bool CheckQuestSucceed(QuestGetScore quest, int score)
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
            QuestUI.Instance.OnGUIQuestSucceed(quest_name);

            //�Ȃ�炩�̃N�G�X�g�������̏���
            Logger.Log(quest_name + " �N�G�X�g�N���A�I");
        }
    }
}