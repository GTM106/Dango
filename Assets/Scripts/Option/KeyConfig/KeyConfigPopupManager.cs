using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TM.Input.KeyConfig
{
    public class KeyConfigPopupManager : MonoBehaviour
    {
        [SerializeField] TextUIData[] texts;
        [SerializeField] TextUIData selectName = default!;
        [SerializeField] KeyConfigManager keyConfigManager = default!;
        [SerializeField] Canvas _canvas = default!;

        //Flags��������
        KeyConfigData.GameAction _action;

        //�����Ȃ�
        List<KeyData.GameAction> _actionDatas = new();
        KeyData.GameAction _currentAction;
        int _currentActionIndex;

        public void OnNavigateVertical(InputAction.CallbackContext context)
        {
            if (!IsPopup) return;
            if (context.phase != InputActionPhase.Performed) return;

            Vector2 axis = context.ReadValue<Vector2>();

            if (axis == Vector2.up)
            {
                _currentActionIndex = (_currentActionIndex + 1) % _actionDatas.Count;
                _currentAction = _actionDatas[_currentActionIndex];
            }
            if (axis == Vector2.down)
            {
                if (--_currentActionIndex < 0) _currentActionIndex = _actionDatas.Count - 1;
                _currentAction = _actionDatas[_currentActionIndex];
            }
            Logger.Log(_currentAction);
        }

        public void OnChoiced(InputAction.CallbackContext context)
        {
            if (!IsPopup) return;
            if (context.phase != InputActionPhase.Performed) return;

            keyConfigManager.Rebinding((int)_currentAction);
        }

        public bool IsPopup => _canvas.enabled;

        private void ResetTexts()
        {
            foreach (var t in texts)
            {
                t.TextData.SetText();
            }
            selectName.TextData.SetText();
            _actionDatas.Clear();
        }

        private void SetTexts()
        {
            int num = 0;

            for (int i = 0; i < (int)KeyConfigData.GameAction.Max; i++)
            {
                if (!_action.HasFlag((KeyConfigData.GameAction)(1 << i))) continue;

                _actionDatas.Add((KeyData.GameAction)i);
                texts[num].TextData.SetText(ActionString(i));
                num++;
            }

            _currentAction = _actionDatas[0];
            selectName.TextData.SetText(keyConfigManager.Data.name);
        }

        public void OnCanvasEnabled()
        {
            _canvas.enabled = true;
            _action = keyConfigManager.Data.ConfigSelection;
            ResetTexts();
            SetTexts();
        }

        public void OnCanvasDisabled()
        {
            _canvas.enabled = false;
        }

        private string ActionString(int num)
        {
            //�}�W�b�N�i���o�[�ł��܂�悭�Ȃ������ł��B���̂����C�����ׂ����Ǝv���܂��B

            return num switch
            {
                0 => "�ړ�",
                1 => "��]",
                2 => "�W�����v",
                3 => "�˂��h��",
                4 => "�H�ׂ�",
                5 => "���O��",
                6 => "�|�[�Y",
                7 => "UI�g��",
                //(int)KeyConfigData.GameAction.Move => DataManager.LanguageData.keyConfigMove,
                //(int)KeyConfigData.GameAction.LookRotation => DataManager.LanguageData.keyConfigLookRotation,
                //(int)KeyConfigData.GameAction.Jump => DataManager.LanguageData.keyConfigJump,
                //(int)KeyConfigData.GameAction.Attack => DataManager.LanguageData.keyConfigAttack,
                //(int)KeyConfigData.GameAction.Eat => DataManager.LanguageData.keyConfigEat,
                //(int)KeyConfigData.GameAction.Remove => DataManager.LanguageData.keyConfigRemove,
                //(int)KeyConfigData.GameAction.Pause => DataManager.LanguageData.keyConfigPause,
                //(int)KeyConfigData.GameAction.UIExpansion => DataManager.LanguageData.keyConfigUIExpansion,
                _ => throw new System.NotImplementedException(),
            };
        }
    }
}