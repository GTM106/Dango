using Cysharp.Threading.Tasks;
using System.Threading;
using TM.Easing;
using TM.Easing.Management;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    enum Menu
    {
        None,

        StageSelect,
        Option,
        Tutorial,
        Quit,

        Max,
    }

    enum CharaFacePatturn
    {
        Normal,
        Sad
    }

    [SerializeField] FusumaManager _fusumaManager;
    [SerializeField] RectTransform option;
    [SerializeField] RectTransform tutorial;
    [SerializeField] RectTransform ex;
    [SerializeField] Image dandou;
    [SerializeField] Sprite[] dandouSprites;
    [SerializeField] Image optionImage;
    [SerializeField] Sprite[] optionSprites;
    [SerializeField] Image tutorialImage;
    [SerializeField] Sprite[] tutorialSprites;
    [SerializeField] Image exImage;
    [SerializeField] Sprite[] exSprites;
    [SerializeField] Image quitImage;

    [SerializeField] Canvas _quitCanvas;
    [SerializeField] Image[] quitImages;

    [SerializeField] ImageUIData _chara;
    [SerializeField] Sprite[] _charaSprite;

    Menu _currentMenu = Menu.StageSelect;
    bool _isTransition;

    bool isSelectedQuit;

    const float SELECTTIME = 1f;
    const float NOSELECTTIME = 1f;

    const float WIDTH_MAX = 700f;
    //const float WIDTH_MIN = 0f;
    const EaseType EASETYPE = EaseType.OutBack;

    CancellationToken _cancellationToken;

    bool _isCharaOut;
    bool _isCharaIn;

    const float CHARASTARTPOS = -500f;
    const float CHARAOFFSET = 500f;

    private Menu CurrentMenu
    {
        get => _currentMenu;
        set
        {
            SetNoSelect(_currentMenu);
            _currentMenu = value;
        }
    }

    private void Awake()
    {
        _cancellationToken = gameObject.GetCancellationTokenOnDestroy();
    }

    private async void Start()
    {
        SetSelect();

        SetNoSelect(Menu.Option);
        SetNoSelect(Menu.Tutorial);

        await _fusumaManager.UniTaskOpen(1f);

        InputSystemManager.Instance.onNavigatePerformed += OnNavigate;
        InputSystemManager.Instance.onNavigatePerformed += QuitNavgate;
        InputSystemManager.Instance.onChoicePerformed += OnChoice;
        InputSystemManager.Instance.onBackPerformed += OnCancel;

        SoundManager.Instance.PlayBGM(SoundSource.BGM5_MENU);
    }

    private void OnNavigate()
    {
        if (_isTransition) return;
        if (_quitCanvas.enabled) return;

        if (InputSystemManager.Instance.NavigateAxis == Vector2.left)
        {
            CurrentMenu--;
            if (CurrentMenu == Menu.None) CurrentMenu = Menu.Max - 1;
        }
        else if (InputSystemManager.Instance.NavigateAxis == Vector2.right)
        {
            CurrentMenu++;
            if (CurrentMenu == Menu.Max) CurrentMenu = Menu.None + 1;
        }
        else if (InputSystemManager.Instance.NavigateAxis == Vector2.up)
        {
            CurrentMenu = Menu.Quit;
        }

        SoundManager.Instance.PlaySE(SoundSource.SE16_UI_SELECTION);
        SetSelect();
    }

    private async void OnChoice()
    {
        if (_isTransition) return;
        _isTransition = true;
        SoundManager.Instance.PlaySE(SoundSource.SE17_UI_DECISION);

        if (CurrentMenu == Menu.Tutorial) SoundManager.Instance.StopBGM(1.5f);

        if (CurrentMenu == Menu.Quit && !isSelectedQuit)
        {
            SetQuitChoiceColor(isSelectedQuit);

            _quitCanvas.enabled ^= true;
            _isTransition = false;
            return;
        }

        //ëÄçÏÇÃéÛïtÇèIóπ
        InputSystemManager.Instance.onNavigatePerformed -= OnNavigate;
        InputSystemManager.Instance.onNavigatePerformed -= QuitNavgate;
        InputSystemManager.Instance.onChoicePerformed -= OnChoice;
        InputSystemManager.Instance.onBackPerformed -= OnCancel;

        await _fusumaManager.UniTaskClose(1.5f);

        switch (CurrentMenu)
        {
            case Menu.Option:
                ToOption();
                break;
            case Menu.Tutorial:
                ToTutorial();
                break;
            case Menu.StageSelect:
                ToStageSelect();
                break;
            case Menu.Quit:
                ToQuit();
                break;
        }
    }

    private void OnCancel()
    {
        if (!_quitCanvas.enabled) return;

        _quitCanvas.enabled = false;
        isSelectedQuit = false;
    }

    private async void SetSelect()
    {
        switch (CurrentMenu)
        {
            case Menu.Option:
                CharaAnimationOut(_cancellationToken).Forget();
                optionImage.sprite = optionSprites[1];
                await Selecting(_cancellationToken, CurrentMenu, option, SELECTTIME);
                break;
            case Menu.Tutorial:
                CharaAnimationOut(_cancellationToken).Forget();
                tutorialImage.sprite = tutorialSprites[1];
                await Selecting(_cancellationToken, CurrentMenu, tutorial, SELECTTIME);
                break;
            case Menu.StageSelect:
                CharaAnimationIn(_cancellationToken, CharaFacePatturn.Normal).Forget();
                dandou.sprite = dandouSprites[1];
                break;
            case Menu.Quit:
                CharaAnimationIn(_cancellationToken, CharaFacePatturn.Sad).Forget();
                quitImage.color = Color.red;
                break;
        }
    }

    private async UniTask CharaAnimationIn(CancellationToken token, CharaFacePatturn patturn)
    {
        token.ThrowIfCancellationRequested();

        _chara.ImageData.SetSprite(_charaSprite[(int)patturn]);

        if (_isCharaIn) return;

        float currentTime = 0;
        float animationTime = 1f;

        _isCharaOut = false;
        _isCharaIn = true;

        while (currentTime < animationTime)
        {
            if (!_isCharaIn) break;

            await UniTask.Yield();

            currentTime += Time.deltaTime;

            float t = EasingManager.EaseProgress(EaseType.OutExpo, currentTime, animationTime, 0f, 0);

            _chara.ImageData.SetPositionX(CHARASTARTPOS + (CHARAOFFSET * t));
        }
    }

    private async UniTask CharaAnimationOut(CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        if (_isCharaOut) return;

        float currentTime = 0;
        float offset = CHARASTARTPOS - _chara.ImageData.GetPosition().x;
        float o = 1f - ((CHARAOFFSET + offset) / CHARAOFFSET);
        float animationTime = 0.5f * o;
        float startPos = _chara.ImageData.GetPosition().x;

        _isCharaIn = false;
        _isCharaOut = true;

        while (currentTime < animationTime)
        {
            if (!_isCharaOut) break;

            await UniTask.Yield();

            currentTime += Time.deltaTime;

            float t = EasingManager.EaseProgress(EaseType.InExpo, currentTime, animationTime, 0f, 0);

            _chara.ImageData.SetPositionX(startPos + (offset * (t)));
        }
    }

    private async UniTask Selecting(CancellationToken token, Menu menu, RectTransform rect, float time, float waitTime = 0)
    {
        token.ThrowIfCancellationRequested();

        await UniTask.Delay((int)(waitTime * 1000f));
        float currentTime = 0;

        Vector2 pos = new(0, 0);
        while (currentTime <= time)
        {
            if (CurrentMenu != menu) break;

            await UniTask.Yield();
            currentTime += Time.deltaTime;
            float d = EasingManager.EaseProgress(EASETYPE, currentTime, time, 3f, 0);

            pos.Set(rect.sizeDelta.x, WIDTH_MAX - (WIDTH_MAX * (1 - d)));
            rect.sizeDelta = pos;
        }
    }

    private async void SetNoSelect(Menu menu)
    {
        switch (menu)
        {
            case Menu.Option:
                optionImage.sprite = optionSprites[0];
                await NoSelecting(_cancellationToken, menu, option, NOSELECTTIME);
                break;
            case Menu.Tutorial:
                tutorialImage.sprite = tutorialSprites[0];
                await NoSelecting(_cancellationToken, menu, tutorial, NOSELECTTIME);
                break;
            case Menu.StageSelect:
                dandou.sprite = dandouSprites[0];
                break;
            case Menu.Quit:
                quitImage.color = Color.white;
                break;
        }
    }

    private async UniTask NoSelecting(CancellationToken token, Menu menu, RectTransform rect, float time, float waitTime = 0)
    {
        token.ThrowIfCancellationRequested();

        await UniTask.Delay((int)(waitTime * 1000f));
        float currentTime = 0;

        Vector2 pos = new(0, 0);
        while (currentTime <= time)
        {
            if (CurrentMenu == menu) break;

            await UniTask.Yield();
            currentTime += Time.deltaTime;
            float d = EasingManager.EaseProgress(EASETYPE, currentTime, time, 3f, 0);

            pos.Set(rect.sizeDelta.x, WIDTH_MAX * (1 - d));
            rect.sizeDelta = pos;
        }
    }

    private void QuitNavgate()
    {
        if (!_quitCanvas.enabled) return;
        if (_currentMenu != Menu.Quit) return;

        Vector2 axis = InputSystemManager.Instance.NavigateAxis;

        if (axis != Vector2.left && axis != Vector2.right) return;
        if (isSelectedQuit && axis == Vector2.left) return;
        if (!isSelectedQuit && axis == Vector2.right) return;

        isSelectedQuit = axis.Equals(Vector2.left);
        SetQuitChoiceColor(isSelectedQuit);
        SoundManager.Instance.PlaySE(SoundSource.SE16_UI_SELECTION);
    }

    private void SetQuitChoiceColor(bool isSelectedQuit)
    {
        quitImages[isSelectedQuit ? 1 : 0].color = new Color32(176, 176, 176, 255);
        quitImages[isSelectedQuit ? 0 : 1].color = Color.red;
    }

    private void ToOption()
    {
        SceneSystem.Instance.Load(SceneSystem.Scenes.Option);
        Unload();
    }
    private void ToTutorial()
    {
        SceneSystem.Instance.Load(SceneSystem.Scenes.TutorialHub);
        Unload();
    }
    private void ToStageSelect()
    {
        SceneSystem.Instance.Load(SceneSystem.Scenes.StageSelect);
        Unload();
    }

    private void ToQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

    private void Unload()
    {
        SceneSystem.Instance.UnLoad(SceneSystem.Scenes.Menu, true);
    }
}