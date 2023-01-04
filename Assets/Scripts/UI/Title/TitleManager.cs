using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TitleManager : MonoBehaviour
{
    [SerializeField] FadeManager _fadeManager;
    [SerializeField] FusumaManager _fusumaManager;

    private void Start()
    {
        InputSystemManager.Instance.onAnyKeyPerformed += GameStart;
        SoundManager.Instance.PlayBGM(SoundSource.BGM4_TITLE);
        _fadeManager.StartFade(FadeStyle.Fadeout, 1f);
    }

    public async void GameStart()
    {
        var scenes = DataManager.saveData.completedTutorial ? SceneSystem.Scenes.Menu : SceneSystem.Scenes.Tutorial;

        InputSystemManager.Instance.onAnyKeyPerformed -= GameStart;

        await _fusumaManager.UniTaskClose();
        SoundManager.Instance.StopBGM();

        SceneSystem.Instance.SetIngameScene(SceneSystem.Scenes.Tutorial);
        SceneSystem.Instance.Load(scenes);
        SceneSystem.Instance.UnLoad(SceneSystem.Scenes.Title,true);
    }
}
