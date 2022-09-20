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
    [SerializeField, Tooltip("�������Ԏc�ʌx��")] Image[] Warningimgs;

    public static float time = 0;
    private float maxTime;
    private float currentTime;
    private int[] warningTimes = new int[3];

    private RectTransform[] w_imgrects;
    private Image[] w_imgs;
    private Color w_color;

    private bool[] warningbool = new bool[3];
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
        w_imgrects = new RectTransform[Warningimgs.Length];

        for (int i = 0; i < Warningimgs.Length;i++) {
            w_imgrects[i] = Warningimgs[i].GetComponent<RectTransform>();
            w_imgs[i] = Warningimgs[i].GetComponent<Image>();
        }
        
        w_color = w_imgs[0].color;
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
        for (int i = 0; i < warningTimes.Length; i++)
        {
            if (time < warningTimes[i] && !warningbool[i])
            {
                for (int j = 0; j < Warningimgs.Length; j++)//��i�K���������ۂ̏���
                {
                    w_color = w_imgs[j].color;
                    w_imgrects[j].sizeDelta *= 1.2f;
                    w_imgs[j].color = new Color(w_color.r, w_color.g, w_color.b, w_color.a += (1f / 3f));
                }
                    warningbool[i] = true;
            }
            else if (time >= warningTimes[i] && warningbool[i])//��i�K�オ�����ۂ̏���
            {
                for (int j = 0; j < Warningimgs.Length; j++)
                {
                    w_color = w_imgs[j].color;
                    w_imgrects[j].localScale /= 1.2f;
                    w_imgs[j].color = new Color(w_color.r, w_color.g, w_color.b, w_color.a -= (1f / 3f));
                }
                warningbool[i] = false;
            }
        }
    }
}
