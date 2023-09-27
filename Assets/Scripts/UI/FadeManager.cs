using System.Collections;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class FadeManager : MonoBehaviour
{
    [SerializeField] ImageUIData image = default!;

    public void OnEnable()
    {
        image.ImageData.SetAlpha(0);
    }

    public void StartFade(FadeStyle fadeStyle, float duration)
    {
        switch (fadeStyle)
        {
            case FadeStyle.Fadein:
                image.ImageData.SetAlpha(0);
                image.ImageData.Fadein(duration).Forget();
                break;
            case FadeStyle.Fadeout:
                image.ImageData.SetAlpha(1);
                image.ImageData.Fadeout(duration).Forget();
                break;
        }
    }

}