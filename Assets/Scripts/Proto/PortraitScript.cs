using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using TM.Easing.Management;

public class PortraitScript : MonoBehaviour
{
    [SerializeField] Image img;
    [SerializeField] TextUIData text;
    [SerializeField] Sprite[] facePatterns;

    bool _isChangePortrait;

    const float OFFSET = -725f;
    const float SLIDETIME = 0.5f;

    //transform�̃C���X�^���X�擾
    Transform _trans;

    public bool IsChangePortrait => _isChangePortrait;

    private void Awake()
    {
        _trans = transform;
        _trans.localPosition = Vector3.zero.SetX(OFFSET);
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

    public void ChangeImg(Sprite image)
    {
        img.sprite = image;
    }

    private async UniTask SlideIn()
    {
        float time = 0;

        //�ʒu�̏�����
        _trans.localPosition = Vector3.zero.SetX(OFFSET);

        try
        {
            while (time < SLIDETIME)
            {
                await UniTask.Yield();
                if (!_trans.root.gameObject.activeSelf) continue;

                time += Time.deltaTime;

                _trans.localPosition = Vector3.zero.SetX(OFFSET * (1f - EasingManager.EaseProgress(TM.Easing.EaseType.OutQuart, time, SLIDETIME, 0f, 0)));
            }
        }
        catch (MissingReferenceException)
        {
            return;
        }

        _trans.localPosition = Vector3.zero;
    }

    private async UniTask SlideOut()
    {
        float time = 0;

        //�ʒu�̏�����
        _trans.localPosition = Vector3.zero;

        try
        {
            while (time < SLIDETIME)
            {
                await UniTask.Yield();
                if (!_trans.root.gameObject.activeSelf) continue;

                time += Time.deltaTime;

                _trans.localPosition = Vector3.zero.SetX(OFFSET * EasingManager.EaseProgress(TM.Easing.EaseType.InQuart, time, SLIDETIME, 0f, 0));
            }
        }
        catch (MissingReferenceException)
        {
            return;
        }

        _trans.localPosition = Vector3.zero.SetX(OFFSET);
    }

    public async UniTask ChangePortraitText(PortraitTextData questTextData)
    {
        //�C�x���g�i�s�I���܂őҋ@
        while (_isChangePortrait) await UniTask.Yield();

        if (questTextData.TextDataIndex == 0) return;

        //�f�[�^�����ݒ�
        PortraitTextData.PTextData data = questTextData.GetQTextData(0);
        ChangePortrait(data);

        await SlideIn();

        //�i�s���t���O���I���ɂ���
        _isChangePortrait = true;

        //�C�x���g�i�s
        for (int i = 0; i < questTextData.TextDataIndex; i++)
        {
            float time = 0;

            data = questTextData.GetQTextData(i);

            ChangePortrait(data);
            try
            {
                while (time < data.printTime)
                {
                    await UniTask.Yield();
                    if (_trans == null) return;
                    if (!_trans.root.gameObject.activeSelf) continue;

                    time += Time.deltaTime;
                }
            }
            catch (MissingReferenceException)
            {
                return;
            }
        }

        await SlideOut();

        //�i�s���t���O���I�t�ɂ���
        _isChangePortrait = false;
    }
}
