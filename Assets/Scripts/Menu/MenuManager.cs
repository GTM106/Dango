using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using System.Collections;
using System.Collections.Generic;
using TM.Easing.Management;
using TM.Easing;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    enum Menu
    {
        None,

        Option,
        Tutorial,
        Ex,

        Max,
    }

    [SerializeField] FusumaManager _fusumaManager;
    [SerializeField] RectTransform option;
    [SerializeField] RectTransform tutorial;
    [SerializeField] RectTransform ex;
    [SerializeField] Image optionImage;
    [SerializeField] Image tutorialImage;
    [SerializeField] Image exImage;
    Menu _currentMenu = Menu.Option;

    const float SELECTTIME = 1f;
    const float NOSELECTTIME = 1f;

    const float WIDTH_MAX = 400f;
    //const float WIDTH_MIN = 0f;
    const EaseType EASETYPE = EaseType.OutBack;

    private Menu CurrentMenu
    {
        get => _currentMenu;
        set
        {
            SetNoSelect(_currentMenu);
            _currentMenu = value;
        }
    }

    private void Start()
    {
        InputSystemManager.Instance.onNavigatePerformed += OnNavigate;
        InputSystemManager.Instance.onChoicePerformed += OnChoice;

        SetSelect();
        SetNoSelect(Menu.Tutorial);
        SetNoSelect(Menu.Ex);

        _fusumaManager.Open(1f);
    }

    private void OnNavigate()
    {
        if (InputSystemManager.Instance.NavigateAxis == Vector2.up)
        {
            CurrentMenu--;
            if (CurrentMenu == Menu.None) CurrentMenu = Menu.Max - 1;
        }
        else if (InputSystemManager.Instance.NavigateAxis == Vector2.down)
        {
            CurrentMenu++;
            if (CurrentMenu == Menu.Max) CurrentMenu = Menu.None + 1;
        }

        SetSelect();
        Logger.Log(CurrentMenu);
    }

    private void OnChoice()
    {
        switch (CurrentMenu)
        {
            case Menu.Option:
                ToOption();
                break;
            case Menu.Tutorial:
                ToTutorial();
                break;
            case Menu.Ex:
                ToEx();
                break;
        }
    }

    private async void SetSelect()
    {
        switch (CurrentMenu)
        {
            case Menu.Option:
                //�����̃J���[�ύX��I���摜�ɕύX�����OK
                optionImage.color = Color.red;
                await Selecting(CurrentMenu, option, SELECTTIME);
                break;
            case Menu.Tutorial:
                tutorialImage.color = Color.red;
                await Selecting(CurrentMenu, tutorial, SELECTTIME);
                break;
            case Menu.Ex:
                exImage.color = Color.red;
                await Selecting(CurrentMenu, ex, SELECTTIME);
                break;
        }
    }

    private async UniTask Selecting(Menu menu, RectTransform rect, float time, float waitTime = 0)
    {
        await UniTask.Delay((int)(waitTime * 1000f));
        float currentTime = 0;

        Vector2 pos = new Vector2(0, 0);
        while (currentTime <= time)
        {
            if (CurrentMenu != menu) break;

            await UniTask.DelayFrame(1);
            currentTime += Time.deltaTime;
            float d = EasingManager.EaseProgress(EASETYPE, currentTime, time, 3f, 0);

            pos.Set(WIDTH_MAX - (WIDTH_MAX * (1 - d)), rect.sizeDelta.y);
            rect.sizeDelta = pos;
        }
    }

    private async void SetNoSelect(Menu menu)
    {
        switch (menu)
        {
            case Menu.Option:
                //�����̃J���[�ύX��I���摜�ɕύX�����OK
                optionImage.color = Color.white;
                await NoSelecting(menu, option, NOSELECTTIME);
                break;
            case Menu.Tutorial:
                tutorialImage.color = Color.white;
                await NoSelecting(menu, tutorial, NOSELECTTIME);
                break;
            case Menu.Ex:
                exImage.color = Color.white;
                await NoSelecting(menu, ex, NOSELECTTIME);
                break;
        }
    }

    private async UniTask NoSelecting(Menu menu, RectTransform rect, float time, float waitTime = 0)
    {
        await UniTask.Delay((int)(waitTime * 1000f));
        float currentTime = 0;

        Vector2 pos = new Vector2(0, 0);
        while (currentTime <= time)
        {
            if (CurrentMenu == menu) break;

            await UniTask.DelayFrame(1);
            currentTime += Time.deltaTime;
            float d = EasingManager.EaseProgress(EASETYPE, currentTime, time, 3f, 0);

            pos.Set(WIDTH_MAX * (1 - d), rect.sizeDelta.y);
            rect.sizeDelta = pos;
        }
    }

    private void ToOption()
    {
        Logger.Log("Option�ɑJ�ڂ����");
    }
    private void ToTutorial()
    {
        Logger.Log("�`���[�g���A���ɑJ�ڂ����");
    }
    private void ToEx()
    {
        Logger.Log("������[�ɑJ�ڂ����");
    }
}