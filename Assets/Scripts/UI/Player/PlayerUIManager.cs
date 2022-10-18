using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUIManager : MonoBehaviour
{
    [SerializeField, Tooltip("�󕠓x�e�L�X�g")] TextMeshProUGUI timeText;
    [SerializeField, Tooltip("�C�x���g�e�L�X�g")] TextUIData eventText;
    [SerializeField, Tooltip("�󕠓x�Q�[�W")] Slider[] timeGage;
    [SerializeField, Tooltip("�������Ԏc�ʌx���摜")] Sprite[] Warningimgs;
    [SerializeField, Tooltip("�������Ԏc�ʌx���I�u�W�F�N�g")] Image W_obj;

    public static float time = 0;
    private float maxTime;
    private float currentTime;
    private int[] warningTimes = new int[3];

    private Image[] w_imgs;

    GameObject[] ErasewithEatObj = new GameObject[3];

    private bool[] warningbool;
    public TextUIData EventText => eventText;

    public void SetTimeText(string text)
    {
        timeText.text = text;
    }

    private void Start()
    {
        maxTime = time;
        currentTime = maxTime;

        for (int i = 0; i < timeGage.Length; i++)
            timeGage[i].value = 1;
        for (int i = 0; i < warningTimes.Length - 1; i++)
            warningTimes[i] = (int)maxTime - ((i + 1) * 10);//���ŏ����l��2/3,1/3�̒l

        w_imgs = new Image[Warningimgs.Length];
        warningbool = new bool[warningTimes.Length];

        ErasewithEatObj[0] = GameObject.Find("DangoBackScreen");
        ErasewithEatObj[1] = GameObject.Find("QuestCanvas");
        ErasewithEatObj[2] = GameObject.Find("PlayerParent").transform.Find("Player1").transform.Find("RangeUI").gameObject;
    }
    private void Update()
    {
        if (time >= 0)
        {
            time -= Time.deltaTime;
        }
        else
        {
            //�Q�[���I�[�o�[����
        }
        currentTime = time;

        Warning();

        for (int i = 0; i < timeGage.Length; i++)//�Q�[�W�̑���
            timeGage[i].value = (float)currentTime / (float)maxTime;
        SetTimeText("" + (int)time);
    }

    private void Warning()
    {
        if (time > warningTimes[0])
        {
            W_obj.gameObject.SetActive(false);
        }
            for (int i = 0; i < warningTimes.Length; i++)
        {
            if (time < warningTimes[i] && !warningbool[i])
            {
                Logger.Log("������");
                W_obj.gameObject.SetActive(true);
                    W_obj.sprite = Warningimgs[i];
                    warningbool[i] = true;
            }
            else if (time >= warningTimes[i] && warningbool[i])//��i�K�オ�����ۂ̏���
            {
                W_obj.gameObject.SetActive(true);
                Logger.Log("a����");
                W_obj.sprite = Warningimgs[i];
                warningbool[i] = false;
            }
        }
    }
    public void EatDangoUI_False()
    {
        for (int i = 0; i < ErasewithEatObj.Length; i++)
            ErasewithEatObj[i].SetActive(false);
    }

    public void EatDangoUI_True()
    {
        for (int i = 0; i < ErasewithEatObj.Length; i++)
            ErasewithEatObj[i].SetActive(true);
    }
}
