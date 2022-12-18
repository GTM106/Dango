using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Video;

public class RoleDirectingScript : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI roleText = default!;
    [SerializeField] GameObject[] imageObj;
    [SerializeField] RoleEffect roleEffect;
    [SerializeField]VideoClip ittouVideo;
    [SerializeField] VideoClip zentenVideo;
    [SerializeField] VideoClip rinneVideo;
    [SerializeField] VideoClip rinsyokuVideo;
    [SerializeField] VideoClip sanmenVideo;
    [SerializeField]VideoPlayer now;
    private Image[] _images;

    void Start()
    {
        _images = new Image[imageObj.Length];

        for (int i = 0; i < imageObj.Length; i++)
        {
            _images[i] = imageObj[i].GetComponent<Image>();
            imageObj[i].SetActive(false);
        }
        now.gameObject.SetActive(false);
    }
    
    // Update is called once per frame
    void Update()
    {
        roleText.text = DangoRoleUI.CurrentRoleName;
    }

    public void Dirrecting(List<DangoColor> dangos)
    {
        //色は多分確定
        //ColorDirecting(dangos);
        //roleEffect.RoleSetEffect(dangos[0]);
        Video();
    }

    //色の演出
    private void ColorDirecting(List<DangoColor> color)
    {
        for (int i = 6; i > -1; i--)
        {
            if (color.Count > i)
            {
                imageObj[i].SetActive(true);
                _images[i].color = color[i] switch
                {
                    //この演出が必要か見直し
                    //DangoColor.None => throw new System.NotImplementedException(),
                    //DangoColor.An => throw new System.NotImplementedException(),
                    //DangoColor.Beni => throw new System.NotImplementedException(),
                    //DangoColor.Mitarashi => throw new System.NotImplementedException(),
                    //DangoColor.Nori => throw new System.NotImplementedException(),
                    //DangoColor.Shiratama => throw new System.NotImplementedException(),
                    //DangoColor.Yomogi => throw new System.NotImplementedException(),
                    DangoColor.Other => Color.gray,
                    _ => Color.white,
                };
            }
            else
            {
                imageObj[i].SetActive(false);
            }
        }
    }
    private void Video()
    {

        switch (DangoRoleUI.CurrentRoleName)
        {
            case "「一統団結」":
                now.clip = ittouVideo;
                break;
            case "「全天鏡面」":
                now.clip = zentenVideo;
                break;
            case "「輪廻転生」":
                now.clip = rinneVideo;
                break;
            case "「隣色鏡面」":
                now.clip = rinsyokuVideo;
                break;
            case "「三面華鏡」":
                now.clip = sanmenVideo;
                break;
        }
        now.gameObject.GetComponent<RawImage>().color = new Color(255, 255, 255, 255);
        now.Play();
        now.gameObject.SetActive(true);
        now.loopPointReached += finishedVideo;
        }

    private void finishedVideo(VideoPlayer vp)
    {
        now.clip = null;
        now.Stop();
        now.gameObject.GetComponent<RawImage>().color =new Color(255, 255, 255, 0);
        now.gameObject.SetActive(false);
    }
}
