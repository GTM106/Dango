using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DangoUIScript : MonoBehaviour
{
    [SerializeField] GameObject[] Objs;
    [SerializeField] Sprite[] DangoImags;

    private Image[] DangoImagObjs;

    private void Start()
    {
        DangoImagObjs = new Image[Objs.Length];
        for(int i = 0; i < Objs.Length; i++) { 
            DangoImagObjs[i] = Objs[i].GetComponent<Image>();
            Objs[i].SetActive(false);
        }
    }
    public void DangoUISet(List<DangoColor> dangos)
    {
        for (int i = 0; i < dangos.Count; i++)
        {
            //�c�q�̎�ނ��݂ă}�e���A���ɐF��t����A�摜���o������imag��؂�ւ���B
            //�c�q���h�����Ă��Ȃ����̂�����Δ�A�N�e�B�u��
            Objs[i].SetActive(true);
            Logger.Log(dangos[i]);
            DangoImagObjs[i].sprite = DangoImags[(int)dangos[i]];
            //DangoImagObjs[i].sprite = dangos[i] switch
            //{
            //    DangoColor.Red => DangoImags[(int)DangoColor.Red],
            //    DangoColor.Orange => DangoImags[(int)DangoColor.Orange],
            //    DangoColor.Yellow => DangoImags[(int)DangoColor.Yellow],
            //    DangoColor.Green => DangoImags[(int)DangoColor.Green],
            //    DangoColor.Cyan => DangoImags[(int)DangoColor.Cyan],
            //    DangoColor.Blue => DangoImags[(int)DangoColor.Blue],
            //    DangoColor.Purple => DangoImags[(int)DangoColor.Purple],
            //    DangoColor.Other => DangoImags[(int)DangoColor.Other],
            //    _ => DangoImags[(int)DangoColor.Other],
            //};
        }
        for (int i = dangos.Count; i < Objs.Length; i++)
        {
            Objs[i].SetActive(false);
        }
    }
 
}
