using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Video;
using Cysharp.Threading.Tasks;

public class RoleDirectingScript : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI roleText;
    [SerializeField] GameObject[] imageObj;
    [SerializeField] RoleEffect roleEffect;
    [SerializeField] VideoClip ittouVideo;
    [SerializeField] VideoClip zentenVideo;
    [SerializeField] VideoClip rinneVideo;
    [SerializeField] VideoClip rinsyokuVideo;
    [SerializeField] VideoClip sanmenVideo;
    [SerializeField] VideoPlayer now;
    [SerializeField] RawImage _videoPlayerImage;
    //private Image[] _images;

    private void Awake()
    {
        now.loopPointReached += FinishedVideo;
        _videoPlayerImage.enabled = false;
    }

    private void OnDisable()
    {
        now.loopPointReached -= FinishedVideo;
    }

    //void Start()
    //{
    //    //_images = new Image[imageObj.Length];

    //    //for (int i = 0; i < imageObj.Length; i++)
    //    //{
    //    //    _images[i] = imageObj[i].GetComponent<Image>();
    //    //    imageObj[i].SetActive(false);
    //    //}
    //}

    // Update is called once per frame
    //void Update()
    //{
    //    //roleText.text = DangoRoleUI.CurrentRoleName;
    //}

    //public void Dirrecting(List<DangoColor> dangos)
    //{
    //    //�F�͑����m��
    //    //ColorDirecting(dangos);
    //    //roleEffect.RoleSetEffect(dangos[0]);
    //}

    public void PlayRoleVideo()
    {
        try
        {
            now.clip = DangoRoleUI.CurrentRoleName switch
            {
                "�u�ꓝ�c���v" => ittouVideo,
                "�u�S�V���ʁv" => zentenVideo,
                "�u�։��]���v" => rinneVideo,
                "�u�אF���ʁv" => rinsyokuVideo,
                "�u�O�ʉ؋��v" => sanmenVideo,
                _ => throw new System.NotImplementedException(),
            };
        }
        catch (System.NotImplementedException)
        {
            return;
        }

        //���̏�Ԃł́A�ŏ��̐��t���[���͒��O�ɍĐ����ꂽ���o�̍ŏI�t���[�����Đ�����܂��B
        //����́A�S����̍ŏI�t���[���͑S�������ɂ��邱�Ƃŉ������Ă��܂��B�i���ꂪ�d�l�����炱�̍������Ă��܂��B�j
        now.Play();
        _videoPlayerImage.enabled = true;
    }

    //�F�̉��o
    //private void ColorDirecting(List<DangoColor> color)
    //{
    //    for (int i = 6; i > -1; i--)
    //    {
    //        if (color.Count > i)
    //        {
    //            imageObj[i].SetActive(true);
    //            _images[i].color = color[i] switch
    //            {
    //                //���̉��o���K�v��������
    //                //DangoColor.None => throw new System.NotImplementedException(),
    //                //DangoColor.An => throw new System.NotImplementedException(),
    //                //DangoColor.Beni => throw new System.NotImplementedException(),
    //                //DangoColor.Mitarashi => throw new System.NotImplementedException(),
    //                //DangoColor.Nori => throw new System.NotImplementedException(),
    //                //DangoColor.Shiratama => throw new System.NotImplementedException(),
    //                //DangoColor.Yomogi => throw new System.NotImplementedException(),
    //                DangoColor.Other => Color.gray,
    //                _ => Color.white,
    //            };
    //        }
    //        else
    //        {
    //            imageObj[i].SetActive(false);
    //        }
    //    }
    //}

    private void FinishedVideo(VideoPlayer vp)
    {
        now.Stop();
        now.clip = null;
        _videoPlayerImage.enabled = false;
    }
}
