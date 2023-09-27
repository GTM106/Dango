using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TitleManager : MonoBehaviour
{
    [SerializeField] FadeManager _fadeManager;
    [SerializeField] FusumaManager _fusumaManager;
    [SerializeField] TextUIData _TitleText;

    private void Start()
    {
        InputSystemManager.Instance.onAnyKeyPerformed += GameStart;
        SoundManager.Instance.PlayBGM(SoundSource.BGM4_TITLE);
        _fadeManager.StartFade(FadeStyle.Fadeout, 1f);
        _TitleText.TextData.FlashAlpha(-1, 0.7f, 0);
    }

    public async void GameStart()
    {
        var scenes = (DataManager.saveData.tutorialStatusBit & (1 << 0)) != 0 ? SceneSystem.Scenes.Menu : SceneSystem.Scenes.Tutorial1;

        InputSystemManager.Instance.onAnyKeyPerformed -= GameStart;

        await _fusumaManager.UniTaskClose();
        SoundManager.Instance.StopBGM();

        SceneSystem.Instance.SetIngameScene(SceneSystem.Scenes.Tutorial1);
        SceneSystem.Instance.Load(scenes);
        SceneSystem.Instance.UnLoad(SceneSystem.Scenes.Title,true);
    }
}
