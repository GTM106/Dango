using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
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

    bool _isChange;

    const float IMAGE_FADEIN_TIME = 0.2f;

    private void Start()
    {
        _fusumaManager.Open();
        InputSystemManager.Instance.onNavigatePerformed += OnChangeStage;
        InputSystemManager.Instance.onChoicePerformed += OnChoiced;
        InputSystemManager.Instance.onBackPerformed += OnBack;
        AssignCurrentStage();
    }

    private void OnChangeStage()
    {
        //�X�V���͏������Ȃ��i�����̏����̓v���C�I�ɂ��܂�ǂ��Ȃ��C�����܂��B�j
        if (_isChange) return;

        if (InputSystemManager.Instance.NavigateAxis == Vector2.right)
        {
            //�E�[�Ȃ珈�����Ȃ�
            if ((int)_currentStage >= _stages.Length - 1) return;

            _currentStage++;
            UpdateSprite(true);
        }
        else if (InputSystemManager.Instance.NavigateAxis == Vector2.left)
        {
            //���[�Ȃ珈�����Ȃ�
            if (_currentStage <= 0) return;

            _currentStage--;
            UpdateSprite(false);
        }
        //���E�ȊO�̓��͔͂������Ȃ�
        else return;

        SoundManager.Instance.PlaySE(SoundSource.SE16_UI_SELECTION);
        UpdateGuids();
        UpdateExplanationText();
        Logger.Log(_currentStage);
    }

    private async void OnChoiced()
    {
        if (!_stages[(int)_currentStage].IsRelease)
        {
            SoundManager.Instance.PlaySE(SoundSource.SE7_CANT_STAB_DANGO);
            return;
        }

        SoundManager.Instance.PlaySE(SoundSource.SE17_UI_DECISION);
        await _fusumaManager.UniTaskClose();
        SceneSystem.Instance.SetIngameScene(SceneSystem.Scenes.Stage1 + (int)_currentStage);
        SceneSystem.Instance.Load(SceneSystem.Scenes.Stage1 + (int)_currentStage);
        UnLoad();
    }

    private async void OnBack()
    {
        await _fusumaManager.UniTaskClose();
        SceneSystem.Instance.Load(SceneSystem.Scenes.Menu);
        UnLoad();
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
        _explanationText.TextData.SetText("������ ������");
        await _explanationText.TextData.Fadein(IMAGE_FADEIN_TIME / 2f);
    }

    private void SetSprite(ImageUIData data)
    {
        //�I�𒆂̃X�e�[�W���A�����b�N����Ă��邩�ɂ���ĉ摜��؂�ւ�
        Sprite[] sprites = _stages[(int)_currentStage].IsRelease ? _stageSprites : _lockedStageSprites;

        data.ImageData.SetSprite(sprites[(int)_currentStage]);
    }

    private void UnLoad()
    {
        InputSystemManager.Instance.onNavigatePerformed -= OnChangeStage;
        InputSystemManager.Instance.onChoicePerformed -= OnChoiced;
        InputSystemManager.Instance.onBackPerformed -= OnBack;

        //���ɔ񓯊��A�j���[�V�������������ꍇ�j������
        _stageImageUpdate.ImageData.CancelUniTask();

        SceneSystem.Instance.UnLoad(SceneSystem.Scenes.StageSelect, true);
    }
}