using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DangoUIScript : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Image[] DangoImagObjs;
    private DangoType[] PlayerDangos;
    void Start()
    {
        for (int i = 0; i < DangoImagObjs.Length; i++)
            DangoImagObjs[i].gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DangoUISet(DangoType[] dangos)
    {
        PlayerDangos = dangos;
        for(int i = 0; i < PlayerDangos.Length; i++)
        {
            DangoImagObjs[i].gameObject.SetActive(true);
            
            //�c�q�̎�ނ��݂ă}�e���A���ɐF��t����A�摜���o������imag��؂�ւ���B
            //�c�q���h�����Ă��Ȃ����̂�����Δ�A�N�e�B�u��
            switch (PlayerDangos[i])
            {
                case DangoType.Red:
                    DangoImagObjs[i].color = Color.red;
                    break;
                case DangoType.Orange:
                    DangoImagObjs[i].color = new Color(1,0.4f,0);
                    break;
                case DangoType.Yellow:
                    DangoImagObjs[i].color = Color.yellow;
                    break;
                case DangoType.Green:
                    DangoImagObjs[i].color = Color.green;
                    break;
                case DangoType.Cyan:
                    DangoImagObjs[i].color = Color.cyan;
                    break;
                case DangoType.Blue:
                    DangoImagObjs[i].color = Color.blue;
                    break;
                case DangoType.Purple:
                    DangoImagObjs[i].color = new Color(0.6f,0,0.6f);
                    break;
                case DangoType.Other:
                    DangoImagObjs[i].color = Color.gray;
                    break;
                default:
                    DangoImagObjs[i].gameObject.SetActive(false);
                    break;
            }
        }
    }
}
