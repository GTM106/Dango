using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUIManager : MonoBehaviour
{
    [SerializeField, Tooltip("�󕠓x�e�L�X�g")] TextMeshProUGUI timeText;
    [SerializeField, Tooltip("�C�x���g�e�L�X�g")] EventTextUI eventText;
    [SerializeField, Tooltip("�󕠓x�Q�[�W")] Slider timeGage;
    public static float time = 0;
    private float maxTime;
    private float currentTime;

    public EventTextUI EventText => eventText;

    public void SetTimeText(string text)
    {
        timeText.text = text;
    }

    private void Start()
    {
        maxTime = time;
        currentTime = maxTime;
        timeGage.value = 1;
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
        timeGage.value = (float)currentTime / (float)maxTime;
        SetTimeText("" + (int)time);
    }
}
