using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DangoHighLight : MonoBehaviour
{
    [SerializeField] ImageUIData[] DangoImages;
    [SerializeField] ImageUIData[] FlashImage;
    [SerializeField] ImageUIData KusiImage;
    [SerializeField] Sprite temp;
    public void Stert(List<DangoColor>dangos, Image[] uidatas)
    {
        for (int i = 0; i < dangos.Count; i++)
        {
            DangoImages[i].ImageData.SetSprite(uidatas[i].sprite);
            DangoImages[i].ImageData.SetAlpha(0.01f);
        }
        for (int i = 0; i < dangos.Count; i++)
        {
            FlashImage[i].ImageData.FlashAlpha(-1f, 0.2f, 0f);
            Logger.Log("�n�C���C�g�J�n"+i);
        }
        KusiImage.ImageData.FlashAlpha(-1f, 0.2f, 0f);
    }
    public void Stop()
    {
        for (int i = 0; i < FlashImage.Length; i++)
        {
            FlashImage[i].ImageData.CancelFlash();
            Logger.Log("�n�C���C�g�I��"+i);
        }
        KusiImage.ImageData.CancelFlash();
    }
}
