using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUIManager : MonoBehaviour
{
    [SerializeField, Tooltip("�C�x���g�e�L�X�g")] TextUIData eventText;
    [SerializeField, Tooltip("�������Ԏc�ʌx���摜")] Sprite[] Warningimgs;
    [SerializeField, Tooltip("�������Ԏc�ʌx���I�u�W�F�N�g")] Image W_obj;
    [SerializeField] PlayerData playerdata;
    [SerializeField] GameObject MAXDangoOBJ;
    [SerializeField] ImageUIData DontEatUIOBJ;
    [SerializeField] Sprite dontEatSprite;
    [SerializeField] DangoUIScript dangoUIScript;
    [SerializeField] DangoHighLight dangoHighlight;
    [SerializeField] bool tutorial;
    [SerializeField] float[] warningTime;
    private float time { get { return playerdata.GetSatiety(); } }
    private float maxTime;
    private int[] warningTimes = new int[3];

    RoleCheck roleCheck = new RoleCheck();

    private Image[] w_imgs;

    [SerializeField] GameObject[] ErasewithEatObj = new GameObject[3];//�H�������ۂɂ�����UI 

    private bool[] warningbool = new bool[3];
    public TextUIData EventText => eventText;

    public bool Expansion;

    public float DefaultEventTextFontSize { get; } = 100f;

    private void Start()
    {
        maxTime = time;

        for (int i = 0; i < warningTimes.Length - 1; i++)
            warningTimes[i] = (int)maxTime - ((i + 1) * 10);//���ŏ����l��2/3,1/3�̒l

        w_imgs = new Image[Warningimgs.Length];
        warningbool = new bool[warningTimes.Length];
    }
    private void Update()
    {
        if (!PlayerData.Event)
            ScoreManager.Instance.AddTime();
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
    }

    public void MAXDangoSet(bool Active)
    {
        if (Active)
            MAXDangoOBJ.SetActive(true);
        else
            MAXDangoOBJ.SetActive(false);
    }

    public void DontEat()
    {
        DontEatUIOBJ.ImageData.SetSprite(dontEatSprite);
    }

    public void SetReach()
    {
        if (playerdata.GetDangos().Count == playerdata.GetCurrentStabCount() - 1)
        {
            DangoColor color = roleCheck.GetReach(playerdata.GetDangos(), playerdata.GetCurrentStabCount());
            if (color != DangoColor.None)
            {
                dangoUIScript.ReachSet(color, playerdata.GetCurrentStabCount());
                Logger.Log("reach����F" + color);
            }
        }
        else
            dangoUIScript.ReachClose();
    }

    public void StartDangoHighlight()
    {
        List<DangoColor> lists;
        lists = playerdata.GetDangos();
        Image[] temp;
        temp = dangoUIScript.GetDangos();
        dangoHighlight.Stert(lists, temp, playerdata.GetCurrentStabCount());
    }

    public void CancelHighlight()
    {
        Image[] temp;
        temp = dangoUIScript.GetDangos();
        dangoHighlight.Stop();
    }
}
