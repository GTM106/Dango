using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DangoUIScript : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Image[] DangoImagObjs;

    public void DangoUISet(List<DangoColor> dangos)
    {
        for (int i = 0; i < dangos.Count; i++)
        {
            //�c�q�̎�ނ��݂ă}�e���A���ɐF��t����A�摜���o������imag��؂�ւ���B
            //�c�q���h�����Ă��Ȃ����̂�����Δ�A�N�e�B�u��
            DangoImagObjs[i].color = dangos[i] switch
            {
                DangoColor.Red => Color.red,
                DangoColor.Orange => new Color32(255, 155, 0, 255),
                DangoColor.Yellow => Color.yellow,
                DangoColor.Green => Color.green,
                DangoColor.Cyan => Color.cyan,
                DangoColor.Blue => Color.blue,
                DangoColor.Purple => new Color32(200, 0, 255, 255),
                DangoColor.Other => Color.gray,
                _ => Color.white,
            };
        }
        for (int i = dangos.Count; i < DangoImagObjs.Length; i++)
        {
            DangoImagObjs[i].color = Color.white;
        }
    }
 
}
