using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TM.Input.KeyConfig
{
    public class KeyConfigWarningManager : MonoBehaviour
    {
        [SerializeField] Canvas _canvas;
        [SerializeField] TextUIData _textUIData;
        [SerializeField] KeyConfigPopupManager _popupManager;
       
        public void SetEnable(bool enable)
        {
            _canvas.enabled = enable;
        }

        public void SetText(List<KeyData.GameAction> gameActions)
        {
            string action = "";
            foreach (var gameAction in gameActions)
            {
                action += _popupManager.ActionString((int)gameAction) + ", ";
            }
            _textUIData.TextData.SetText("�{�^���Ɋ��蓖�Ă��Ă��Ȃ��A�N�V����������܂��B�Œ�ł�1�̃{�^���Ɋ��蓖�ĂĂ��������B\n" + action);
        }

        public bool IsWarning => _canvas.enabled;
    }
}