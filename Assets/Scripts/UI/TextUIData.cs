using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using TM.UI.Text;

/// <summary>
/// �e�L�X�g�̊�{�N���X�B�ݒ�̍ۂ�TextData���Q�ƁB
/// </summary>
public class TextUIData : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textUGUI = default!;

    TextData _textData;
    public TextData TextData
    {
        get
        {
            if (_textData == null) _textData = new(textUGUI);
            return _textData;
        }
        private set => _textData = value;
    }

    private void Awake()
    {
        if (_textData == null)
            _textData = new(textUGUI);
    }
}