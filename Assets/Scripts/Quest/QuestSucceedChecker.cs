using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dango.Quest.UI;
using System.Linq;
using System.Data;

namespace Dango.Quest
{
    class QuestSucceedChecker
    {
        QuestManager _manager;

        public QuestSucceedChecker(QuestManager manager)
        {
            _manager = manager;
        }

        #region EatDango
        public bool CheckQuestEatDangoSucceed(QuestManager questManager, List<DangoColor> colors, bool createRole)
        {
            for (int i = 0; i < questManager.QuestsCount; i++)
            {
                //�L���X�g�\�����m�F�i�s�\�ȏꍇ�G���[���N���邽�߂��̏����͕K�{�j
                if (questManager.GetQuest(i) is QuestEatDango questEa)
                {
                    if (CheckQuestSucceed(questEa, colors, createRole)) return true;
                }
            }

            return false;
        }

        private bool CheckQuestSucceed(QuestEatDango quest, List<DangoColor> colors, bool createRole)
        {
            //�s���ȃA�N�Z�X�ł���Βe��
            if (quest.QuestType != QuestType.EatDango) return false;

            //���̐�������Œe�����̂͒e��
            if (!quest.CanCountCreateRole && createRole)
            {
                quest.SetIsPrebCreateRole(createRole);
                return false;
            }
            if (!quest.CanCountNoCreateRole && !createRole)
            {
                quest.SetIsPrebCreateRole(createRole);
                return false;
            }

            //�F�𔻒肵�A�������F�Ȃ�H�ׂ�����ǉ�
            foreach (var color in colors)
            {
                if (!quest.Colors.Contains(color)) continue;

                quest.AddEatCount();
            }

            if (quest.IsPrebCreateRole != createRole) quest.AddContinueCount();
            else quest.ResetContinueCount();

            //����̑O�ɍ��������������L�^
            quest.SetIsPrebCreateRole(createRole);

            //�w��񐔍����������
            if (quest.SpecifyCount > quest.EatCount) return false;
            if (quest.ContinueCount > quest.CurrentContinueCount) return false;

            //�������ׂăN���A�����ꍇ�A�N�G�X�g�����Ƃ��ĕԋp
            QuestSucceed(quest);
            return true;
        }
        #endregion

        #region CreateRole
        public bool CheckQuestCreateRoleSucceed(QuestManager questManager, Role<int> posRole, List<DangoColor> dangos, bool createRole, int currentMaxDango)
        {
            for (int i = 0; i < questManager.QuestsCount; i++)
            {
                //�L���X�g�\�����m�F�i�s�\�ȏꍇ�G���[���N���邽�߂��̏����͕K�{�j
                if (questManager.GetQuest(i) is QuestCreateRole questCr)
                {
                    if (CheckQuestSucceed(questCr, posRole,createRole,dangos.Distinct().Count(), currentMaxDango)) return true;
                }
            }

            return false;
        }
        public bool CheckQuestCreateRoleSucceed(QuestManager questManager, List<DangoColor> dangos, int currentMaxDango)
        {
            for (int i = 0; i < questManager.QuestsCount; i++)
            {
                //�L���X�g�\�����m�F�i�s�\�ȏꍇ�G���[���N���邽�߂��̏����͕K�{�j
                if (questManager.GetQuest(i) is QuestCreateRole questCr)
                {
                    if (CheckQuestSucceed(questCr, dangos, currentMaxDango)) return true;
                }
            }

            return false;
        }
        public bool CheckQuestCreateRoleSucceed(QuestManager questManager, bool createRole)
        {
            for (int i = 0; i < questManager.QuestsCount; i++)
            {
                //�L���X�g�\�����m�F�i�s�\�ȏꍇ�G���[���N���邽�߂��̏����͕K�{�j
                if (questManager.GetQuest(i) is QuestCreateRole questCr)
                {
                    if (CheckQuestSucceed(questCr, createRole)) return true;
                }
            }

            return false;
        }
        
        private bool CheckQuestSucceed(QuestCreateRole quest, Role<int> role, bool createRole,int colorCount,int currentMaxDango)
        {
            //�s���ȃA�N�Z�X�ł���Βe��
            if (quest.QuestType != QuestType.CreateRole) return false;
            if (role == null) return false;

            //���̐����E�񐬗��̃t���O����v���Ă��Ȃ���Βe��
            if (createRole != quest.CreateRole) return false;

            //�F�̐��𔻒�
            var color = role.GetData().Distinct();
            if (color.Count() != colorCount) return false;

            //������������N�G�X�g�ƍ��v���Ă��邩����
            if (role.GetRolename() != quest.RoleName) return false;

            //���S���݂̂̏ꍇ���S��������
            if (quest.OnlyPerfectRole && role.GetData().Length != currentMaxDango) return false;

            //������񐔂��J�E���g���āc
            quest.AddMadeCount();

            //�w��񐔍����������
            if (quest.SpecifyCount != quest.MadeCount) return false;

            //�������ׂăN���A�����ꍇ�A�N�G�X�g�����Ƃ��ĕԋp
            QuestSucceed(quest);
            return true;
        }
        private bool CheckQuestSucceed(QuestCreateRole quest, List<DangoColor> colors, int currentMaxDango)
        {
            //�s���ȃA�N�Z�X�ł���Βe��
            if (quest.QuestType != QuestType.CreateRole) return false;

            //���炩�̖��Ƃ������������Ȃ��N�G�X�g�̏ꍇ�e��
            if (!quest.EnableAnyRole) return false;

            //���S���݂̂̏ꍇ���S��������
            if (quest.OnlyPerfectRole && colors.Count != currentMaxDango) return false;

            //�F�𔻒肵�A�������F�Ȃ�H�ׂ�����ǉ�
            foreach (var color in colors.Distinct())
            {
                //�w��F���������甲����
                if (quest.Colors.Contains(color)) break;

                //����ԂȂ�������e��
                return false;
            }

            //������񐔂��J�E���g���āc
            quest.AddMadeCount();

            //�w��񐔍����������
            if (quest.SpecifyCount > quest.MadeCount) return false;

            //�������ׂăN���A�����ꍇ�A�N�G�X�g�����Ƃ��ĕԋp
            QuestSucceed(quest);
            return true;
        }
        private bool CheckQuestSucceed(QuestCreateRole quest,bool createRole)
        {
            //�s���ȃA�N�Z�X�ł���Βe��
            if (quest.QuestType != QuestType.CreateRole) return false;

            //�������ꍇ�̃p�^�[����e��
            if (quest.CreateRole)
            {
                quest.SetIsPrebCreateRole(createRole);
                return false;
            }

            quest.AddMadeCount();

            //�w��񐔍����������
            if (quest.MadeCount < quest.SpecifyCount) return false;

            quest.AddContinueCount();

            if (quest.IsPrebCreateRole == createRole) quest.AddContinueCount();
            else quest.ResetContinueCount();

            quest.SetIsPrebCreateRole(createRole);

            //�w��񐔍����������
            if (quest.ContinueCount >= quest.CurrentContinueCount) return false;
            
            //�������ׂăN���A�����ꍇ�A�N�G�X�g�����Ƃ��ĕԋp
            QuestSucceed(quest);

            return true;
        }
        #endregion

        #region PlayAction
        public bool CheckQuestPlayActionSucceed(QuestManager questManager, QuestPlayAction.PlayerAction action)
        {
            for (int i = 0; i < questManager.QuestsCount; i++)
            {
                //�L���X�g�\�����m�F�i�s�\�ȏꍇ�G���[���N���邽�߂��̏����͕K�{�j
                if (questManager.GetQuest(i) is QuestPlayAction questPla)
                {
                    if (CheckQuestSucceed(questPla, action)) return true;
                }
            }

            return false;
        }

        private bool CheckQuestSucceed(QuestPlayAction quest, QuestPlayAction.PlayerAction action)
        {
            //�s���ȃA�N�Z�X�ł���Βe��
            if (quest.QuestType != QuestType.PlayAction) return false;

            //���肵�����A�N�V�������قȂ�����e��
            if (quest.Action != action) return false;

            quest.AddMadeCount();

            //�w��񐔍����������
            if (quest.SpecifyCount != quest.MadeCount) return false;

            QuestSucceed(quest);
            return true;
        }
        #endregion

        #region Destination
        public bool CheckQuestDestinationSucceed(QuestManager questManager,bool onEatSucceed)
        {
            for (int i = 0; i < questManager.QuestsCount; i++)
            {
                //�L���X�g�\�����m�F�i�s�\�ȏꍇ�G���[���N���邽�߂��̏����͕K�{�j
                if (questManager.GetQuest(i) is QuestDestination questDest)
                {
                    if (CheckQuestSucceed(questDest,onEatSucceed)) return true;
                }
            }

            return false;
        }

        private bool CheckQuestSucceed(QuestDestination quest,bool onEatSucceed)
        {
            //�s���ȃA�N�Z�X�ł���Βe��
            if (quest.QuestType != QuestType.Destination) return false;

            //�ړI�n�ɂ������ł����̂��A���ĐH�ׂȂ��Ƃ����Ȃ��̂�����
            if (quest.OnEatSucceed != onEatSucceed) return false;

            QuestSucceed(quest);
            return true;
        }
        #endregion

        private void QuestSucceed(QuestData quest)
        {
            QuestUI.Instance.OnGUIQuestSucceed(quest.QuestName);

            SoundManager.Instance.PlaySE(SoundSource.SE12_QUEST_SUCCEED);

            List<QuestData> nextQuest = new();
            for (int i = 0; i < quest.NextQuestId.Count; i++)
            {
                nextQuest.Add(Stage001Data.Instance.QuestData[quest.NextQuestId[i]]);
            }

            _manager.ChangeQuest(nextQuest);
            _manager.Player.GrowStab(quest.EnableDangoCountUp);
            _manager.Player.AddSatiety(quest.RewardTime);
            _manager.CreateExpansionUIObj();

            if (quest.IsKeyQuest)
            {
                //TODO:S7�ɑJ��
            }

            Logger.Log(quest.QuestName + " �N�G�X�g�N���A�I");
        }
    }
}