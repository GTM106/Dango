using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUIManager : MonoBehaviour
{
    [SerializeField, Tooltip("�󕠓x�e�L�X�g")] TextMeshProUGUI timeText;
    [SerializeField, Tooltip("�C�x���g�e�L�X�g")] TextMeshProUGUI eventText;

    public void SetTimeText(string text)
    {
        timeText.text = text;
    }

    public void SetEventText(string text)
    {
        eventText.text = text;
    }
}
