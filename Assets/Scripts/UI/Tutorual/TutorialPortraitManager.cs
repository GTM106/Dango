using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using TM.Easing.Management;
using UnityEngine;
using UnityEngine.UI;

//��{�̃|�[�g���[�g�\���ƊT�˓����ł��B
//�������A�`���[�g���A�����菈���������������肷��̂͏璷�Ɣ��f��������
//�`���[�g���A����p�̃X�N���v�g�Ƃ��ēƗ������܂����B
public class TutorialPortraitManager : MonoBehaviour, IChangePortrait
{
    [SerializeField] Image img;
    [SerializeField] TextUIData text;
    [SerializeField] Sprite[] facePatterns;
    [SerializeField] ImageUIData U11Image;
    [SerializeField] ImageUIData U8Image;

    bool _isChangePortrait;

    const float OFFSET = -1750f;
    const float SLIDETIME = 0.5f;

    //transform�̃C���X�^���X�擾
    Transform _transform;

    public bool IsChangePortrait => _isChangePortrait;

    private void Awake()
    {
        _transform = transform;
        _transform.localPosition = Vector3.zero.SetX(OFFSET);

        U11Image.ImageData.FlashAlpha(-1f, 0.7f, 0);
    }

    private void ChangePortrait(PortraitTextData.PTextData data)
    {
        if (string.IsNullOrEmpty(data.text))
        {
            text.TextData.SetText();
            return;
        }

        text.TextData.SetText(data.text);
        img.sprite = facePatterns[(int)data.face];
    }

    private async UniTask SlideIn()
    {
        float time = 0;
        try
        {
            //�ʒu�̏�����
            _transform.localPosition = Vector3.zero.SetX(OFFSET);

            while (time < SLIDETIME)
            {
                await UniTask.Yield();
                if (!_transform.root.gameObject.activeSelf) continue;

                time += Time.deltaTime;

                _transform.localPosition = Vector3.zero.SetX(OFFSET * (1f - EasingManager.EaseProgress(TM.Easing.EaseType.OutQuart, time, SLIDETIME, 0f, 0)));
            }
        }
        catch (MissingReferenceException)
        {
            return;
        }

        _transform.localPosition = Vector3.zero;
    }

    private async UniTask SlideOut()
    {
        float time = 0;

        //�ʒu�̏�����
        _transform.localPosition = Vector3.zero;

        try
        {
            while (time < SLIDETIME)
            {
                await UniTask.Yield();
                if (!_transform.root.gameObject.activeSelf) continue;

                time += Time.deltaTime;

                _transform.localPosition = Vector3.zero.SetX(OFFSET * EasingManager.EaseProgress(TM.Easing.EaseType.InQuart, time, SLIDETIME, 0f, 0));
            }
        }
        catch (MissingReferenceException)
        {
            return;
        }

        _transform.localPosition = Vector3.zero.SetX(OFFSET);
    }

    public async UniTask ChangePortraitText(PortraitTextData questTextData)
    {
        PlayerData.IsClear = false;

        //�C�x���g�i�s�I���܂őҋ@
        while (_isChangePortrait) await UniTask.Yield();

        if (questTextData.TextDataIndex == 0) return;

        //�f�[�^�����ݒ�
        ChangePortrait(questTextData.GetQTextData(0));

        //�`���[�g���A���͕�����ǂ܂���������UI�}�b�v�ɕς���
        InputSystemManager.Instance.Input.SwitchCurrentActionMap("UI");

        U8Image.ImageData.SetImageEnabled(false);

        await SlideIn();

        //�i�s���t���O���I���ɂ���
        _isChangePortrait = true;

        //�C�x���g�i�s
        for (int i = 0; i < questTextData.TextDataIndex; i++)
        {
            //�f�[�^�̕ύX
            ChangePortrait(questTextData.GetQTextData(i));

            await UniTask.DelayFrame(50, PlayerLoopTiming.FixedUpdate);

            try
            {
                while (!InputSystemManager.Instance.IsPressChoice)
                {
                    await UniTask.Yield();
                    if (!_transform.root.gameObject.activeSelf) continue;
                }
            }
            catch (MissingReferenceException)
            {
                return;
            }
        }

        //�A�N�V�����}�b�v�����Ƃɖ߂�
        InputSystemManager.Instance.Input.SwitchCurrentActionMap("Player");
        U8Image.ImageData.SetImageEnabled(true);

        await SlideOut();

        //�i�s���t���O���I�t�ɂ���
        _isChangePortrait = false;
        PlayerData.IsClear = true;
        Logger.Log(PlayerData.IsClear);
    }

}
