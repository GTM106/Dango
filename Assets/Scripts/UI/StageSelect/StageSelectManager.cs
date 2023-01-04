using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TM.Easing.Management;
using UnityEngine;
using UnityEngine.UI;

public class StageSelectManager : MonoBehaviour
{
    Stage _currentStage;

    [SerializeField] FusumaManager _fusumaManager = default!;
    [SerializeField] StageData[] _stages;
    [SerializeField] Sprite[] _stageSprites;
    [SerializeField] Sprite[] _lockedStageSprites;
    [SerializeField] ImageUIData _stageImage = default!;
    [SerializeField] ImageUIData _stageImageUpdate = default!;
    [SerializeField] Image[] guids;
    [SerializeField] TextUIData _explanationText = default!;

    //�A�����b�N���o
    [SerializeField] Canvas _unlockCanvas;
    [SerializeField] ImageUIData _unlockImage;
    [SerializeField] ImageUIData _stageUnlockImage;
    [SerializeField] ImageUIData _padlockImage;

    List<int> _unlockList = new();

    bool _isChange;
    bool _isUnlockDirection;

    const float IMAGE_FADEIN_TIME = 0.2f;
    const float DIRECTION_TIME = 1f;

    private void Awake()
    {
        _unlockCanvas.enabled = false;
        _stageUnlockImage.ImageData.SetAlpha(0);
    }

    private async void Start()
    {
        AssignCurrentStage();
        CheckUnlockDirection();
        UpdateExplanationText();

        await _fusumaManager.UniTaskOpen();

        InputSystemManager.Instance.onNavigatePerformed += OnChangeStage;
        InputSystemManager.Instance.onChoicePerformed += OnChoiced;
        InputSystemManager.Instance.onBackPerformed += OnBack;
        InputSystemManager.Instance.onAnyKeyPerformed += OnAnyKeyPerformed;
    }

    private void OnChangeStage()
    {
        if (_unlockCanvas.enabled) return;
        if (_isUnlockDirection) return;

        //�X�V���͏������Ȃ��i�����̏����̓v���C�I�ɂ��܂�ǂ��Ȃ��C�����܂��B�j
        if (_isChange) return;

        if (!ChangeChoiceUtil.Choice(InputSystemManager.Instance.NavigateAxis, ref _currentStage, Stage.Tutorial, false, ChangeChoiceUtil.OptionDirection.Horizontal)) return;

        SoundManager.Instance.PlaySE(SoundSource.SE16_UI_SELECTION);
        UpdateSprite(InputSystemManager.Instance.NavigateAxis.x > 0);
        UpdateGuids();
        UpdateExplanationText();
    }

    private async void OnChoiced()
    {
        if (_unlockCanvas.enabled) return;
        if (_isUnlockDirection) return;

        if (_isChange) return;
        if (!_stages[(int)_currentStage].IsRelease)
        {
            SoundManager.Instance.PlaySE(SoundSource.SE7_CANT_STAB_DANGO);
            return;
        }

        _isChange = true;

        SoundManager.Instance.PlaySE(SoundSource.SE17_UI_DECISION);

        DisposeInput();

        await _fusumaManager.UniTaskClose();

        SceneSystem.Instance.SetIngameScene(SceneSystem.Scenes.Stage1 + (int)_currentStage);
        SceneSystem.Instance.Load(SceneSystem.Scenes.Stage1 + (int)_currentStage);
        UnLoad();
    }

    private async void OnBack()
    {
        if (_unlockCanvas.enabled) return;
        if (_isUnlockDirection) return;

        //���͎�t���I��
        DisposeInput();

        await _fusumaManager.UniTaskClose();
        SceneSystem.Instance.Load(SceneSystem.Scenes.Menu);
        UnLoad();
    }

    private async void OnAnyKeyPerformed()
    {
        if (!_unlockCanvas.enabled) return;

        //�A�����b�N�p�̃e�L�X�g���I�t��
        _unlockCanvas.enabled = false;

        //�A�����b�N���o���̃t���O�𗧂Ă�
        _isUnlockDirection = true;

        //���o�I���܂őҋ@
        while (_unlockList.Count > 0)
        {
            await UnlockDirection(this.GetCancellationTokenOnDestroy());
        }

        //�A�����b�N���o�t���O��܂�
        _isUnlockDirection = false;
    }

    private async UniTask UnlockDirection(CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        int offset = _unlockList[0] - (int)_currentStage;

        float waitTime = offset == 0 ? 0 : 0.5f;

        //�X���C�h���o�҂�
        await WaitForUpdateSprite(offset);

       //�싞�����t�F�[�h�A�E�g
        _padlockImage.ImageData.SetImageEnabled(true);
        _padlockImage.ImageData.SetAlpha(1f);
        _padlockImage.ImageData.Fadeout(DIRECTION_TIME, waitTime).Forget();

        //�����ɐ���ȉ摜���t�F�[�h�C��
        _stageUnlockImage.ImageData.SetSprite(_stageSprites[(int)_currentStage]);
        _stageUnlockImage.ImageData.SetAlpha(0);
        await _stageUnlockImage.ImageData.Fadein(DIRECTION_TIME, waitTime);

        //�摜��ʏ�X���C�h�ɐݒ肵�A�A�����b�N�p������
        _stageImage.ImageData.SetSprite(_stageSprites[(int)_currentStage]);
        _stageImageUpdate.ImageData.SetSprite(_stageSprites[(int)_currentStage]);
        _stageUnlockImage.ImageData.SetAlpha(0);

        //�싞�������Ƃɖ߂�
        _padlockImage.ImageData.SetAlpha(1);
        _padlockImage.ImageData.SetImageEnabled(false);

        _unlockList.RemoveAt(0);

        //�Z�[�u�f�[�^�ɕۑ�
        DataManager.saveData.stagesStatus[(int)_currentStage] = (int)StageStatus.Unlock;
    }

    private async UniTask WaitForUpdateSprite(int offset)
    {
        if (offset == 0) return;

        //�X�e�[�W���A�����b�N���ꂽ�����ɕύX
        _currentStage = (Stage)_unlockList[0];

        //�Y���X�e�[�W�܂ŃX�N���[��
        UpdateSprite(offset > 0);
        UpdateGuids();
        UpdateExplanationText();

        //�X�N���[�����o��ҋ@
        while (!_isChange) await UniTask.Yield();
    }

    private bool CheckUnlockDirection()
    {
        for (int i = 0; i < DataManager.saveData.stagesStatus.Length; i++)
        {
            //�A�����b�N����Ă��邪�A�܂����o�����Ă��Ȃ����̂̂ݍs��
            if (DataManager.saveData.stagesStatus[i] == (int)StageStatus.StandbyForDirection)
            {
                //�A�����b�N���o��ON�ɂ���
                _unlockCanvas.enabled = true;

                _unlockList.Add(i);
            }
        }

        return _unlockCanvas.enabled;
    }

    private void AssignCurrentStage()
    {
        //�A�����b�N����Ă���ŐV�̃X�e�[�W��I��
        foreach (var stage in _stages)
        {
            if (stage.IsRelease) _currentStage = (Stage)Mathf.Max((int)_currentStage, (int)stage.Stage);
        }

        //�K�C�h�̕`��X�V
        UpdateGuids();

        //�X�e�[�W�摜���X�V
        SetSprite(_stageImage);
        SetSprite(_stageImageUpdate);
    }

    private void UpdateGuids()
    {
        //�Ƃ肠�����S��ON�ɂ���
        foreach (var guid in guids) guid.gameObject.SetActive(true);

        //���̌㍶�E�����邩�m�F���āAOff�ɂ���
        if (_currentStage == 0) guids[0].gameObject.SetActive(false);
        if ((int)_currentStage == _stages.Length - 1) guids[1].gameObject.SetActive(false);
    }

    private async void UpdateSprite(bool isLeft)
    {
        _isChange = true;

        SetSprite(_stageImage);

        float width = _stageImage.ImageData.GetWidth();
        float center = 0;

        //�X���C�h�C�����ۂ����ǃX���C�h�C������Ȃ��Ⴄ����
        await UniTask.WhenAll(//�ꉞ����őҋ@���邪�A�S�����^�C�~���O�ŏI������
         _stageImage.ImageData.MoveX(isLeft ? width : -width, isLeft ? -width : width, IMAGE_FADEIN_TIME),
         _stageImage.ImageData.WipeinHorizontal(IMAGE_FADEIN_TIME, isLeft ? Image.OriginHorizontal.Left : Image.OriginHorizontal.Right),
         _stageImageUpdate.ImageData.MoveX(center, isLeft ? -width : width, IMAGE_FADEIN_TIME),
         _stageImageUpdate.ImageData.WipeoutHorizontal(IMAGE_FADEIN_TIME, isLeft ? Image.OriginHorizontal.Right : Image.OriginHorizontal.Left));

        //�t�F�[�h���鑤��ݒ肵�A�t�F�[�h�C��������
        SetSprite(_stageImageUpdate);

        _isChange = false;
    }

    private async void UpdateExplanationText()
    {
        await _explanationText.TextData.Fadeout(IMAGE_FADEIN_TIME / 2f);

        string stageDescriptions = _currentStage switch
        {
            Stage.Stage1 => "�X�e�[�W1�@��Փx�F������\n���l�͓`���̒c�q�����߂�\n�h�s���K�ꂽ�c�c",
            Stage.Stage2 => "�X�e�[�W2�@��Փx�F������\n����c�q������������ŁA\n���l�͏��ڎw��",
            Stage.Stage3 => "�X�e�[�W3�@��Փx�F������\n�h�s��̍ŏ�K��ڎw���āA\n�Ō�̒c�����n�܂�",
            _ => throw new System.NotImplementedException(),
        };

        _explanationText.TextData.SetText(stageDescriptions);

        await _explanationText.TextData.Fadein(IMAGE_FADEIN_TIME / 2f);
    }

    private void SetSprite(ImageUIData data)
    {
        bool isRelease = _stages[(int)_currentStage].IsRelease;

        //�I�𒆂̃X�e�[�W���A�����b�N����Ă��邩�ɂ���ĉ摜��؂�ւ�
        Sprite[] sprites = isRelease ? _stageSprites : _lockedStageSprites;

        _padlockImage.ImageData.SetImageEnabled(!isRelease);

        data.ImageData.SetSprite(sprites[(int)_currentStage]);
    }

    private void UnLoad()
    {
        //���ɔ񓯊��A�j���[�V�������������ꍇ�j������
        _stageImageUpdate.ImageData.CancelUniTask();

        SceneSystem.Instance.UnLoad(SceneSystem.Scenes.StageSelect, true);
    }

    private void DisposeInput()
    {
        InputSystemManager.Instance.onNavigatePerformed -= OnChangeStage;
        InputSystemManager.Instance.onChoicePerformed -= OnChoiced;
        InputSystemManager.Instance.onBackPerformed -= OnBack;
        InputSystemManager.Instance.onAnyKeyPerformed -= OnAnyKeyPerformed;
    }
}