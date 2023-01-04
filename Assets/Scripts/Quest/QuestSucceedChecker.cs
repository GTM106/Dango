using System.Collections.Generic;
using Dango.Quest.UI;
using System.Linq;
using Cysharp.Threading.Tasks;

namespace Dango.Quest
{
    class QuestSucceedChecker
    {
        QuestManager _manager;
        bool _isSucceedThisFrame;

        PlayerUIManager _playerUIManager;
        IChangePortrait _portraitScript;
        StageData _stageData;
        TutorialUIManager _tutorialUIManager;
        QuestSucceedUIManager _questSucceedUIManager;
        QuestExpansionUIManager _questExpansionUIManager;

        public QuestSucceedChecker(QuestManager manager, PlayerUIManager playerUIManager, IChangePortrait portraitScript, StageData stageData, TutorialUIManager tutorialUIManager, QuestSucceedUIManager questSucceedUIManager, QuestExpansionUIManager questExpansionUIManager)
        {
            _manager = manager;
            _playerUIManager = playerUIManager;
            _portraitScript = portraitScript;
            _stageData = stageData;
            _tutorialUIManager = tutorialUIManager;
            _questSucceedUIManager = questSucceedUIManager;
            _questExpansionUIManager = questExpansionUIManager;
        }

        #region EatDango
        public bool CheckQuestEatDangoSucceed(QuestManager questManager, List<DangoColor> colors, bool createRole)
        {
            if (_isSucceedThisFrame) return false;

            for (int i = 0; i < questManager.QuestsCount; i++)
            {
                if (questManager.GetQuest(i) is QuestEatDango questEa)
                {
                    if (CheckQuestSucceed(questEa, colors, createRole)) return true;
                }
            }

            return false;
        }

        private bool CheckQuestSucceed(QuestEatDango quest, List<DangoColor> colors, bool createRole)
        {
            if (createRole && !quest.AllowCountCreateRole || !createRole && !quest.AllowCountNoCreateRole)
            {
                quest.SetIsPrebCreateRole(createRole);
                return false;
            }

            foreach (var color in colors)
            {
                if (!quest.ReadColors.Contains(color)) continue;

                quest.AddEatCount();
            }

            if (quest.IsPrebCreateRole != createRole) quest.AddContinueCount();
            else quest.ResetContinueCount();

            if (quest.ContinueCount <= 1) _questExpansionUIManager.OnNext(quest, quest.SpecifyProgress());
            else _questExpansionUIManager.OnNext(quest, quest.ContinueProgress());

            quest.SetIsPrebCreateRole(createRole);

            if (!quest.IsAchievedEatCount()) return false;
            if (!quest.IsAchievedContinueCount()) return false;

            QuestSucceed(quest);
            return true;
        }
        #endregion

        #region CreateRole
        public bool CheckQuestCreateRoleSucceedEs(List<DangoColor> dangos, bool createRole, int currentMaxDango)
        {
            if (_isSucceedThisFrame) return false;

            for (int i = 0; i < _manager.QuestsCount; i++)
            {
                if (_manager.GetQuest(i) is QuestCreateRole questCr)
                {
                    if (CheckQuestSucceedEs(questCr, dangos, createRole, currentMaxDango)) return true;
                }
            }

            return false;
        }
        public bool CheckQuestCreateRoleSucceedSr(Role<int> role)
        {
            if (_isSucceedThisFrame) return false;

            for (int i = 0; i < _manager.QuestsCount; i++)
            {
                if (_manager.GetQuest(i) is QuestCreateRole questCr)
                {
                    if (CheckQuestSucceedSr(questCr, role)) return true;
                }
            }

            return false;
        }
        public bool CheckQuestCreateRoleSucceedIr(List<DangoColor> dangos)
        {
            if (_isSucceedThisFrame) return false;

            for (int i = 0; i < _manager.QuestsCount; i++)
            {
                if (_manager.GetQuest(i) is QuestCreateRole questCr)
                {
                    if (CheckQuestSucceedIc(questCr, dangos)) return true;
                }
            }

            return false;
        }
        public bool CheckQuestCreateRoleSucceedSm(Role<int> role)
        {
            if (_isSucceedThisFrame) return false;

            for (int i = 0; i < _manager.QuestsCount; i++)
            {
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
                if (quest.Establish.ReadColors.Contains(color)) return true;
            }

            return false;
        }

        private bool CheckQuestSucceedEs(QuestCreateRole quest, List<DangoColor> dangos, bool createRole, int currentMaxDango)
        {
            if (quest.CRType != QuestCreateRole.CreateRoleType.EstablishRole) return false;

            if (createRole != quest.Establish.CreateRole)
            {
                quest.ResetContinueCount();
                return false;
            }
            if (quest.Establish.OnlyPerfectRole && dangos.Count != currentMaxDango)
            {
                quest.ResetContinueCount();
                return false;
            }
            if (!HasReadColor(quest, dangos.Distinct()))
            {
                quest.ResetContinueCount();
                return false;
            }

            quest.AddMadeCount();
            quest.AddContinueCount();

            if (quest.ContinueCount <= 1) _questExpansionUIManager.OnNext(quest, quest.SpecifyProgress());
            else _questExpansionUIManager.OnNext(quest, quest.ContinueProgress());

            if (!quest.IsAchievedMadeCount()) return false;
            if (!quest.IsAchievedContinueCount()) return false;

            QuestSucceed(quest);
            return true;
        }
        private bool CheckQuestSucceedSr(QuestCreateRole quest, Role<int> role)
        {
            if (quest.CRType != QuestCreateRole.CreateRoleType.SpecifyTheRole) return false;

            if (quest.SpecifyRole.RoleName != role.GetRolename()) return false;

            quest.AddMadeCount();

            _questExpansionUIManager.OnNext(quest, quest.SpecifyProgress());

            if (!quest.IsAchievedMadeCount()) return false;

            QuestSucceed(quest);
            return true;
        }
        private bool CheckQuestSucceedIc(QuestCreateRole quest, List<DangoColor> colors)
        {
            if (quest.CRType != QuestCreateRole.CreateRoleType.IncludeColor) return false;

            if (colors.Distinct().Count() != quest.IncludeColors.ColorCount) return false;

            quest.AddMadeCount();

            _questExpansionUIManager.OnNext(quest, quest.SpecifyProgress());

            if (!quest.IsAchievedMadeCount()) return false;

            QuestSucceed(quest);
            return true;
        }
        private bool CheckQuestSucceedSm(QuestCreateRole quest, Role<int> role)
        {
            if (quest.CRType != QuestCreateRole.CreateRoleType.CreateSameRole) return false;

            if (quest.SameRole.SameRole)
            {
                if (!quest.SameRole.IsEqualRole(role))
                {
                    quest.ResetContinueCount();
                    _questExpansionUIManager.OnNext(quest, quest.ContinueProgress());

                    return false;
                }
            }
            else
            {
                if (quest.SameRole.IsEqualRole(role))
                {
                    quest.ResetContinueCount();
                    _questExpansionUIManager.OnNext(quest, quest.ContinueProgress());

                    return false;
                }
            }

            quest.AddContinueCount();
            _questExpansionUIManager.OnNext(quest, quest.ContinueProgress());

            if (!quest.IsAchievedContinueCount()) return false;

            QuestSucceed(quest);

            return true;
        }
        #endregion

        #region PlayAction
        public bool CheckQuestPlayActionSucceed(QuestPlayAction.PlayerAction action)
        {
            if (_isSucceedThisFrame) return false;

            for (int i = 0; i < _manager.QuestsCount; i++)
            {
                if (_manager.GetQuest(i) is QuestPlayAction questPla)
                {
                    if (CheckQuestSucceed(questPla, action)) return true;
                }
            }

            return false;
        }

        private bool CheckQuestSucceed(QuestPlayAction quest, QuestPlayAction.PlayerAction action)
        {
            if (quest.Action != action) return false;

            quest.AddMadeCount();

            if (action != QuestPlayAction.PlayerAction.Look)
                _questExpansionUIManager.OnNext(quest, quest.SpecifyProgress());

            if (!quest.IsAchievedMadeCount()) return false;

            QuestSucceed(quest);
            return true;
        }
        #endregion

        #region Destination
        public bool CheckQuestDestinationSucceed(FloorManager.Floor floor, bool inFloor)
        {
            if (_isSucceedThisFrame) return false;

            for (int i = 0; i < _manager.QuestsCount; i++)
            {
                if (_manager.GetQuest(i) is QuestDestination questDest)
                {
                    if (CheckQuestSucceed(questDest, floor, inFloor)) return true;
                }
            }

            return false;
        }

        public bool CheckQuestDestinationSucceed()
        {
            if (_isSucceedThisFrame) return false;

            for (int i = 0; i < _manager.QuestsCount; i++)
            {
                if (_manager.GetQuest(i) is QuestDestination questDest)
                {
                    if (CheckQuestSucceed(questDest)) return true;
                }
            }

            return false;
        }

        private bool CheckQuestSucceed(QuestDestination quest, FloorManager.Floor floor, bool inFloor)
        {
            if (inFloor) quest.SetFloor(floor);

            if (!quest.Floors.Contains(floor)) return false;

            quest.SetIsInFloor(inFloor);

            if (quest.SucceedOnEat) return false;

            if (!quest.IsInFloor) return false;

            QuestSucceed(quest);
            return true;
        }
        private bool CheckQuestSucceed(QuestDestination quest)
        {
            if (!quest.Floors.Contains(quest.CurrentFloor)) return false;

            QuestSucceed(quest);
            return true;
        }
        #endregion

        public void QuestSkip()
        {
            QuestSucceed(_manager.GetQuest(0));
        }

        private void QuestSucceed(QuestData quest)
        {
            SoundManager.Instance.PlaySE(SoundSource.SE12_QUEST_SUCCEED);

            List<QuestData> nextQuest = new();
            for (int i = 0; i < quest.NextQuestId.Count; i++)
            {
                nextQuest.Add(_stageData.QuestData[quest.NextQuestId[i]]);
            }

            _manager.ChangeQuest(nextQuest);
            _manager.Player.GrowStab(quest.EnableDangoCountUp);
            _manager.Player.AddSatiety(quest.RewardTime);
            _playerUIManager.ScoreCatch(quest.RewardTime);

            ScoreManager.Instance.AddClearTime(ScoreManager.Instance.SetQuestTime());
            ScoreManager.Instance.AddClearQuest(quest);

            _isSucceedThisFrame = true;
            SetBoolAfterOneFrame(false).Forget();

            _portraitScript.ChangePortraitText(quest.QuestTextDatas).Forget();

            if (quest.IsKeyQuest)
            {
                _manager.SetIsComplete();
                _stageData.OnStageSucceed();

                return;
            }

            Logger.Log(quest.QuestName + "クエストクリア！");

            _manager.CreateExpansionUIObj();

            if (_tutorialUIManager != null)
            {
                _tutorialUIManager.ChangeNextGuide(quest.NextQuestId[0]);
            }

            if (quest.IsKeyQuest)
            {
                //最後のクエストなら次のクエストはない
                _questSucceedUIManager.PlayAnimation();
            }
            else
            {
                _questSucceedUIManager.PlayAnimation(nextQuest.ToArray());
            }
        }

        private async UniTask SetBoolAfterOneFrame(bool enable)
        {
            await UniTask.Yield();

            _isSucceedThisFrame = enable;
        }
    }
}