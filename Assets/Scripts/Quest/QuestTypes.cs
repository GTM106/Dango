using System;
using System.Collections.Generic;

namespace Dango.Quest
{
    class QuestEatDango : QuestData
    {
        int _specifyCount;          //�H�ׂ鐔
        int _eatCount;              //�H�ׂ���
        int _continueCount;         //�A�����č�鐔
        int _currentContinueCount;  //�A�����č������

        bool _allowCountCreateRole;   //����������Ƃ��ɃJ�E���g���邩
        bool _allowCountNoCreateRole; //���Ȃ������Ƃ��ɃJ�E���g���邩

        bool _isPrebCreateRole;     //���O�ɖ�������ĐH�ׂ���

        List<DangoColor> _colors = new(); //���̐F�����ǂݎ��

        public QuestEatDango(int id, int specifyCount, int continueCount, bool canCountCreateRole, bool canCountNoCreateRole, string questName, float rewardTime, bool enableDangoCountUp, bool isKeyQuest, PortraitTextData questTextData, int[] nextQuestId)
            : base(id, QuestType.CreateRole, questName, rewardTime, enableDangoCountUp, isKeyQuest, questTextData, nextQuestId)
        {
            _specifyCount = specifyCount;
            _continueCount = continueCount;
            _allowCountCreateRole = canCountCreateRole;
            _allowCountNoCreateRole = canCountNoCreateRole;

            for (DangoColor i = DangoColor.None + 1; i < DangoColor.Other; i++)
                _colors.Add(i);
        }
        public QuestEatDango(int id, List<DangoColor> colors, int specifyCount, int continueCount, bool canCountCreateRole, bool canCountNoCreateRole, string questName, float rewardTime, bool enableDangoCountUp, bool isKeyQuest, PortraitTextData questTextData, int[] nextQuestId)
            : base(id, QuestType.CreateRole, questName, rewardTime, enableDangoCountUp, isKeyQuest, questTextData, nextQuestId)
        {
            _specifyCount = specifyCount;
            _continueCount = continueCount;
            _allowCountCreateRole = canCountCreateRole;
            _allowCountNoCreateRole = canCountNoCreateRole;

            _colors = colors;
        }
        public QuestEatDango(int id, DangoColor[] colors, int specifyCount, int continueCount, bool canCountCreateRole, bool canCountNoCreateRole, string questName, float rewardTime, bool enableDangoCountUp, bool isKeyQuest, PortraitTextData questTextData, int[] nextQuestId)
            : base(id, QuestType.CreateRole, questName, rewardTime, enableDangoCountUp, isKeyQuest, questTextData, nextQuestId)
        {
            _specifyCount = specifyCount;
            _continueCount = continueCount;
            _allowCountCreateRole = canCountCreateRole;
            _allowCountNoCreateRole = canCountNoCreateRole;

            _colors.AddRange(colors);
        }
        public QuestEatDango(int id, DangoColor color, int specifyCount, int continueCount, bool canCountCreateRole, bool canCountNoCreateRole, string questName, float rewardTime, bool enableDangoCountUp, bool isKeyQuest, PortraitTextData questTextData, int[] nextQuestId)
            : base(id, QuestType.CreateRole, questName, rewardTime, enableDangoCountUp, isKeyQuest, questTextData, nextQuestId)
        {
            _specifyCount = specifyCount;
            _continueCount = continueCount;
            _allowCountCreateRole = canCountCreateRole;
            _allowCountNoCreateRole = canCountNoCreateRole;

            _colors.Add(color);
        }

        public bool IsAchievedEatCount() => _specifyCount <= _eatCount;
        public bool IsAchievedContinueCount() => _continueCount <= _currentContinueCount;
        public int SpecifyCount => _specifyCount;
        public int ContinueCount => _continueCount;
        public bool AllowCountCreateRole => _allowCountCreateRole;
        public bool AllowCountNoCreateRole => _allowCountNoCreateRole;
        public bool IsPrebCreateRole => _isPrebCreateRole;
        public List<DangoColor> ReadColors => _colors;
        public void AddEatCount()
        {
            _eatCount++;
        }

        public void AddEatCount(int count)
        {
            _eatCount += count;
        }

        public void AddContinueCount() => _currentContinueCount++;
        public void ResetContinueCount() => _currentContinueCount = 0;
        public void SetIsPrebCreateRole(bool isPrebCreateRole) => _isPrebCreateRole = isPrebCreateRole;
        public override float SpecifyProgress()
        {
            return (float)_eatCount / _specifyCount;
        }
        public override float ContinueProgress()
        {
            return (float)_currentContinueCount / _continueCount;
        }
    }

    class QuestCreateRole : QuestData
    {
        public enum CreateRoleType
        {
            EstablishRole,
            SpecifyTheRole,
            IncludeColor,
            CreateSameRole,
        }

        CreateRoleType _type;
        EstablishRole _establishRole;
        SpecifyTheRole _specifyTheRole;
        UseColorCount _includeColor;
        CreateSameRole _createSameRole;

        int _specifyCount;          //��鐔
        int _madeCount;             //�������
        int _continueCount;         //�A�����č�鐔
        int _currentContinueCount;  //�A�����č������

        public QuestCreateRole(int id, EstablishRole establish, int specifyCount, int continueCount, string questName, float rewardTime, bool enableDangoCountUp, bool isKeyQuest, PortraitTextData questTextData, int[] nextQuestId) : base(id, QuestType.CreateRole, questName, rewardTime, enableDangoCountUp, isKeyQuest, questTextData, nextQuestId)
        {
            _type = CreateRoleType.EstablishRole;
            _establishRole = establish;
            _specifyCount = specifyCount;
            _continueCount = continueCount;
        }
        public QuestCreateRole(int id, SpecifyTheRole specifyTheRole, int specifyCount, int continueCount, string questName, float rewardTime, bool enableDangoCountUp, bool isKeyQuest, PortraitTextData questTextData, int[] nextQuestId) : base(id, QuestType.CreateRole, questName, rewardTime, enableDangoCountUp, isKeyQuest, questTextData, nextQuestId)
        {
            _type = CreateRoleType.SpecifyTheRole;
            _specifyTheRole = specifyTheRole;
            _specifyCount = specifyCount;
            _continueCount = continueCount;
        }
        public QuestCreateRole(int id, UseColorCount includeColor, int specifyCount, int continueCount, string questName, float rewardTime, bool enableDangoCountUp, bool isKeyQuest, PortraitTextData questTextData, int[] nextQuestId) : base(id, QuestType.CreateRole, questName, rewardTime, enableDangoCountUp, isKeyQuest, questTextData, nextQuestId)
        {
            _type = CreateRoleType.IncludeColor;
            _includeColor = includeColor;
            _specifyCount = specifyCount;
            _continueCount = continueCount;
        }
        public QuestCreateRole(int id, CreateSameRole createSameRole, int specifyCount, int continueCount, string questName, float rewardTime, bool enableDangoCountUp, bool isKeyQuest, PortraitTextData questTextData, int[] nextQuestId) : base(id, QuestType.CreateRole, questName, rewardTime, enableDangoCountUp, isKeyQuest, questTextData, nextQuestId)
        {
            _type = CreateRoleType.CreateSameRole;
            _createSameRole = createSameRole;
            _specifyCount = specifyCount;
            _continueCount = continueCount;
        }

        public CreateRoleType CRType => _type;

        public EstablishRole Establish => _establishRole;
        public SpecifyTheRole SpecifyRole => _specifyTheRole;
        public UseColorCount IncludeColors => _includeColor;
        public CreateSameRole SameRole => _createSameRole;


        public bool IsAchievedMadeCount() => _specifyCount <= _madeCount;
        public bool IsAchievedContinueCount() => _continueCount <= _currentContinueCount;
        public int SpecifyCount => _specifyCount;
        public int ContinueCount => _continueCount;
        public void AddMadeCount()
        {
            _madeCount++;
        }

        public void AddContinueCount() => _currentContinueCount++;
        public void ResetContinueCount() => _currentContinueCount = 0;
        public override float SpecifyProgress()
        {
            return (float)_madeCount / _specifyCount;
        }
        public override float ContinueProgress()
        {
            return (float)_currentContinueCount / _continueCount;
        }

        /// <summary>
        /// ���𐬗�������i���e�s��j�n�̃N�G�X�g
        /// </summary>
        public class EstablishRole
        {
            bool _createRole;                   //�����u������Ƃ��v���u���Ȃ������Ƃ��v��
            bool _onlyPerfectRole;              //���S���iD5�����݂̃}�b�N�X�̂Ƃ������j�̂�
            List<DangoColor> _readColors = new();   //���̐F�����ǂݎ��
            bool _isPrebCreateRole;             //���O�ɖ�������ĐH�ׂ���

            static readonly DangoColor[] dangoColors = { DangoColor.An, DangoColor.Beni, DangoColor.Mitarashi, DangoColor.Nori, DangoColor.Yuzu, DangoColor.Yomogi };

            public EstablishRole(bool createRole, bool onlyPerfectRole, params DangoColor[] colors)
            {
                _createRole = createRole;
                _onlyPerfectRole = onlyPerfectRole;
                _readColors.AddRange(colors);
            }
            public EstablishRole(bool createRole, bool onlyPerfectRole)
            {
                _createRole = createRole;
                _onlyPerfectRole = onlyPerfectRole;
                _readColors.AddRange(dangoColors);
            }

            public bool CreateRole => _createRole;
            public bool OnlyPerfectRole => _onlyPerfectRole;
            public List<DangoColor> ReadColors => _readColors;
            public bool IsPrebCreateRole => _isPrebCreateRole;
            public void SetIsPrebCreateRole(bool isPrebCreateRole) => _isPrebCreateRole = isPrebCreateRole;
        }

        /// <summary>
        /// ���𐬗�������i���e�w��j�n�̃N�G�X�g
        /// </summary>
        public class SpecifyTheRole
        {
            string _roleName;

            public SpecifyTheRole(string roleName)
            {
                _roleName = roleName;
            }

            public string RoleName => _roleName;
        }

        /// <summary>
        /// �F�̐��𔻒�Ɏg���n�̃N�G�X�g
        /// </summary>
        public class UseColorCount
        {
            int _colorCount;

            public UseColorCount(int colorCount)
            {
                _colorCount = colorCount;
            }

            public int ColorCount => _colorCount;
        }

        /// <summary>
        /// �������i�Ⴄ��)��A���ō��n�̃N�G�X�g
        /// </summary>
        public class CreateSameRole
        {
            bool _sameRole;
            Role<int> _prebRole;

            public CreateSameRole(bool sameRole)
            {
                _sameRole = sameRole;
            }

            public bool SameRole => _sameRole;
            public bool IsEqualRole(Role<int> role)
            {
                if (_prebRole == null)
                {
                    _prebRole = role;

                    //����͕K������������
                    return _sameRole;
                }

                bool isEqual = role.GetRolename() == _prebRole.GetRolename();

                _prebRole = role;

                return isEqual;
            }
        }
    }

    class QuestPlayAction : QuestData
    {
        public enum PlayerAction
        {
            FallAttack,
            Stab,
        }

        PlayerAction _action;
        int _specifyCount;
        int _madeCount;

        public QuestPlayAction(int id, PlayerAction action, int specifyCount, string questName, float rewardTime, bool enableDangoCountUp, bool isKeyQuest, PortraitTextData questTextData, int[] nextQuestId) : base(id, QuestType.PlayAction, questName, rewardTime, enableDangoCountUp, isKeyQuest, questTextData, nextQuestId)
        {
            _action = action;
            _specifyCount = specifyCount;
        }

        public PlayerAction Action => _action;
        public bool IsAchievedMadeCount() => _specifyCount <= _madeCount;
        public int SpecifyCount => _specifyCount;
        public void AddMadeCount()
        {
            _madeCount++;
        }

        public override float SpecifyProgress()
        {
            return (float)_madeCount / _specifyCount;
        }
        public override float ContinueProgress()
        {
            return 0;
        }
    }

    class QuestDestination : QuestData
    {
        List<FloorManager.Floor> _floors = new();

        bool _onEatSucceed;
        bool _isInFloor;
        FloorManager.Floor _currentFloor;

        public QuestDestination(int id, FloorManager.Floor floor, bool onEatSucceed, string questName, float rewardTime, bool enableDangoCountUp, bool isKeyQuest, PortraitTextData questTextData, int[] nextQuestId) : base(id, QuestType.Destination, questName, rewardTime, enableDangoCountUp, isKeyQuest, questTextData, nextQuestId)
        {
            _floors.Add(floor);
            _onEatSucceed = onEatSucceed;
        }
        public QuestDestination(int id, FloorManager.Floor[] floors, bool onEatSucceed, string questName, float rewardTime, bool enableDangoCountUp, bool isKeyQuest, PortraitTextData questTextData, int[] nextQuestId) : base(id, QuestType.Destination, questName, rewardTime, enableDangoCountUp, isKeyQuest, questTextData, nextQuestId)
        {
            _floors.AddRange(floors);
            _onEatSucceed = onEatSucceed;
        }

        public List<FloorManager.Floor> Floors => _floors;
        public FloorManager.Floor CurrentFloor => _currentFloor;
        public bool SucceedOnEat => _onEatSucceed;
        public bool IsInFloor => _isInFloor;
        public void SetIsInFloor(bool enable) => _isInFloor = enable;
        public void SetFloor(FloorManager.Floor floor) => _currentFloor = floor;
        public override float SpecifyProgress()
        {
            //�i�����͑��݂��Ȃ�
            return 0;
        }
        public override float ContinueProgress()
        {
            return 0;
        }
    }
}