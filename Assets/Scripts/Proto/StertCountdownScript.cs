using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class StertCountdownScript : MonoBehaviour
{
    int a = 3;
    int i=0;
    TextMeshProUGUI text;

    private void Start()
    {
        text = gameObject.GetComponent<TextMeshProUGUI>();
        text.text = "3";
    }
    public void countDown()//�A�j���[�V��������Ăяo��
    {
        i++;
        text.text = (a - i).ToString("0");

        if (i == a)
            text.text = "�n�߁I";
        
        if (i > a)
            Destroy(gameObject);
    }
}
