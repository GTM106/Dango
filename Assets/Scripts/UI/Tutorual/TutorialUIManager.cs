using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using TM.Easing.Management;
using TM.Input.KeyConfig;
using UnityEngine;
using UnityEngine.UI;
using static TM.Input.KeyConfig.KeyData;

public class TutorialUIManager : MonoBehaviour
{
    [SerializeField] U7 _u7;
    [SerializeField] U8 _u8;

    [SerializeField] ImageUIData _u8Image;

    [SerializeField] QuestManager _questManager;

    [SerializeField] IngameUIManager _ingameUIManager;
    [SerializeField] TextUIData _firstText;

    const float FLASHTIME = 0.7f;

    private void OnEnable()
    {
        _u8Image.ImageData.FlashAlpha(-1, FLASHTIME, 0);
    }

    private void OnDisable()
    {
        _u8Image.ImageData.CancelFlash();
    }

    private void Start()
    {
        _u7.SetText(0);
        StartTextDirecting();
        InputSystemManager.Instance.onTutorialSkipPerformed += CheckSkipQuest;
    }

    private void OnDestroy()
    {
        InputSystemManager.Instance.onTutorialSkipPerformed -= CheckSkipQuest;
    }

    public void ChangeNextGuide(int nextQuestID)
    {
        _u7.SetText(nextQuestID);
    }

    private async void StartTextDirecting()
    {
        _firstText.TextData.SetAlpha(0);
        await _firstText.TextData.Fadein(1f);

        SoundManager.Instance.PlaySE(SoundSource.SE6_CREATE_ROLE_CHARACTER_ANIMATION);

        TextFontSizeAnimation();
    }

    private async void TextFontSizeAnimation()
    {
        float fontSize;
        float time = 0;
        float duration = 1.5f;

        while (time < duration)
        {
            await UniTask.Yield();
            time += Time.deltaTime;
            fontSize = 110f + (30f * EasingManager.EaseProgress(TM.Easing.EaseType.Linear, time, duration, 0, 0));
            _firstText.TextData.SetFontSize(fontSize);
        }
        time = 0;

        _firstText.TextData.Fadeout(0.5f).Forget();

        while (time < duration)
        {
            await UniTask.Yield();
            time += Time.deltaTime;
            fontSize = 140f + (105f * EasingManager.EaseProgress(TM.Easing.EaseType.OutBack, time, duration, 0, 0));
            _firstText.TextData.SetFontSize(fontSize);
        }

        _firstText.TextData.SetFontSize(250);
    }

    private async void CheckSkipQuest()
    {
        if (_ingameUIManager.DuringEndProduction) return;

        _u8.SetCanvasEnable(true);

        InputSystemManager.Instance.Input.SwitchCurrentActionMap("UI");

        bool skip = false;
        _u8.SetImageColor(skip);

        while (!InputSystemManager.Instance.IsPressChoice)
        {
            await UniTask.Yield();

            if (InputSystemManager.Instance.IsPressBack)
            {
                skip = false;
                break;
            }
            if (InputSystemManager.Instance.NavigateAxis.magnitude <= 0.5f) continue;

            skip = InputSystemManager.Instance.NavigateAxis.x < 0;
            _u8.SetImageColor(skip);
        }

        InputSystemManager.Instance.Input.SwitchCurrentActionMap("Player");
        _u8.SetCanvasEnable(false);

        if (!skip) return;

        ChangeNextGuide(_questManager.GetQuest(0).NextQuestId[0]);
        _questManager.SucceedChecker.QuestSkip();
    }

    [Serializable]
    struct U7
    {
        [SerializeField] ImageUIData _operationGuideImage;
        [SerializeField] List<KeyData.GameAction> _actions;
        [SerializeField] List<Image> _keyImages;
        [SerializeField] List<Sprite> _actionSprite;
        [SerializeField] List<Sprite> _keySprite;

        public void SetText(int nextQuestID)
        {
            if (nextQuestID < 0 || nextQuestID >= _actions.Count) return;

            var keySprites = KeyCode(nextQuestID);

            for (int i = 0; i < _keyImages.Count; i++)
            {
                _keyImages[i].enabled = false;
            }

            for (int i = 0; i < keySprites.Count; i++)
            {
                _keyImages[i].enabled = true;
                _keyImages[i].sprite = keySprites[i];
            }

            _operationGuideImage.ImageData.SetSprite(ActionText(nextQuestID));
        }

        private Sprite ActionText(int nextQuestID)
        {
            return _actions[nextQuestID] switch
            {
                GameAction.Move => _actionSprite[0],
                GameAction.LookRotation => _actionSprite[1],
                GameAction.Jump => _actionSprite[2],
                GameAction.Attack => _actionSprite[3],
                GameAction.Eat => _actionSprite[4],
                GameAction.Remove => _actionSprite[5],
                GameAction.Pause => _actionSprite[6],
                GameAction.UIExpansion => _actionSprite[7],
                _ => throw new NotImplementedException(),
            };
        }

        private List<Sprite> KeyCode(int nextQuestID)
        {
            List<Sprite> sprites = new();

            if (_actions[nextQuestID] == GameAction.Move)
            {
                sprites.Add(_keySprite[8]);
            }
            else if (_actions[nextQuestID] == GameAction.LookRotation)
            {
                sprites.Add(_keySprite[9]);
            }
            else
            {
                for (int i = 0; i < DataManager.keyConfigData.keys.Length; i++)
                {
                    if (DataManager.keyConfigData.keys[i] == (int)_actions[nextQuestID])
                    {
                        Sprite sprite = (GamepadKey)i switch
                        {
                            GamepadKey.ButtonNorth => _keySprite[0],
                            GamepadKey.ButtonEast => _keySprite[1],
                            GamepadKey.ButtonWest => _keySprite[2],
                            GamepadKey.ButtonSouth => _keySprite[3],
                            GamepadKey.L => _keySprite[4],
                            GamepadKey.R => _keySprite[5],
                            GamepadKey.LTrigger => _keySprite[6],
                            GamepadKey.RTrigger => _keySprite[7],
                            GamepadKey.D_padDown => _keySprite[10],
                            GamepadKey.D_padUp => _keySprite[11],
                            GamepadKey.D_padLeft => _keySprite[12],
                            GamepadKey.D_padRight => _keySprite[13],
                            _ => throw new NotImplementedException(),
                        };

                        sprites.Add(sprite);
                    }
                }
            }

            return sprites;
        }
    }

    [Serializable]
    struct U8
    {
        [SerializeField] Canvas U8Canvas;
        [SerializeField] ImageUIData ok;
        [SerializeField] ImageUIData ng;

        public void SetImageColor(bool isOk)
        {
            ok.ImageData.SetColor(isOk ? Color.red : Color.white);
            ng.ImageData.SetColor(isOk ? Color.white : Color.red);
        }

        public void SetCanvasEnable(bool enabled)
        {
            U8Canvas.enabled = enabled;
        }
    }
}
