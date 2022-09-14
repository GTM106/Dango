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

        KeyConfig,
        Operation,
        Sound,
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
    [SerializeField] OperationManager _operationManager = default!;
    [SerializeField] SoundSettingManager _soundSettingManager = default!;
    [SerializeField] OtherSettingsManager _otherSettingsManager = default!;

    //���ݖ��g�p
    [SerializeField] Canvas _canvas = default!;

    [SerializeField] TextUIData[] _optionTexts;
    [SerializeField] FusumaManager _fusumaManager;

    /// <summary>
    /// �ÓI�Ɏ擾�o����I�v�V�����L�����o�X
    /// </summary>
    /// �P��V�[���Ŏ������邽�߁A�e��ʊԂ̂��Ƃ�̍ۂɎg�p����ÓI�Ȃ��̂ł��B
    public static Canvas OptionCanvas { get; private set; }

    //���Ɉȍ~����I�v�V�����n�_
    OptionChoices _currentChoice = OptionChoices.KeyConfig;

    //�c������
    static readonly OptionDirection direction = OptionDirection.Horizontal;
    static readonly Vector2[,] directionTable = { { Vector2.up, Vector2.down }, { Vector2.left, Vector2.right } };

    //��[���牺�[�Ɉړ����邩�ۂ�
    static readonly bool canMoveTopToBottom = false;

    #endregion

    private void Awake()
    {
        OptionCanvas = _canvas;

        EnterNextChoice();
        SetFontSize();
    }

    private void Start()
    {
        OptionInputEnable();
        _fusumaManager.Open();
    }

    private void OptionInputEnable()
    {
        InputSystemManager.Instance.onBackPerformed += OnBack;
        InputSystemManager.Instance.onTabControlPerformed += ChangeChoice;
    }

    private void OptionInputDisable()
    {
        InputSystemManager.Instance.onBackPerformed -= OnBack;
        InputSystemManager.Instance.onTabControlPerformed -= ChangeChoice;
        _keyConfig.OnChangeScene();
        _operationManager.OnChangeScene();
        _soundSettingManager.OnChangeScene();
        _otherSettingsManager.OnChangeScene();
    }

    private async void OnBack()
    {
        if (_keyConfig.IsPopup) return;
        if (!_keyConfig.CheckHasKeyAllActions()) return;

        OptionInputDisable();
        await _fusumaManager.UniTaskClose(1.5f);
        SceneSystem.Instance.Load(SceneSystem.Scenes.Menu);
        SceneSystem.Instance.UnLoad(SceneSystem.Scenes.Option);
    }

    private void ChangeChoice()
    {
        if (_keyConfig.IsPopup) return;
        if (!_keyConfig.CheckHasKeyAllActions()) return;

        Vector2 axis = InputSystemManager.Instance.TabControlAxis;

        //Up or Left
        if (axis == directionTable[(int)direction, 0])
        {
            _currentChoice--;

            if (_currentChoice == OptionChoices.Option) _currentChoice = canMoveTopToBottom ? OptionChoices.Max - 1 : OptionChoices.Option + 1;

            SetFontSize();
        }
        //Down or Right
        else if (axis == directionTable[(int)direction, 1])
        {
            _currentChoice++;

            if (_currentChoice == OptionChoices.Max) _currentChoice = canMoveTopToBottom ? OptionChoices.Option + 1 : OptionChoices.Max - 1;

            SetFontSize();
        }

        EnterNextChoice();
    }

    private void EnterNextChoice()
    {
        _keyConfig.SetCanvasEnable(_currentChoice == OptionChoices.KeyConfig);
        _soundSettingManager.SetCanvasEnable(_currentChoice == OptionChoices.Sound);
        _operationManager.SetCanvasEnable(_currentChoice == OptionChoices.Operation);
        _otherSettingsManager.SetCanvasEnable(_currentChoice == OptionChoices.Other);
    }

    private void SetFontSize()
    {
        for (int i = 0; i < _optionTexts.Length; i++)
        {
            float size = (int)_currentChoice - 1 == i ? 65f : 55.5f;

            _optionTexts[i].TextData.SetFontSize(size);
        }
    }
}