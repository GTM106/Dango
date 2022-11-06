using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class StertCountdownScript : MonoBehaviour
{
    int a = 3;
    int i=0;
    int y = 0;
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
    private void Update()
    {
        if (GameManager.GameClearFrag)
        {
            GameManager.GameClearFrag = false;
            Gameclear();
        }
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
    public void Gameclear()
    {
        StartCoroutine("clear");
    }

    IEnumerator clear()
    {
        text.fontSize *= 3;
        foreach (var word in words)
        {
            text.text =text.text +word;
            Logger.Log(word);
            yield return new WaitForSeconds(1f);
        }
        //�N���A�V�[���ֈړ�
        SceneSystem.Instance.UnLoad(SceneSystem.Scenes.Stage2, true);
        SceneSystem.Instance.Load(SceneSystem.Scenes.Success);
    }
}
