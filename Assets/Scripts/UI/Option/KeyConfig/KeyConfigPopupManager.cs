using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TM.Input.KeyConfig
{
    public class KeyConfigPopupManager : MonoBehaviour
    {
        [SerializeField] ImageUIData[] images;
        [SerializeField] TextUIData[] texts;
        [SerializeField] ImageUIData selectImage = default!;
        [SerializeField] KeyConfigManager keyConfigManager = default!;
        [SerializeField] Canvas _canvas = default!;
        [SerializeField] RectTransform popup = default!;

        //Flags��������
        KeyConfigData.GameAction _action;

        //�����Ȃ�
        List<KeyData.GameAction> _actionDatas = new();
        KeyData.GameAction _currentAction;
        int _currentActionIndex;
        int _settingActionIndex;

        const float OFFSET = 300f;

        public void OnNavigate()
        {
            if (!IsPopup) return;

            Vector2 axis = InputSystemManager.Instance.NavigateAxis;

            if (!ChangeChoiceUtil.Choice(axis, ref _currentActionIndex, _actionDatas.Count, false, ChangeChoiceUtil.OptionDirection.Vertical)) return;

            ChangeCurrentAction();
        }

        public void OnChoiced()
        {
            if (!IsPopup) return;

            keyConfigManager.Rebinding(_currentAction);
            _settingActionIndex = _currentActionIndex;
            SetImagesColor();
        }

        public bool IsPopup => _canvas.enabled;

        private void InitCurrentActionIndex()
        {
            for (int i = 0; i < _actionDatas.Count; i++)
            {
                if (_actionDatas[i] == keyConfigManager.Data.KeyData.Action)
                {
                    _currentActionIndex = i;
                    _settingActionIndex = i;
                    return;
                }
            }

            _currentActionIndex = 0;
        }

        private void ChangeCurrentAction()
        {
            _currentAction = _actionDatas[_currentActionIndex];
            SetImagesColor();
        }

        private void SetImagesColor()
        {
            foreach (var image in images)
            {
                image.ImageData.SetColor(Color.gray);
            }

            images[_settingActionIndex].ImageData.SetColor(Color.cyan);
            images[_currentActionIndex].ImageData.SetColor(Color.white);
        }

        private void ResetTexts()
        {
            foreach (var t in texts)
            {
                t.TextData.SetText();
            }
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

            selectImage.ImageData.SetSprite(keyConfigManager.Data.GetComponentInChildren<Image>().sprite);
        }

        public void OnCanvasEnabled()
        {
            InputSystemManager.Instance.onNavigatePerformed += OnNavigate;
            InputSystemManager.Instance.onChoicePerformed += OnChoiced;

            _action = keyConfigManager.Data.ConfigSelection;

            //�e�L�X�g�̕ύX
            ResetTexts();
            SetTexts();

            //�C���[�W�F�̕ύX
            InitCurrentActionIndex();
            ChangeCurrentAction();

            float x = keyConfigManager.Data.transform.localPosition.x > 0 ? -OFFSET : OFFSET;
            popup.localPosition = popup.localPosition.SetX(x);

            _canvas.enabled = true;
        }

        public void OnCanvasDisabled()
        {
            InputSystemManager.Instance.onNavigatePerformed -= OnNavigate;
            InputSystemManager.Instance.onChoicePerformed -= OnChoiced;

            _canvas.enabled = false;
        }

        public string ActionString(int num)
        {
            return num switch
            {
                (int)KeyData.GameAction.Unknown => "���ݒ�",
                (int)KeyData.GameAction.Move => "�ړ�",
                (int)KeyData.GameAction.LookRotation => "��]",
                (int)KeyData.GameAction.Jump => "�W�����v",
                (int)KeyData.GameAction.Attack => "�˂��h��",
                (int)KeyData.GameAction.Eat => "�H�ׂ�",
                (int)KeyData.GameAction.Remove => "���O��",
                (int)KeyData.GameAction.Pause => "�|�[�Y",
                (int)KeyData.GameAction.UIExpansion => "UI�g��",
                _ => throw new System.NotImplementedException(),
            };
        }
    }
}