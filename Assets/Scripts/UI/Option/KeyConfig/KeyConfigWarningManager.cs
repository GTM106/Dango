using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TM.Input.KeyConfig
{
    public class KeyConfigWarningManager : MonoBehaviour
    {
        [SerializeField] Canvas _canvas = default!;
        [SerializeField] TextUIData _textUIData = default!;

        public void SetEnable(bool enable)
        {
            _canvas.enabled = enable;
        }

        public void SetText(List<KeyData.GameAction> gameActions)
        {
            string action = "";
            foreach (var gameAction in gameActions)
            {
                action += gameAction.ToString();
            }
            _textUIData.TextData.SetText("�{�^�������蓖�Ă��Ă��Ȃ��@�\������܂��B�Œ�ł�1�̃{�^���Ɋ��蓖�ĂĂ��������B\n" + action);
        }

        public bool IsWarming => _canvas.enabled;
    }
}