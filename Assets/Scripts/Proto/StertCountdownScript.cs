using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class StertCountdownScript : MonoBehaviour
{
    int a = 3;
    int i = 0;

    TextMeshProUGUI text;
    Animator animator;
    [SerializeField] string[] words;

    private void Start()
    {
        text = gameObject.GetComponent<TextMeshProUGUI>();
        animator = GetComponent<Animator>();
        animator.SetBool("stert", true);
        text.text = "3";
    }

    public void countDown()//�A�j���[�V��������Ăяo��
    {
        i++;
        text.text = (a - i).ToString("0");

        if (i == a)
            text.text = "�n�߁I";
        if (i > a)
        {
            text.text = "";
            animator.SetBool("stert", false);
        }
    }
}
