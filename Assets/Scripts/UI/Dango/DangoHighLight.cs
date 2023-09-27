using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DangoHighLight : MonoBehaviour
{
    [SerializeField] ImageUIData[] DangoImages;
    [SerializeField] ImageUIData[] FlashImage;
    [SerializeField] Sprite[] kusiSprites;
    [SerializeField] ImageUIData KusiFlashImage;
    [SerializeField] ImageUIData KusiImage;
    private Sprite temp;
    public void Stert(List<DangoColor> dangos, Image[] uidatas, int D5Num)
    {
        for (int i = 0; i < dangos.Count; i++)
        {
            DangoImages[i].ImageData.SetSprite(uidatas[i].sprite);
            DangoImages[i].ImageData.SetAlpha(0.01f);
        }
        for (int i = 0; i < dangos.Count; i++)
        {
            FlashImage[i].ImageData.FlashAlpha(-1f, 0.2f, 0f);
        }

        KusiImage.ImageData.SetSprite(kusiSprites[D5Num - 3]);
        KusiImage.ImageData.SetAlpha(0.01f);
        KusiFlashImage.ImageData.FlashAlpha(-1f, 0.2f, 0f);
    }
    public void Stop()
    {
        for (int i = 0; i < FlashImage.Length; i++)
        {
            FlashImage[i].ImageData.CancelFlash();
        }
        KusiFlashImage.ImageData.CancelFlash();
    }
}