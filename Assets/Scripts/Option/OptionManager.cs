using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TM.Input.KeyConfig;
using Unity.VisualScripting;

public class OptionManager : MonoBehaviour
{
    /// <summary>�I�v�V������ʂ̑I����</summary>
    enum OptionChoices
    {
        Option,

        TEMP,
        TEMP2,
        TEMP3,
        KeyConfig,

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

    /// <summary>
    /// �ÓI�Ɏ擾�o����I�v�V�����L�����o�X
    /// </summary>
    /// �P��V�[���Ŏ������邽�߁A�e��ʊԂ̂��Ƃ�̍ۂɎg�p����ÓI�Ȃ��̂ł��B
    public static Canvas OptionCanvas { get; private set; }

    //���݂̃I�v�V�����n�_
    OptionChoices _currentChoice = OptionChoices.Option;

    //���Ɉȍ~����I�v�V�����n�_
    OptionChoices _nextChoice = OptionChoices.TEMP;

    //�c������
    static readonly OptionDirection direction = OptionDirection.Vertical;
    static readonly Vector2[,] directionTable = { { Vector2.up, Vector2.down }, { Vector2.left, Vector2.right } };

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
            case OptionChoices.TEMP:
                break;
            case OptionChoices.TEMP2:
                break;
            case OptionChoices.TEMP3:
                break;
            case OptionChoices.KeyConfig:
                break;
            case OptionChoices.Max:
                break;
        }
    }
    #endregion

    private void Awake()
    {
        OptionCanvas = _canvas;

       // PlayerData.MyPlayerInput.
    }

    private void Start()
    {
        InputSystemManager.Instance.onBackPerformed += OnBack;
        InputSystemManager.Instance.onNavigatePerformed += OnNavigate;
        InputSystemManager.Instance.onChoicePerformed += Choiced;
    }

    private void ChangeChoice(Vector2 axis)
    {
        //�c�̃V�X�e���ɂ���H�H
        if (axis == directionTable[(int)direction, 0])
        {
            _nextChoice--;
            if (_nextChoice == OptionChoices.Option) _nextChoice = OptionChoices.Max - 1;
            Logger.Log(_nextChoice);

        }
        if (axis == directionTable[(int)direction, 1])
        {
            _nextChoice++;
            if (_nextChoice == OptionChoices.Max) _nextChoice = OptionChoices.Option + 1;

            Logger.Log(_nextChoice);
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
            case OptionChoices.TEMP:
                break;
            case OptionChoices.TEMP2:
                break;
            case OptionChoices.TEMP3:
                break;
            case OptionChoices.KeyConfig:
                _keyConfig.OnSelect();
                break;
        }
    }

    private void EnterNextChoice()
    {
        switch (_nextChoice)
        {
            case OptionChoices.TEMP:
                break;
            case OptionChoices.TEMP2:
                break;
            case OptionChoices.TEMP3:
                break;
            case OptionChoices.KeyConfig:
                EnterKeyConfig();
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
}