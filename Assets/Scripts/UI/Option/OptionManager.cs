using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TM.Input.KeyConfig;
using UnityEngine.UI;
using static ChangeChoiceUtil;

public class OptionManager : MonoBehaviour
{
    /// <summary>オプション画面の選択肢</summary>
    enum OptionChoices
    {
        KeyConfig,
        Operation,
        Sound,
        Other,

        Max,
    }

    #region メンバ
    [SerializeField] KeyConfigManager _keyConfig = default!;
    [SerializeField] OperationManager _operationManager = default!;
    [SerializeField] SoundSettingManager _soundSettingManager = default!;
    [SerializeField] OtherSettingsManager _otherSettingsManager = default!;

    [SerializeField] ImageUIData[] _optionImages;
    [SerializeField] ImageUIData[] _LRGuide;

    [SerializeField] FusumaManager _fusumaManager;

    //次に以降するオプション地点
    OptionChoices _currentChoice = OptionChoices.KeyConfig;

    //縦か横か
    static readonly OptionDirection direction = OptionDirection.Horizontal;

    //上端から下端に移動するか否か
    static readonly bool canMoveTopToBottom = false;

    #endregion

    private void Awake()
    {
        EnterNextChoice();
        SetOptionSelectColor();
        SetLRGuideColor();
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
        SetOptionSelectColor();
        SetLRGuideColor();
        EnterNextChoice();
    }

    private void EnterNextChoice()
    {
        _keyConfig.SetCanvasEnable(_currentChoice == OptionChoices.KeyConfig);
        _soundSettingManager.SetCanvasEnable(_currentChoice == OptionChoices.Sound);
        _operationManager.SetCanvasEnable(_currentChoice == OptionChoices.Operation);
        _otherSettingsManager.SetCanvasEnable(_currentChoice == OptionChoices.Other);
    }

    private void SetOptionSelectColor()
    {
        for (int i = 0; i < _optionImages.Length; i++)
        {
            Color color = (int)_currentChoice == i ? Color.white : Color.gray;

            _optionImages[i].ImageData.SetColor(color);
        }
    }

    private void SetLRGuideColor()
    {
        foreach (var image in _LRGuide)
        {
            image.ImageData.SetColor(Color.white);
        }

        if (_currentChoice == 0) _LRGuide[0].ImageData.SetColor(Color.gray);
        else if (_currentChoice == OptionChoices.Max - 1) _LRGuide[1].ImageData.SetColor(Color.gray);
    }
}