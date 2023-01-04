using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TM.Easing.Management;
using UnityEngine;
using UnityEngine.UI;

public class StageSelectManager : MonoBehaviour
{
    Stage _currentStage;

    [SerializeField] FusumaManager _fusumaManager = default!;
    [SerializeField] StageData[] _stages;
    [SerializeField] Sprite[] _stageSprites;
    [SerializeField] Sprite[] _lockedStageSprites;
    [SerializeField] ImageUIData _stageImage = default!;
    [SerializeField] ImageUIData _stageImageUpdate = default!;
    [SerializeField] Image[] guids;
    [SerializeField] TextUIData _explanationText = default!;

    //アンロック演出
    [SerializeField] Canvas _unlockCanvas;
    [SerializeField] ImageUIData _unlockImage;
    [SerializeField] ImageUIData _stageUnlockImage;
    [SerializeField] ImageUIData _padlockImage;

    List<int> _unlockList = new();

    bool _isChange;
    bool _isUnlockDirection;

    const float IMAGE_FADEIN_TIME = 0.2f;
    const float DIRECTION_TIME = 1f;

    private void Awake()
    {
        _unlockCanvas.enabled = false;
        _stageUnlockImage.ImageData.SetAlpha(0);
    }

    private async void Start()
    {
        AssignCurrentStage();
        CheckUnlockDirection();
        UpdateExplanationText();

        await _fusumaManager.UniTaskOpen();

        InputSystemManager.Instance.onNavigatePerformed += OnChangeStage;
        InputSystemManager.Instance.onChoicePerformed += OnChoiced;
        InputSystemManager.Instance.onBackPerformed += OnBack;
        InputSystemManager.Instance.onAnyKeyPerformed += OnAnyKeyPerformed;
    }

    private void OnChangeStage()
    {
        if (_unlockCanvas.enabled) return;
        if (_isUnlockDirection) return;

        //更新中は処理しない（ここの処理はプレイ的にあまり良くない気がします。）
        if (_isChange) return;

        if (!ChangeChoiceUtil.Choice(InputSystemManager.Instance.NavigateAxis, ref _currentStage, Stage.Tutorial, false, ChangeChoiceUtil.OptionDirection.Horizontal)) return;

        SoundManager.Instance.PlaySE(SoundSource.SE16_UI_SELECTION);
        UpdateSprite(InputSystemManager.Instance.NavigateAxis.x > 0);
        UpdateGuids();
        UpdateExplanationText();
    }

    private async void OnChoiced()
    {
        if (_unlockCanvas.enabled) return;
        if (_isUnlockDirection) return;

        if (_isChange) return;
        if (!_stages[(int)_currentStage].IsRelease)
        {
            SoundManager.Instance.PlaySE(SoundSource.SE7_CANT_STAB_DANGO);
            return;
        }

        _isChange = true;

        SoundManager.Instance.PlaySE(SoundSource.SE17_UI_DECISION);

        DisposeInput();

        await _fusumaManager.UniTaskClose();

        SceneSystem.Instance.SetIngameScene(SceneSystem.Scenes.Stage1 + (int)_currentStage);
        SceneSystem.Instance.Load(SceneSystem.Scenes.Stage1 + (int)_currentStage);
        UnLoad();
    }

    private async void OnBack()
    {
        if (_unlockCanvas.enabled) return;
        if (_isUnlockDirection) return;

        //入力受付を終了
        DisposeInput();

        await _fusumaManager.UniTaskClose();
        SceneSystem.Instance.Load(SceneSystem.Scenes.Menu);
        UnLoad();
    }

    private async void OnAnyKeyPerformed()
    {
        if (!_unlockCanvas.enabled) return;

        //アンロック用のテキストをオフに
        _unlockCanvas.enabled = false;

        //アンロック演出中のフラグを立てる
        _isUnlockDirection = true;

        //演出終了まで待機
        while (_unlockList.Count > 0)
        {
            await UnlockDirection(this.GetCancellationTokenOnDestroy());
        }

        //アンロック演出フラグを折る
        _isUnlockDirection = false;
    }

    private async UniTask UnlockDirection(CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        int offset = _unlockList[0] - (int)_currentStage;

        float waitTime = offset == 0 ? 0 : 0.5f;

        //スライド演出待ち
        await WaitForUpdateSprite(offset);

       //南京錠をフェードアウト
        _padlockImage.ImageData.SetImageEnabled(true);
        _padlockImage.ImageData.SetAlpha(1f);
        _padlockImage.ImageData.Fadeout(DIRECTION_TIME, waitTime).Forget();

        //同時に正常な画像をフェードイン
        _stageUnlockImage.ImageData.SetSprite(_stageSprites[(int)_currentStage]);
        _stageUnlockImage.ImageData.SetAlpha(0);
        await _stageUnlockImage.ImageData.Fadein(DIRECTION_TIME, waitTime);

        //画像を通常スライドに設定し、アンロック用を消す
        _stageImage.ImageData.SetSprite(_stageSprites[(int)_currentStage]);
        _stageImageUpdate.ImageData.SetSprite(_stageSprites[(int)_currentStage]);
        _stageUnlockImage.ImageData.SetAlpha(0);

        //南京錠をもとに戻す
        _padlockImage.ImageData.SetAlpha(1);
        _padlockImage.ImageData.SetImageEnabled(false);

        _unlockList.RemoveAt(0);

        //セーブデータに保存
        DataManager.saveData.stagesStatus[(int)_currentStage] = (int)StageStatus.Unlock;
    }

    private async UniTask WaitForUpdateSprite(int offset)
    {
        if (offset == 0) return;

        //ステージをアンロックされた部分に変更
        _currentStage = (Stage)_unlockList[0];

        //該当ステージまでスクロール
        UpdateSprite(offset > 0);
        UpdateGuids();
        UpdateExplanationText();

        //スクロール演出を待機
        while (!_isChange) await UniTask.Yield();
    }

    private bool CheckUnlockDirection()
    {
        for (int i = 0; i < DataManager.saveData.stagesStatus.Length; i++)
        {
            //アンロックされているが、まだ演出をしていないもののみ行う
            if (DataManager.saveData.stagesStatus[i] == (int)StageStatus.StandbyForDirection)
            {
                //アンロック演出をONにする
                _unlockCanvas.enabled = true;

                _unlockList.Add(i);
            }
        }

        return _unlockCanvas.enabled;
    }

    private void AssignCurrentStage()
    {
        //アンロックされている最新のステージを選択
        foreach (var stage in _stages)
        {
            if (stage.IsRelease) _currentStage = (Stage)Mathf.Max((int)_currentStage, (int)stage.Stage);
        }

        //ガイドの描画更新
        UpdateGuids();

        //ステージ画像を更新
        SetSprite(_stageImage);
        SetSprite(_stageImageUpdate);
    }

    private void UpdateGuids()
    {
        //とりあえず全部ONにする
        foreach (var guid in guids) guid.gameObject.SetActive(true);

        //その後左右があるか確認して、Offにする
        if (_currentStage == 0) guids[0].gameObject.SetActive(false);
        if ((int)_currentStage == _stages.Length - 1) guids[1].gameObject.SetActive(false);
    }

    private async void UpdateSprite(bool isLeft)
    {
        _isChange = true;

        SetSprite(_stageImage);

        float width = _stageImage.ImageData.GetWidth();
        float center = 0;

        //スライドインっぽいけどスライドインじゃない違う処理
        await UniTask.WhenAll(//一応これで待機するが、全部同タイミングで終了する
         _stageImage.ImageData.MoveX(isLeft ? width : -width, isLeft ? -width : width, IMAGE_FADEIN_TIME),
         _stageImage.ImageData.WipeinHorizontal(IMAGE_FADEIN_TIME, isLeft ? Image.OriginHorizontal.Left : Image.OriginHorizontal.Right),
         _stageImageUpdate.ImageData.MoveX(center, isLeft ? -width : width, IMAGE_FADEIN_TIME),
         _stageImageUpdate.ImageData.WipeoutHorizontal(IMAGE_FADEIN_TIME, isLeft ? Image.OriginHorizontal.Right : Image.OriginHorizontal.Left));

        //フェードする側を設定し、フェードインさせる
        SetSprite(_stageImageUpdate);

        _isChange = false;
    }

    private async void UpdateExplanationText()
    {
        await _explanationText.TextData.Fadeout(IMAGE_FADEIN_TIME / 2f);

        string stageDescriptions = _currentStage switch
        {
            Stage.Stage1 => "ステージ1　難易度：★☆☆\n旅人は伝説の団子を求めて\n栄都城を訪れた……",
            Stage.Stage2 => "ステージ2　難易度：★★☆\nあん団子が増えた城内で、\n旅人は上を目指す",
            Stage.Stage3 => "ステージ3　難易度：★★★\n栄都城の最上階を目指して、\n最後の団道が始まる",
            _ => throw new System.NotImplementedException(),
        };

        _explanationText.TextData.SetText(stageDescriptions);

        await _explanationText.TextData.Fadein(IMAGE_FADEIN_TIME / 2f);
    }

    private void SetSprite(ImageUIData data)
    {
        bool isRelease = _stages[(int)_currentStage].IsRelease;

        //選択中のステージがアンロックされているかによって画像を切り替え
        Sprite[] sprites = isRelease ? _stageSprites : _lockedStageSprites;

        _padlockImage.ImageData.SetImageEnabled(!isRelease);

        data.ImageData.SetSprite(sprites[(int)_currentStage]);
    }

    private void UnLoad()
    {
        //仮に非同期アニメーション中だった場合破棄する
        _stageImageUpdate.ImageData.CancelUniTask();

        SceneSystem.Instance.UnLoad(SceneSystem.Scenes.StageSelect, true);
    }

    private void DisposeInput()
    {
        InputSystemManager.Instance.onNavigatePerformed -= OnChangeStage;
        InputSystemManager.Instance.onChoicePerformed -= OnChoiced;
        InputSystemManager.Instance.onBackPerformed -= OnBack;
        InputSystemManager.Instance.onAnyKeyPerformed -= OnAnyKeyPerformed;
    }
}