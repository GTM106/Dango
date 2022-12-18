using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using TM.Easing.Management;
using UnityEngine;

public class QuestSucceedUIManager : MonoBehaviour
{
    [SerializeField] ImageUIData _succeedTextImage;
    [SerializeField] Transform _nextQuestTransform;
    [SerializeField] List<ImageUIData> _nextQuestImageDatas;
    [SerializeField] List<TextUIData> _nextQuestTextDatas;

    [Header("���c���B���̃��x���f�U�C���͂����火")]
    [SerializeField, Tooltip("�X���C�h�C������"), Min(0)] float _succeedImageSlideinTime;
    [SerializeField, Tooltip("�X���C�h�A�E�g����"), Min(0)] float _succeedImageSlideoutTime;
    [SerializeField, Tooltip("�����؍ݎ���"), Min(0)] float _succeedImageDisplayTime;
    [SerializeField, Tooltip("�����؍ݒ��̈ړ�����"), Min(0)] float _succeedImageDisplayAmountOfMovement;

    [Header("�����̃N�G�X�g�̃��x���f�U�C���͂����火")]
    [SerializeField, Tooltip("�X���C�h�C������"), Min(0)] float _nextQuestSlideinTime;
    [SerializeField, Tooltip("�X���C�h�A�E�g����"), Min(0)] float _nextQuestSlideoutTime;
    [SerializeField, Tooltip("�����؍ݎ���"), Min(0)] float _nextQuestDisplayTime;
    [SerializeField, Tooltip("�����؍ݒ��̈ړ�����"), Min(0)] float _nextQuestDisplayAmountOfMovement;

    const float OFFSET = 1400f;

    List<string> _nextQuestTexts = new();

    private void Awake()
    {
        _succeedTextImage.ImageData.SetPositionX(-OFFSET);
        _nextQuestTransform.position = _nextQuestTransform.position.SetX(-OFFSET);

#if UNITY_EDITOR
        //���x���f�U�C��������͖��ʂȔ���ɂȂ邽�߃r���h�ɂ͒ʂ��܂���B
        //����āA���̗�O���d��Ȃ��̂Ƒ����G���[�Ƃ��܂��B
        if (_succeedImageDisplayTime == 0)
        {
            Logger.Error("���z��O�̒l�����͂���Ă��܂���\ndisplayTime��0�̏ꍇ��displayAmountOfMovement��0����������܂��B\ndisplayAmountOfMovement�������I��0�ɂ��܂��B");
            _succeedImageDisplayAmountOfMovement = 0;
        }
        if (_nextQuestDisplayTime == 0)
        {
            Logger.Error("���z��O�̒l�����͂���Ă��܂���\ndisplayTime��0�̏ꍇ��displayAmountOfMovement��0����������܂��B\ndisplayAmountOfMovement�������I��0�ɂ��܂��B");
            _nextQuestDisplayAmountOfMovement = 0;
        }
#endif
    }

    public async void PlayAnimation(params QuestData[] questDatas)
    {
        float currentTime = 0;
        float offset = OFFSET - (_succeedImageDisplayAmountOfMovement / 2f);

        _nextQuestTexts.Clear();

        foreach (QuestData questData in questDatas)
        {
            _nextQuestTexts.Add(questData.QuestName);
        }

        try
        {
            _succeedTextImage.ImageData.SetPositionX(-OFFSET);

            //�X���C�h�C��
            while (currentTime < _succeedImageSlideinTime)
            {
                await UniTask.Yield();
                currentTime += Time.deltaTime;
                currentTime = Mathf.Min(_succeedImageSlideinTime, currentTime);

                _succeedTextImage.ImageData.SetPositionX(-OFFSET + offset * EasingManager.EaseProgress(TM.Easing.EaseType.OutCubic, currentTime, _succeedImageSlideinTime, 0, 0));
            }

            _succeedTextImage.ImageData.SetPositionX(-_succeedImageDisplayAmountOfMovement / 2f);
            currentTime = 0;

            //Slidein��ɍs��
            PlayNextQuestAnimation();

            //�����\��
            while (currentTime < _succeedImageDisplayTime)
            {
                await UniTask.Yield();
                currentTime += Time.deltaTime;
                currentTime = Mathf.Min(_succeedImageDisplayTime, currentTime);

                _succeedTextImage.ImageData.SetPositionX(-_succeedImageDisplayAmountOfMovement / 2f + _succeedImageDisplayAmountOfMovement * EasingManager.EaseProgress(TM.Easing.EaseType.Linear, currentTime, _succeedImageDisplayTime, 0, 0));
            }

            _succeedTextImage.ImageData.SetPositionX(_succeedImageDisplayAmountOfMovement / 2f);
            currentTime = 0;

            //�X���C�h�A�E�g
            while (currentTime < _succeedImageSlideoutTime)
            {
                await UniTask.Yield();
                currentTime += Time.deltaTime;
                currentTime = Mathf.Min(_succeedImageSlideoutTime, currentTime);

                _succeedTextImage.ImageData.SetPositionX((_succeedImageDisplayAmountOfMovement / 2f) + (offset * EasingManager.EaseProgress(TM.Easing.EaseType.InCubic, currentTime, _succeedImageSlideoutTime, 0, 0)));
            }

            _succeedTextImage.ImageData.SetPositionX(OFFSET);
        }
        catch (MissingReferenceException)
        {
            return;
        }
    }

    private async void PlayNextQuestAnimation()
    {
        //���̃N�G�X�g���Ȃ��i�Ō�Ȃ�e���j
        if (_nextQuestTexts.Count == 0) return;

        SetText();

        float currentTime = 0;
        float offset = OFFSET - (_nextQuestDisplayAmountOfMovement / 2f);

        try
        {
            _nextQuestTransform.localPosition = _nextQuestTransform.localPosition.SetX(-OFFSET);

            //�X���C�h�C��
            while (currentTime < _nextQuestSlideinTime)
            {
                await UniTask.Yield();
                currentTime += Time.deltaTime;
                currentTime = Mathf.Min(_nextQuestSlideinTime, currentTime);

                _nextQuestTransform.localPosition = _nextQuestTransform.localPosition.SetX(-OFFSET + offset * EasingManager.EaseProgress(TM.Easing.EaseType.OutCubic, currentTime, _nextQuestSlideinTime, 0, 0));
            }

            _nextQuestTransform.localPosition = _nextQuestTransform.localPosition.SetX(-_nextQuestDisplayAmountOfMovement / 2f);
            currentTime = 0;

            //�����\��
            while (currentTime < _nextQuestDisplayTime)
            {
                await UniTask.Yield();
                currentTime += Time.deltaTime;
                currentTime = Mathf.Min(_nextQuestDisplayTime, currentTime);

                _nextQuestTransform.localPosition = _nextQuestTransform.localPosition.SetX(-_nextQuestDisplayAmountOfMovement / 2f + _nextQuestDisplayAmountOfMovement * EasingManager.EaseProgress(TM.Easing.EaseType.Linear, currentTime, _nextQuestDisplayTime, 0, 0));
            }

            _nextQuestTransform.localPosition = _nextQuestTransform.localPosition.SetX(_nextQuestDisplayAmountOfMovement / 2f);
            currentTime = 0;

            //�X���C�h�A�E�g
            while (currentTime < _nextQuestSlideoutTime)
            {
                await UniTask.Yield();
                currentTime += Time.deltaTime;
                currentTime = Mathf.Min(_nextQuestSlideoutTime, currentTime);

                //�E
                //_nextQuestTransform.localPosition = _nextQuestTransform.position.SetX((_nextQuestDisplayAmountOfMovement / 2f) + (offset * EasingManager.EaseProgress(TM.Easing.EaseType.InCubic, currentTime, _nextQuestSlideoutTime, 0, 0)));

                //��
                _nextQuestTransform.localPosition = _nextQuestTransform.localPosition.SetX((_nextQuestDisplayAmountOfMovement / 2f) - ((offset + _nextQuestDisplayAmountOfMovement) * EasingManager.EaseProgress(TM.Easing.EaseType.InCubic, currentTime, _nextQuestSlideoutTime, 0, 0)));
            }

            _nextQuestTransform.localPosition = _nextQuestTransform.localPosition.SetX(OFFSET);
        }
        catch (MissingReferenceException)
        {
            return;
        }
    }

    private void SetText()
    {
        _nextQuestTextDatas[0].TextData.SetText(_nextQuestTexts[0]);

        switch (_nextQuestTexts.Count)
        {
            case 1:
                _nextQuestImageDatas[0].ImageData.SetPositionY(-270f);
                _nextQuestImageDatas[1].gameObject.SetActive(false);
                break;
            case 2:
                _nextQuestImageDatas[0].ImageData.SetPositionY(-180f);
                _nextQuestTextDatas[1].TextData.SetText(_nextQuestTexts[1]);
                _nextQuestImageDatas[1].gameObject.SetActive(true);
                break;
        }
    }
}
