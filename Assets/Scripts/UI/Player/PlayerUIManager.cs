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
    [SerializeField] PlayerData playerdata;
    [SerializeField] GameObject MAXDangoOBJ;
    [SerializeField] Animator[] TimegageAnima;
    [SerializeField] TimeGageAnima[] timeGageAnimaText;
    [SerializeField] ImageUIData DontEatUIOBJ;
    [SerializeField] Sprite dontEatSprite;
    private float time { get { return playerdata.GetSatiety(); } }
    private float maxTime;
    private float currentTime;
    private int[] warningTimes = new int[3];


    private Image[] w_imgs;

    [SerializeField] GameObject[] ErasewithEatObj = new GameObject[3];//�H�������ۂɂ�����UI 

    private bool[] warningbool = new bool[3];
    public TextUIData EventText => eventText;

    public bool Expansion;

    public float DefaultEventTextFontSize { get; } = 100f;

    float temp=0;

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

        //GameObject.Find���g���Ȃ�V���A���C�Y���Ď擾���܂��傤�B���̕������O�ɔ���ꂸ�m���ł��B
        //ErasewithEatObj[0] = GameObject.Find("DangoBackScreen");
        //ErasewithEatObj[1] = GameObject.Find("QuestCanvas");
        //ErasewithEatObj[2] = GameObject.Find("PlayerParent").transform.Find("Player1").transform.Find("RangeUI").gameObject;
    }
    private void Update()
    {
        currentTime = time;
        if(!PlayerData.Event)
        ScoreManager.Instance.AddTime();

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
                W_obj.gameObject.SetActive(true);
                W_obj.sprite = Warningimgs[i];
                warningbool[i] = true;
            }
            else if (time >= warningTimes[i] && warningbool[i])//��i�K�オ�����ۂ̏���
            {
                W_obj.gameObject.SetActive(true);
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
        {
            if (i == 2 && Expansion) continue;

            ErasewithEatObj[i].SetActive(true);
        }
        TimeGageUpAnima();
    }

    public void MAXDangoSet(bool Active)
    {
        if (Active)
            MAXDangoOBJ.SetActive(true);
        else
            MAXDangoOBJ.SetActive(false);
    }

    public void ScoreCatch(float score)
    {
        temp += score;
    }

    public void TimeGageUpAnima()
    {
        for (int i = 0; i < timeGageAnimaText.Length; i++)
        {
            timeGageAnimaText[i].SetText(temp);
            TimegageAnima[i].SetBool("Play",true);
        }
        temp = 0;
    }

    public void DontEat()
    {
        DontEatUIOBJ.ImageData.SetSprite(dontEatSprite);
    }
}
