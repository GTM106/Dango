using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TM.Input.KeyConfig;
using UnityEngine.UI;
using static ChangeChoiceUtil;

public class OptionManager : MonoBehaviour
{
    /// <summary>�I�v�V������ʂ̑I����</summary>
    enum OptionChoices
    {
        KeyConfig,
        Operation,
        Sound,
        Other,

        Max,
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

    [SerializeField] Image _methodOfOperation = default!;
    [SerializeField] Sprite[] _methodOfOperationSprites;

    /// <summary>
    /// �ÓI�Ɏ擾�o����I�v�V�����L�����o�X
    /// </summary>
    /// �P��V�[���Ŏ������邽�߁A�e��ʊԂ̂��Ƃ�̍ۂɎg�p����ÓI�Ȃ��̂ł��B
    public static Canvas OptionCanvas { get; private set; }

    //���Ɉȍ~����I�v�V�����n�_
    OptionChoices _currentChoice = OptionChoices.KeyConfig;

    //�c������
    static readonly OptionDirection direction = OptionDirection.Horizontal;

    //��[���牺�[�Ɉړ����邩�ۂ�
    static readonly bool canMoveTopToBottom = false;

    #endregion

    private void Awake()
    {
        OptionCanvas = _canvas;

        EnterNextChoice();
        SetFontSize();
    }

    private async void Start()
    {
        await _fusumaManager.UniTaskOpen();
        OptionInputEnable();
    }

    private void OptionInputEnable()
    {
        InputSystemManager.Instance.onBackPerformed += OnBack;
        InputSystemManager.Instance.onTabControlPerformed += ChangeChoice;
        _keyConfig.AfterFusumaOpen();
        _operationManager.AfterFusumaOpen();
        _otherSettingsManager.AfterFusumaOpen();
        _soundSettingManager.AfterFusumaOpen();
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
        if (_otherSettingsManager.IsPopUp) return;
        if (_keyConfig.IsPopup) return;
        if (!_keyConfig.CheckHasKeyAllActions()) return;

        OptionInputDisable();
        await _fusumaManager.UniTaskClose(1.5f);
        SceneSystem.Instance.Load(SceneSystem.Instance.PrebScene);
        SceneSystem.Instance.UnLoad(SceneSystem.Scenes.Option, true);
    }

    private void ChangeChoice()
    {
        if (_otherSettingsManager.IsPopUp) return;
        if (_keyConfig.IsPopup) return;
        if (!_keyConfig.CheckHasKeyAllActions()) return;

        Vector2 axis = InputSystemManager.Instance.TabControlAxis;

        if (!Choice(axis, ref _currentChoice, OptionChoices.Max, canMoveTopToBottom, direction)) return;

        SoundManager.Instance.PlaySE(SoundSource.SE16_UI_SELECTION);
        SetFontSize();
        EnterNextChoice();
    }

    private void EnterNextChoice()
    {
        _keyConfig.SetCanvasEnable(_currentChoice == OptionChoices.KeyConfig);
        _soundSettingManager.SetCanvasEnable(_currentChoice == OptionChoices.Sound);
        _operationManager.SetCanvasEnable(_currentChoice == OptionChoices.Operation);
        _otherSettingsManager.SetCanvasEnable(_currentChoice == OptionChoices.Other);

        ChangeMethodOfOperation();
    }

    private void ChangeMethodOfOperation()
    {
        //�E���̕\���iU65�j�̏����ݒ�
        if (_methodOfOperationSprites.Length != 3) return;
        _methodOfOperation.sprite = _currentChoice switch
        {
            OptionChoices.KeyConfig => _methodOfOperationSprites[0],
            OptionChoices.Operation => _methodOfOperationSprites[1],
            OptionChoices.Sound => _methodOfOperationSprites[2],
            OptionChoices.Other => _methodOfOperationSprites[0],
            _ => throw new System.NotImplementedException(),
        };
    }

    private void SetFontSize()
    {
        for (int i = 0; i < _optionTexts.Length; i++)
        {
            float size = (int)_currentChoice == i ? 65f : 55.5f;

            _optionTexts[i].TextData.SetFontSize(size);
        }
    }
}