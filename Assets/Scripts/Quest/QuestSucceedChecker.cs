using System.Collections.Generic;
using Dango.Quest.UI;
using System.Linq;
using Cysharp.Threading.Tasks;

namespace Dango.Quest
{
    class QuestSucceedChecker
    {
        QuestManager _manager;
        bool isSucceedThisFrame;

        PlayerUIManager _playerUIManager;

        private async UniTask SetBoolAfterOneFrame(bool enable)
        {
            await UniTask.Yield();

            isSucceedThisFrame = enable;
        }

        public QuestSucceedChecker(QuestManager manager, PlayerUIManager playerUIManager)
        {
            _manager = manager;
            _playerUIManager = playerUIManager;
        }

        #region EatDango
        public bool CheckQuestEatDangoSucceed(QuestManager questManager, List<DangoColor> colors, bool createRole)
        {
            //���̃t���[���ɕʂ̃N�G�X�g���N���A����Ă�����e��
            if (isSucceedThisFrame) return false;

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
            //���̐�������Œe�����̂͒e��
            if (createRole && !quest.AllowCountCreateRole || !createRole && !quest.AllowCountNoCreateRole)
            {
                quest.SetIsPrebCreateRole(createRole);
                return false;
            }

            //�F�𔻒肵�A�������F�Ȃ�H�ׂ�����ǉ�
            foreach (var color in colors)
            {
                if (!quest.ReadColors.Contains(color)) continue;

                quest.AddEatCount();
            }

            if (quest.IsPrebCreateRole != createRole) quest.AddContinueCount();
            else quest.ResetContinueCount();

            //����̑O�ɍ��������������L�^
            quest.SetIsPrebCreateRole(createRole);

            //�w��񐔍����������
            if (!quest.IsAchievedEatCount()) return false;
            if (!quest.IsAchievedContinueCount()) return false;

            //�������ׂăN���A�����ꍇ�A�N�G�X�g�����Ƃ��ĕԋp
            QuestSucceed(quest);
            return true;
        }
        #endregion

        #region CreateRole
        public bool CheckQuestCreateRoleSucceedEs(List<DangoColor> dangos, bool createRole, int currentMaxDango)
        {
            //���̃t���[���ɕʂ̃N�G�X�g���N���A����Ă�����e��
            if (isSucceedThisFrame) return false;

            for (int i = 0; i < _manager.QuestsCount; i++)
            {
                //�L���X�g�\�����m�F�i�s�\�ȏꍇ�G���[���N���邽�߂��̏����͕K�{�j
                if (_manager.GetQuest(i) is QuestCreateRole questCr)
                {
                    if (CheckQuestSucceedEs(questCr, dangos, createRole, currentMaxDango)) return true;
                }
            }

            return false;
        }
        public bool CheckQuestCreateRoleSucceedSr(Role<int> role)
        {
            //���̃t���[���ɕʂ̃N�G�X�g���N���A����Ă�����e��
            if (isSucceedThisFrame) return false;

            for (int i = 0; i < _manager.QuestsCount; i++)
            {
                //�L���X�g�\�����m�F�i�s�\�ȏꍇ�G���[���N���邽�߂��̏����͕K�{�j
                if (_manager.GetQuest(i) is QuestCreateRole questCr)
                {
                    if (CheckQuestSucceedSr(questCr, role)) return true;
                }
            }

            return false;
        }
        public bool CheckQuestCreateRoleSucceedIr(List<DangoColor> dangos)
        {
            //���̃t���[���ɕʂ̃N�G�X�g���N���A����Ă�����e��
            if (isSucceedThisFrame) return false;

            for (int i = 0; i < _manager.QuestsCount; i++)
            {
                //�L���X�g�\�����m�F�i�s�\�ȏꍇ�G���[���N���邽�߂��̏����͕K�{�j
                if (_manager.GetQuest(i) is QuestCreateRole questCr)
                {
                    if (CheckQuestSucceedIc(questCr, dangos)) return true;
                }
            }

            return false;
        }
        public bool CheckQuestCreateRoleSucceedSm(Role<int> role)
        {
            //���̃t���[���ɕʂ̃N�G�X�g���N���A����Ă�����e��
            if (isSucceedThisFrame) return false;

            for (int i = 0; i < _manager.QuestsCount; i++)
            {
                //�L���X�g�\�����m�F�i�s�\�ȏꍇ�G���[���N���邽�߂��̏����͕K�{�j
                if (_manager.GetQuest(i) is QuestCreateRole questCr)
                {
                    if (CheckQuestSucceedSm(questCr, role)) return true;
                }
            }

            return false;
        }

        private bool HasReadColor(QuestCreateRole quest, IEnumerable<DangoColor> dangosDistinct)
        {
            foreach (var color in dangosDistinct)
            {
                //�w��F���������甲����
                if (quest.Establish.ReadColors.Contains(color)) return true;
            }

            //����ԂȂ�������e��
            return false;
        }

        private bool CheckQuestSucceedEs(QuestCreateRole quest, List<DangoColor> dangos, bool createRole, int currentMaxDango)
        {
            //�s���ȃA�N�Z�X�ł���Βe��
            if (quest.CRType != QuestCreateRole.CreateRoleType.EstablishRole) return false;

            //�H�ׂ��c�q���N�G�X�g���e�ƈ�v���Ă��邩����
            //���̐����E�񐬗��̃t���O����v���Ă��Ȃ���Βe��
            if (createRole != quest.Establish.CreateRole)
            {
                quest.ResetContinueCount();
                return false;
            }
            //���S���݂̂̏ꍇ�A���S��������
            if (quest.Establish.OnlyPerfectRole && dangos.Count != currentMaxDango)
            {
                quest.ResetContinueCount();
                return false;
            }
            //�w��F�����邩����
            if (!HasReadColor(quest, dangos.Distinct()))
            {
                quest.ResetContinueCount();
                return false;
            }

            //������񐔂��J�E���g���āc
            quest.AddMadeCount();
            //����ɘA�������񐔂��J�E���g���āc
            quest.AddContinueCount();

            //�w��񐔍����������
            if (!quest.IsAchievedMadeCount()) return false;
            if (!quest.IsAchievedContinueCount()) return false;

            //�������ׂăN���A�����ꍇ�A�N�G�X�g�����Ƃ��ĕԋp
            QuestSucceed(quest);
            return true;
        }
        private bool CheckQuestSucceedSr(QuestCreateRole quest, Role<int> role)
        {
            //�s���ȃA�N�Z�X�ł���Βe��
            if (quest.CRType != QuestCreateRole.CreateRoleType.SpecifyTheRole) return false;

            //�H�ׂ��c�q���N�G�X�g���e�ƈ�v���Ă��邩����
            //������v���Ă��邩����
            if (quest.SpecifyRole.RoleName != role.GetRolename()) return false;

            //������񐔂��J�E���g���āc
            quest.AddMadeCount();

            //�w��񐔍����������
            if (!quest.IsAchievedMadeCount()) return false;

            //�������ׂăN���A�����ꍇ�A�N�G�X�g�����Ƃ��ĕԋp
            QuestSucceed(quest);
            return true;
        }
        private bool CheckQuestSucceedIc(QuestCreateRole quest, List<DangoColor> colors)
        {
            //�s���ȃA�N�Z�X�ł���Βe��
            if (quest.CRType != QuestCreateRole.CreateRoleType.IncludeColor) return false;

            //�F�̐��𔻒�
            if (colors.Distinct().Count() != quest.IncludeColors.ColorCount) return false;

            //������񐔂��J�E���g���āc
            quest.AddMadeCount();

            //�w��񐔍����������
            if (!quest.IsAchievedMadeCount()) return false;

            //�������ׂăN���A�����ꍇ�A�N�G�X�g�����Ƃ��ĕԋp
            QuestSucceed(quest);
            return true;
        }
        private bool CheckQuestSucceedSm(QuestCreateRole quest, Role<int> role)
        {
            //�s���ȃA�N�Z�X�ł���Βe��
            if (quest.CRType != QuestCreateRole.CreateRoleType.CreateSameRole) return false;

            if (!quest.SameRole.IsEqualRole(role))
            {
                quest.ResetContinueCount();

                return false;
            }

            quest.AddContinueCount();

            //�w��񐔍����������
            if (!quest.IsAchievedContinueCount()) return false;

            //�������ׂăN���A�����ꍇ�A�N�G�X�g�����Ƃ��ĕԋp
            QuestSucceed(quest);

            return true;
        }
        #endregion

        #region PlayAction
        public bool CheckQuestPlayActionSucceed(QuestManager questManager, QuestPlayAction.PlayerAction action)
        {
            //���̃t���[���ɕʂ̃N�G�X�g���N���A����Ă�����e��
            if (isSucceedThisFrame) return false;

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
            //���肵�����A�N�V�������قȂ�����e��
            if (quest.Action != action) return false;

            quest.AddMadeCount();

            //�w��񐔍����������
            if (!quest.IsAchievedMadeCount()) return false;

            QuestSucceed(quest);
            return true;
        }
        #endregion

        #region Destination
        public bool CheckQuestDestinationSucceed(FloorManager.Floor floor, bool inFloor)
        {
            //���̃t���[���ɕʂ̃N�G�X�g���N���A����Ă�����e��
            if (isSucceedThisFrame) return false;

            for (int i = 0; i < _manager.QuestsCount; i++)
            {
                //�L���X�g�\�����m�F�i�s�\�ȏꍇ�G���[���N���邽�߂��̏����͕K�{�j
                if (_manager.GetQuest(i) is QuestDestination questDest)
                {
                    if (CheckQuestSucceed(questDest, floor, inFloor)) return true;
                }
            }

            return false;
        }

        public bool CheckQuestDestinationSucceed()
        {
            //���̃t���[���ɕʂ̃N�G�X�g���N���A����Ă�����e��
            if (isSucceedThisFrame) return false;

            for (int i = 0; i < _manager.QuestsCount; i++)
            {
                //�L���X�g�\�����m�F�i�s�\�ȏꍇ�G���[���N���邽�߂��̏����͕K�{�j
                if (_manager.GetQuest(i) is QuestDestination questDest)
                {
                    if (CheckQuestSucceed(questDest)) return true;
                }
            }

            return false;
        }

        private bool CheckQuestSucceed(QuestDestination quest, FloorManager.Floor floor, bool inFloor)
        {
            //�͂��߂Ɍ��݂���Floor��o�^����
            if (inFloor) quest.SetFloor(floor);

            //�ړI�n�łȂ���Βe��
            if (!quest.Floors.Contains(floor)) return false;

            //�����̏o������L�^
            quest.SetIsInFloor(inFloor);

            //�ړI�n�ɂ������ł����̂��A���ĐH�ׂȂ��Ƃ����Ȃ��̂�����
            if (quest.SucceedOnEat) return false;

            if (!quest.IsInFloor) return false;

            QuestSucceed(quest);
            return true;
        }
        private bool CheckQuestSucceed(QuestDestination quest)
        {
            //�ړI�n�łȂ���Βe��
            if (!quest.Floors.Contains(quest.CurrentFloor)) return false;

            QuestSucceed(quest);
            return true;
        }
        #endregion

        private async void QuestSucceed(QuestData quest)
        {
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

            //���̃t���[���ő��̔���͍s��Ȃ��悤�ɂ��鏈��
            isSucceedThisFrame = true;
            SetBoolAfterOneFrame(false).Forget();

            if (quest.IsKeyQuest)
            {
                //TODO:S7�ɑJ��
            }

            Logger.Log(quest.QuestName + " �N�G�X�g�N���A�I");

            //�N�G�X�g��B�������Ƃ��̉��o
            _playerUIManager.EventText.TextData.SetText("�c���B��");
            _playerUIManager.EventText.TextData.SetFontSize(210f);
            
            await _playerUIManager.EventText.TextData.Fadeout(0.5f, 2f);

            _playerUIManager.EventText.TextData.SetFontSize(_playerUIManager.defaultEventTextFontSize);
        }
    }
}