using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TM.Input.KeyConfig
{
    public class KeyData : MonoBehaviour
    {
        public enum GamepadKey
        {
            ButtonNorth,
            ButtonEast,
            ButtonWest,
            ButtonSouth,
            L,
            R,
            LTrigger,
            RTrigger,
            LStick,
            LStickDown,
            LStickUp,
            LStickLeft,
            LStickRight,
            LStickPress,
            RStick,
            RStickDown,
            RStickUp,
            RStickLeft,
            RStickRight,
            RStickPress,
            D_pad,
            D_padDown,
            D_padUp,
            D_padLeft,
            D_padRight,
            Select,
            Start,
        }

        public enum GameAction
        {
            Move = 0,
            LookRotation = 1,
            Jump = 2,
            Attack = 3,
            Eat = 4,
            Remove = 5,
            Pause = 6,
            UIExpansion = 7,
        }

        [SerializeField] GamepadKey key;
        [SerializeField] GameAction defalutAction;

        [SerializeField] InputActionReference currentActionReference;

        public InputActionReference CurrentActionReference => currentActionReference;
        public GamepadKey Key => key;

        //static readonly string REBINDJSON = "rebindKey";

        //�o�C���f�B���O��Ԃ�JSON�`���ŏo�́A�ǂݍ���
        //���������񂾂��ǂЂƂ܂��킩��Ȃ��̂ň�U���u

        //private bool LoadRebinding()
        //{
        //    //���łɃ��o�C���f�B���O�������Ƃ�����ꍇ�̓V�[���ǂݍ��ݎ��ɕύX�B
        //    string rebinds = PlayerPrefs.GetString(REBINDJSON);

        //    //Null���󔒂ł������烍�[�h���s�Ƃ��ĕԋp
        //    if (string.IsNullOrEmpty(rebinds)) return false;

        //    //���o�C���f�B���O��Ԃ����[�h
        //    _playerInput.actions.LoadBindingOverridesFromJson(rebinds);


        //    return true;
        //}

        //public void RebindComplete()
        //{
        //    //fire�A�N�V������1�Ԗڂ̃R���g���[��(�o�C���f�B���O�����R���g���[��)�̃C���f�b�N�X���擾
        //    int bindingIndex = _action.action.GetBindingIndexForControl(_action.action.controls[0]);

        //    //�o�C���f�B���O�����L�[�̖��̂��擾����
        //    _bindingName.text = InputControlPath.ToHumanReadableString(
        //        _action.action.bindings[bindingIndex].effectivePath,
        //        InputControlPath.HumanReadableStringOptions.OmitDevice);

        //    _rebindingOperation.Dispose();

        //    //��ʂ�ʏ�ɖ߂�
        //    _rebindingButton.SetActive(true);
        //    _rebindingMessage.SetActive(false);

        //    //���o�C���f�B���O���͋�̃A�N�V�����}�b�v�������̂Œʏ�̃A�N�V�����}�b�v�ɐ؂�ւ�
        //    _pInput.SwitchCurrentActionMap("Player");

        //    //���o�C���f�B���O�����L�[��ۑ�(�V�[���J�n���ɓǂݍ��ނ���)
        //    PlayerPrefs.SetString(REBINDJSON, _playerInput.actions.SaveBindingOverridesAsJson());
        //}

        /// <summary>
        /// �I������Ă���KeyData�̃o�C���h��ύX����֐��B
        /// </summary>
        public void KeyBindingOverride(InputActionReference actionReference)
        {
            if (currentActionReference != null)
            {
                currentActionReference.action.ChangeBinding(new InputBinding { path = ToGamepadKeyPass(key) }).Erase();
            }
            actionReference.action.AddBinding(new InputBinding { path = ToGamepadKeyPass(key) });
            currentActionReference = actionReference;

            Logger.Log("�L�[�o�C���h��ύX������I");
        }

        private string ToGamepadKeyPass(GamepadKey gamepadKey)
        {
            return gamepadKey switch
            {
                GamepadKey.ButtonNorth => "<Gamepad>/buttonNorth",
                GamepadKey.ButtonEast => "<Gamepad>/buttonEast",
                GamepadKey.ButtonWest => "<Gamepad>/buttonSouth",
                GamepadKey.ButtonSouth => "<Gamepad>/buttonWest",
                GamepadKey.L => "<Gamepad>/leftShoulder",
                GamepadKey.R => "<Gamepad>/rightShoulder",
                GamepadKey.LTrigger => "<Gamepad>/leftTrigger",
                GamepadKey.RTrigger => "<Gamepad>/rightTrigger",
                GamepadKey.LStick => "<Gamepad>/leftStick",
                GamepadKey.LStickDown => "<Gamepad>/leftStick/down",
                GamepadKey.LStickUp => "<Gamepad>/leftStick/up",
                GamepadKey.LStickLeft => "<Gamepad>/leftStick/left",
                GamepadKey.LStickRight => "<Gamepad>/leftStick/right",
                GamepadKey.LStickPress => "<Gamepad>/leftStickPress",
                GamepadKey.RStick => "<Gamepad>/rightStick",
                GamepadKey.RStickDown => "<Gamepad>/rightStick/down",
                GamepadKey.RStickUp => "<Gamepad>/rightStick/up",
                GamepadKey.RStickLeft => "<Gamepad>/rightStick/left",
                GamepadKey.RStickRight => "<Gamepad>/rightStick/right",
                GamepadKey.RStickPress => "<Gamepad>/rightStickPress",
                GamepadKey.D_pad => "<Gamepad>/dpad",
                GamepadKey.D_padDown => "<Gamepad>/dpad/down",
                GamepadKey.D_padUp => "<Gamepad>/dpad/up",
                GamepadKey.D_padLeft => "<Gamepad>/dpad/left",
                GamepadKey.D_padRight => "<Gamepad>/dpad/right",
                GamepadKey.Select => "<Gamepad>/select",
                GamepadKey.Start => "<Gamepad>/start",
                _ => throw new System.NotImplementedException(),
            };
        }
    }
}