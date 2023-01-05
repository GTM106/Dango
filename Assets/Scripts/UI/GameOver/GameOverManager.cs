using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    enum GameOverChoice
    {
        Retry,
        BackToStageSelect,

        Max,
    }

    [SerializeField] Canvas _canvas;
    [SerializeField] ImageUIData _backGround;
    [SerializeField] ImageUIData _gameOverImage;
    [SerializeField] ImageUIData _retry;
    [SerializeField] ImageUIData _backToStageSelect;
    [SerializeField] Sprite[] _retrySprites;
    [SerializeField] Sprite[] _backToStageSprites;

    GameOverChoice _choice;

    private async void OnEnable()
    {
        //操作をUIに変更
        InputSystemManager.Instance.Input.SwitchCurrentActionMap("UI");

        SoundManager.Instance.PlaySE(SoundSource.SE22_GAMEOVERJINGLE);
       
        SetSprite();

        _backToStageSelect.ImageData.SetAlpha(0);

        _retry.ImageData.SetAlpha(0);
        _gameOverImage.ImageData.SetAlpha(0);

        //黒画面をフェードイン
        await _backGround.ImageData.Fadein(0.5f, 2.5f, 0);

        //上記処理が終わったら文字を表示
        await _gameOverImage.ImageData.Fadein(0.5f);

        await UniTask.WhenAll(
            _retry.ImageData.Fadein(0.5f),
            _backToStageSelect.ImageData.Fadein(0.5f)
            );

        InputSystemManager.Instance.onNavigatePerformed += OnNavigate;
        InputSystemManager.Instance.onChoicePerformed += OnChoice;
    }

    private void OnNavigate()
    {
        if (!ChangeChoiceUtil.Choice(InputSystemManager.Instance.NavigateAxis, ref _choice, GameOverChoice.Max, true, ChangeChoiceUtil.OptionDirection.Horizontal)) return;

        SetSprite();

        SoundManager.Instance.PlaySE(SoundSource.SE16_UI_SELECTION);
    }

    private void OnChoice()
    {
        InputSystemManager.Instance.onNavigatePerformed -= OnNavigate;
        InputSystemManager.Instance.onChoicePerformed -= OnChoice;

        SceneSystem.Instance.UnLoad(SceneSystem.Scenes.GameOver, true);
        SceneSystem.Instance.UnLoad(SceneSystem.Instance.CurrentIngameScene, true);

        SceneSystem.Scenes scene = _choice switch
        {
            GameOverChoice.Retry => SceneSystem.Instance.CurrentIngameScene,
            GameOverChoice.BackToStageSelect => SceneSystem.Scenes.StageSelect,
            _ => throw new System.NotImplementedException(),
        };

        if (scene == SceneSystem.Instance.CurrentIngameScene) SceneSystem.Instance.ReLoad(scene);
        else SceneSystem.Instance.Load(scene);
    }

    private void SetSprite()
    {
        _retry.ImageData.SetSprite(_retrySprites[_choice == GameOverChoice.Retry ? 1 : 0]);
        _backToStageSelect.ImageData.SetSprite(_backToStageSprites[_choice == GameOverChoice.BackToStageSelect ? 1 : 0]);
    }
}
