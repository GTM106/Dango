using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum TutorialStage
{
    BasicOperation,
    ApplicationOperation,
    Role_Ittou,
    Role_Zennten,
    Role_Rinne,
    Role_Rinshoku,
    Role_Sanmen,

    Max,
}

public class TutorialManager : MonoBehaviour
{
    [Header("U3�̉摜 ��U5���珇�Ԃɓ���Ă�������")]
    [SerializeField] List<Sprite> _U3Sprite;
    [Header("U4�̃e�L�X�g ��U5���珇�Ԃɓ��͂��Ă�������")]
    [SerializeField] List<string> _U4Texts;

    [Header("U5-11�̃Z���N�g���̉摜 ��U5���珇�Ԃɓ���Ă�������")]
    [SerializeField] List<Sprite> _U5_11SelectedSprite;
    [Header("U5-11�̔�Z���N�g���̉摜 ��U5���珇�Ԃɓ���Ă�������")]
    [SerializeField] List<Sprite> _U5_11UnselectedSprite;

    [Header("U12�̉摜")]
    [SerializeField] Sprite _U12CompleteSprite;
    [SerializeField] Sprite _U12IncompleteSprite;

    [Header("���ȉ��̓v���O���}�ȊO�G��Ȃ��ł���������")]
    [SerializeField] List<ImageUIData> _U12s;
    [SerializeField] List<ImageUIData> _U5_11;
    [SerializeField] TextMeshProUGUI _U4TextMesh;
    [SerializeField] FusumaManager _fusumaManager;

    TutorialStage _currentStage;

    private void Awake()
    {
        _currentStage = TutorialStage.BasicOperation;
        SetU12Sprite();
        ResetU5_11Sprite();
        _U5_11[(int)_currentStage].ImageData.SetSprite(_U5_11SelectedSprite[(int)_currentStage]);
        _U4TextMesh.text = _U4Texts[(int)_currentStage];
    }

    private async void OnEnable()
    {
        await _fusumaManager.UniTaskOpen(1.5f);
        InputSystemManager.Instance.onNavigatePerformed += OnNavigate;
        InputSystemManager.Instance.onBackPerformed += OnBack;
        InputSystemManager.Instance.onChoicePerformed += OnChoice;
    }

    private void OnNavigate()
    {
        //���O�̃X�e�[�W���L�^���Ă���
        int prebStage = (int)_currentStage;

        if (!ChangeChoiceUtil.Choice(InputSystemManager.Instance.NavigateAxis, ref _currentStage, TutorialStage.Max, true, ChangeChoiceUtil.OptionDirection.Vertical)) return;

        SoundManager.Instance.PlaySE(SoundSource.SE16_UI_SELECTION);

        //���O�̃X�e�[�W�����Z�b�g
        _U5_11[prebStage].ImageData.SetSprite(_U5_11UnselectedSprite[prebStage]);

        //���̌�Z���N�g���Ă�����̂�����
        _U5_11[(int)_currentStage].ImageData.SetSprite(_U5_11SelectedSprite[(int)_currentStage]);

        //�e�L�X�g��ύX
        _U4TextMesh.text = _U4Texts[(int)_currentStage];
    }

    private async void OnBack()
    {
        Dispose();
        await _fusumaManager.UniTaskClose(1.5f);

        SceneSystem.Instance.UnLoad(SceneSystem.Scenes.TutorialHub, true);
        SceneSystem.Instance.Load(SceneSystem.Scenes.Menu);
    }

    private async void OnChoice()
    {
        SoundManager.Instance.PlaySE(SoundSource.SE17_UI_DECISION);
        Dispose();

        await _fusumaManager.UniTaskClose(1.5f);

        SceneSystem.Scenes scene = SceneSystem.Scenes.Tutorial1 + (int)_currentStage;

        SceneSystem.Instance.UnLoad(SceneSystem.Scenes.TutorialHub, true);
        SceneSystem.Instance.Load(scene);
        SceneSystem.Instance.SetIngameScene(scene);
    }

    private void SetU12Sprite()
    {
        for (int i = 0; i < _U12s.Count; i++)
        {
            Sprite sprite = (DataManager.saveData.tutorialStatusBit & (1 << i)) != 0 ? _U12CompleteSprite : _U12IncompleteSprite;

            _U12s[i].ImageData.SetSprite(sprite);
        }
    }

    private void ResetU5_11Sprite()
    {
        for (int i = 0; i < (int)TutorialStage.Max; i++)
        {
            _U5_11[i].ImageData.SetSprite(_U5_11UnselectedSprite[i]);
        }
    }

    private void Dispose()
    {
        InputSystemManager.Instance.onNavigatePerformed -= OnNavigate;
        InputSystemManager.Instance.onBackPerformed -= OnBack;
        InputSystemManager.Instance.onChoicePerformed -= OnChoice;
    }
}
