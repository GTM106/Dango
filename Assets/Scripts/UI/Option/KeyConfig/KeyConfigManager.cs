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

        [SerializeField,Tooltip("�����ɑI������Ă���L�[")] KeyConfigData _firstData = default!;

        //�X�e�B�b�N��L�[�{�[�h�̈ړ����͓ǂݎ��p
        Vector2 _axis;

        //�\���E��\���؂�ւ��p�ɊǗ��������
        [SerializeField] Canvas _staticCanvas = default!;
        [SerializeField] Canvas _dynamicCanvas = default!;
        [SerializeField] KeyConfigPopupManager _popupManager = default!;

        //InputSystem��InputActions�{��
        [SerializeField] InputActionAsset _asset;

        //�A�N�V�������t�@�����X���ɐݒ肷��f�[�^�̑S�v�f
        ActionData[] _actionDatas = new ActionData[(int)KeyConfigData.GameAction.Max];

        public void OnStick()
        {
            if (!_staticCanvas.isActiveAndEnabled) return;
            if (!_dynamicCanvas.isActiveAndEnabled) return;
            if (_popupManager.IsPopup) return;

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
                _currentData.GetComponent<RawImage>().color = Color.white;
                _currentData = _firstData;
                _currentData.GetComponent<RawImage>().color = Color.red;
            }
        }

        //���ڂ�I�������ۂ̂���
        public void OnSelect()
        {
            _popupManager.OnCanvasEnabled();
        }

        /// <summary>
        /// �|�b�v�A�b�v�����ۂ��ŋ������ς��B
        /// </summary>
        /// <returns>false:�|�b�v�A�b�v��</returns>
        public bool OnBack()
        {
            if (_popupManager.IsPopup)
            {
                _popupManager.OnCanvasDisabled();
                return false;
            }

            return true;
        }

        public void Rebinding(int index)
        {
            _currentData.GetComponent<KeyData>().KeyBindingOverride(_actionDatas[index].ActionReference);
        }

        public KeyConfigData Data => _currentData;
    }
}