using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TM.Input.KeyConfig;

public class OptionManager : MonoBehaviour
{
    /// <summary>�I�v�V������ʂ̑I����</summary>
    enum OptionChoices
    {
        Option,

        Camera,
        Screen,
        Sound,
        KeyConfig,
        Other,

        Max,
    }

    enum OptionDirection
    {
        Vertical,
        Horizontal,
    }

    #region �����o
    [SerializeField] KeyConfigManager _keyConfig = default!;
    [SerializeField] Canvas _canvas = default!;
    [SerializeField] TextUIData[] _optionTexts;

    /// <summary>
    /// �ÓI�Ɏ擾�o����I�v�V�����L�����o�X
    /// </summary>
    /// �P��V�[���Ŏ������邽�߁A�e��ʊԂ̂��Ƃ�̍ۂɎg�p����ÓI�Ȃ��̂ł��B
    public static Canvas OptionCanvas { get; private set; }

    //���݂̃I�v�V�����n�_
    OptionChoices _currentChoice = OptionChoices.Option;

    //���Ɉȍ~����I�v�V�����n�_
    OptionChoices _nextChoice = OptionChoices.Camera;

    //�c������
    static readonly OptionDirection direction = OptionDirection.Vertical;
    static readonly Vector2[,] directionTable = { { Vector2.up, Vector2.down }, { Vector2.left, Vector2.right } };

    //��[���牺�[�Ɉړ����邩�ۂ�
    static readonly bool canMoveTopToBottom = false;

    #endregion

    #region InputSystem
    private void OnBack()
    {
        if (_currentChoice == OptionChoices.Option)
        {
            //Player(�ʏ�v���C�̓���)�}�b�v�ɖ߂�
            InputSystemManager.Instance.Input.SwitchCurrentActionMap("Player");

            //B�{�^��������u�Ŗ߂��ăW�����v���Ă��܂��o�O����B�����s��

            _canvas.enabled = false;
        }
        else if (_currentChoice == OptionChoices.KeyConfig)
        {
            //�|�b�v�A�b�v���Ȃ�ʏ���������
            if (!_keyConfig.OnBack()) return;

            _currentChoice = OptionChoices.Option;
            EnterOption();
        }
        else
        {
            _currentChoice = OptionChoices.Option;
            EnterOption();
        }
    }

    private void OnNavigate()
    {
        Vector2 axis = InputSystemManager.Instance.NavigateAxis;

        switch (_currentChoice)
        {
            case OptionChoices.Option:
                ChangeChoice(axis);
                break;
            case OptionChoices.Camera:
                break;
            case OptionChoices.Screen:
                break;
            case OptionChoices.KeyConfig:
                break;
            case OptionChoices.Other:
                break;
        }
    }
    #endregion

    private void Awake()
    {
        OptionCanvas = _canvas;

        EnterOption();
        SetFontSize();
    }

    private void Start()
    {
        InputSystemManager.Instance.onBackPerformed += OnBack;
        InputSystemManager.Instance.onNavigatePerformed += OnNavigate;
        InputSystemManager.Instance.onChoicePerformed += Choiced;
    }

    private void ChangeChoice(Vector2 axis)
    {
        //Up or Left
        if (axis == directionTable[(int)direction, 0])
        {
            _nextChoice--;

            if (_nextChoice == OptionChoices.Option) _nextChoice = canMoveTopToBottom ? OptionChoices.Max - 1 : OptionChoices.Option + 1;

            SetFontSize();
        }
        //Down or Right
        else if (axis == directionTable[(int)direction, 1])
        {
            _nextChoice++;

            if (_nextChoice == OptionChoices.Max) _nextChoice = canMoveTopToBottom ? OptionChoices.Option + 1 : OptionChoices.Max - 1;

            SetFontSize();
        }

    }

    private void Choiced()
    {
        switch (_currentChoice)
        {
            case OptionChoices.Option:
                Logger.Log("�I��������");
                EnterNextChoice();
                break;

            case OptionChoices.Camera:
                break;
            case OptionChoices.Screen:
                break;
            case OptionChoices.KeyConfig:
                _keyConfig.OnSelect();
                break;
            case OptionChoices.Other:
                break;
        }
    }

    private void EnterNextChoice()
    {
        switch (_nextChoice)
        {
            case OptionChoices.Camera:
                break;
            case OptionChoices.Screen:
                break;
            case OptionChoices.KeyConfig:
                EnterKeyConfig();
                break;
            case OptionChoices.Other:
                break;
        }
    }

    private void EnterOption()
    {
        _keyConfig.SetCanvasEnable(false);
    }

    private void EnterKeyConfig()
    {
        _currentChoice = OptionChoices.KeyConfig;
        _keyConfig.SetCanvasEnable(true);
    }

    private void SetFontSize()
    {
        for (int i = 0; i < _optionTexts.Length; i++)
        {
            float size = (int)_nextChoice - 1 == i ? 100f : 80f;

            _optionTexts[i].TextData.SetFontSize(size);
        }
    }
}