using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace TM.Input.KeyConfig
{
    enum InputKey
    {
        Up,
        UpperRight,
        Right,
        LowerRight,
        Down,
        LowerLeft,
        Left,
        UpperLeft,

        Unknown,
    }

    public class KeyConfigManager : MonoBehaviour
    {
        //���ݑI������Ă���ύX�������L�[�̃f�[�^
        KeyConfigData _currentData;

        [SerializeField, Tooltip("�����ɑI������Ă���L�[")] KeyConfigData _firstData = default!;

        //�X�e�B�b�N��L�[�{�[�h�̈ړ����͓ǂݎ��p
        Vector2 _axis;

        //�\���E��\���؂�ւ��p�ɊǗ��������
        [SerializeField] Canvas _staticCanvas = default!;
        [SerializeField] Canvas _dynamicCanvas = default!;
        [SerializeField] KeyConfigWarningManager _warningManager = default!;
        [SerializeField] KeyConfigPopupManager _popupManager = default!;

        //InputSystem��InputActions�{��
        [SerializeField] InputActionAsset _asset;

        //�A�N�V�������t�@�����X���ɐݒ肷��f�[�^�̑S�v�f
        readonly ActionData[] _actionDatas = new ActionData[(int)KeyConfigData.GameAction.Max];
        readonly List<KeyData.GameAction> _gameActions = new();

        public void OnStick()
        {
            if (!_staticCanvas.isActiveAndEnabled) return;
            if (!_dynamicCanvas.isActiveAndEnabled) return;
            if (_popupManager.IsPopup) return;
            if (_warningManager.IsWarning) return;

            //�ǂݎ�����l��ۑ�
            _axis = InputSystemManager.Instance.StickAxis;

            //�ǂݎ�����l����ł��߂�8�����̂����ꂩ��ԋp
            InputKey key = CheckInputKey(_axis);
            if (key == InputKey.Unknown) return;

            ///���ɉ���null�������ꍇaxis.x�Œ����Ƃ�����Ȃ炱���ɏ������򂷂邵�����̂Ƃ��떳���B
            ///�Ƃ肠������a���̂���I���ɂ͂Ȃ邩������Ȃ����A�������������ŁB
            //null�`�F�b�N�Bnull�̏ꍇ�͏�L�ݒ���s���������R�ł���B
            if (_currentData.GetKeyConfigDatas()[(int)key] == null) return;

            //�������|�b�v�A�b�v�ɕς���
            _currentData.GetComponent<RawImage>().color = Color.white;

            //���݂̃f�[�^��I�������f�[�^�ɕύX
            _currentData = _currentData.GetKeyConfigDatas()[(int)key];

            //�������|�b�v�A�b�v�ɕς���
            _currentData.GetComponent<RawImage>().color = Color.red;

            Logger.Log(_currentData.name);
        }

        static readonly KeyData.GamepadKey[][] DefaultKeyTable = new KeyData.GamepadKey[(int)KeyConfigData.GameAction.Max][]
        {
           new KeyData.GamepadKey[] {KeyData.GamepadKey.LStick },                                       //Move
           new KeyData.GamepadKey[] {KeyData.GamepadKey.RStick },                                       //Look
           new KeyData.GamepadKey[] {KeyData.GamepadKey.ButtonSouth,KeyData.GamepadKey.ButtonEast },    //Jump
           new KeyData.GamepadKey[] {KeyData.GamepadKey.ButtonNorth,KeyData.GamepadKey.ButtonWest },    //Attack
           new KeyData.GamepadKey[] {KeyData.GamepadKey.LTrigger },                                     //EatDango
           new KeyData.GamepadKey[] {KeyData.GamepadKey.RTrigger },                                     //Fire
           new KeyData.GamepadKey[] {KeyData.GamepadKey.Start,KeyData.GamepadKey.Select },              //Option
           new KeyData.GamepadKey[] {KeyData.GamepadKey.R },                                            //UIExtra(not Found)
        };

        private void Awake()
        {
            InitConfigData();

            for (int i = 0; i < (int)KeyConfigData.GameAction.Max; i++)
            {
                _actionDatas[i] = new((KeyData.GameAction)i, DefaultKeyTable[i], _asset, "Player");
            }
        }

        private void Start()
        {
            InputSystemManager.Instance.onStickPerformed += OnStick;
            InputSystemManager.Instance.onChoicePerformed += OnSelect;
            InputSystemManager.Instance.onBackCanceled += OnBack;
        }

        public void OnChangeScene()
        {
            _popupManager.OnChangeScene();
            InputSystemManager.Instance.onStickPerformed -= OnStick;
            InputSystemManager.Instance.onChoicePerformed -= OnSelect;
            InputSystemManager.Instance.onBackCanceled -= OnBack;
        }

        //���̊֐���Awake�ɒu���Β��O�ɔ������n�_���ۑ�����Ă�������ړ��ł��܂��B
        //�t�ɖ��񏉊����������ꍇ��OnEnable�ɔz�u���Ă��������B
        private void InitConfigData()
        {
            _currentData = _firstData;
        }

        const float RAD = 90f;

        private InputKey CheckInputKey(Vector2 axis)
        {
            //���������Ă��ꂼ��U�蕪���Ă��܂��B

            if (axis.x > 0)
            {
                if (axis.y is >= -90f / RAD and < -67.5f / RAD) return InputKey.Down;
                if (axis.y is >= -67.5f / RAD and < -22.5f / RAD) return InputKey.LowerRight;
                if (axis.y is >= -22.5f / RAD and < 22.5f / RAD) return InputKey.Right;
                if (axis.y is >= 22.5f / RAD and < 67.5f / RAD) return InputKey.UpperRight;
                if (axis.y is >= 67.5f / RAD and <= 90.0f / RAD) return InputKey.Up;
            }
            if (axis.x <= 0)
            {
                if (axis.y is >= -90.0f / RAD and < -67.5f / RAD) return InputKey.Down;
                if (axis.y is >= -67.5f / RAD and < -22.5f / RAD) return InputKey.LowerLeft;
                if (axis.y is >= -22.5f / RAD and < 22.5f / RAD) return InputKey.Left;
                if (axis.y is >= 22.5f / RAD and < 67.5f / RAD) return InputKey.UpperLeft;
                if (axis.y is >= 67.5f / RAD and <= 90.0f / RAD) return InputKey.Up;
            }

            Logger.Error("[Error]:����U���Ă��Ȃ����̂����݂��Ă��܂��B");
            return InputKey.Unknown;
        }

        /// <summary>
        /// Canvas�̕\���E��\����ݒ肷��֐�
        /// </summary>
        public void SetCanvasEnable(bool enable)
        {
            _staticCanvas.enabled = enable;
            _dynamicCanvas.enabled = enable;

            if (enable)
            {
                if (_currentData != null)  _currentData.GetComponent<RawImage>().color = Color.white;
                
                _currentData = _firstData;
                _currentData.GetComponent<RawImage>().color = Color.red;
            }
        }

        private void OnSelect()
        {
            if (!_staticCanvas.isActiveAndEnabled) return;
            if (!_dynamicCanvas.isActiveAndEnabled) return;
            if (IsPopup) return;
            if (_warningManager.IsWarning) return;

            _popupManager.OnCanvasEnabled();
        }

        public void OnBack()
        {
            if (!_staticCanvas.isActiveAndEnabled) return;
            if (!_dynamicCanvas.isActiveAndEnabled) return;

            if (_popupManager.IsPopup)
            {
                _popupManager.OnCanvasDisabled();
                return;
            }

            CheckHasKeyAllActions();
        }

        public void Rebinding(int index)
        {
            foreach (var actionData in _actionDatas)
            {
                if (_currentData.KeyData.CurrentActionReference == null) break;

                if (actionData.ActionReference.ToString().Equals(_currentData.KeyData.CurrentActionReference.ToString()))
                    actionData.Keys.Remove(_currentData.KeyData.Key);
            }
            _actionDatas[index].Keys.Add(_currentData.KeyData.Key);

            _currentData.KeyData.KeyBindingOverride(_actionDatas[index].ActionReference);
        }

        public bool CheckHasKeyAllActions()
        {
            if (_warningManager.IsWarning) return false;

            bool hasKey = true;
            _gameActions.Clear();

            //���ׂẴA�N�V�����ɑ΂��Ċ��蓖�Ă����݂��Ă��邩�`�F�b�N
            foreach (var actionData in _actionDatas)
            {
                //���蓖�Ă��Ȃ������ꍇ
                if (!actionData.HasKey())
                {
                    _gameActions.Add(actionData.Action);
                    hasKey = false;
                }
            }
            if (!hasKey) HasAllAction();

            return hasKey;
        }

        private async void HasAllAction()
        {
            _warningManager.SetEnable(true);
            _warningManager.SetText(_gameActions);
            await WaitForAnyKey();
            _warningManager.SetEnable(false);
        }

        private async UniTask WaitForAnyKey()
        {
            await UniTask.Yield();

            while (true)
            {
                await UniTask.Yield();
                if (InputSystemManager.Instance.WasPressedThisFrameAnyKey) break;
            }
        }

        public KeyConfigData Data => _currentData;
        public bool IsPopup => _popupManager.IsPopup;
    }
}