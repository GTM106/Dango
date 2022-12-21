using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ClearSceneScript : MonoBehaviour
{
    private enum Next
    {
        Retry,
        StageSelect,

        Max,
    }

    [SerializeField] GameObject[] Ranks;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] GameObject QuestBlokObj;
    [SerializeField] GameObject QuestFlowChart;
    [SerializeField] FusumaManager _fusumaManager;
    [SerializeField] Canvas _canvas;
    [SerializeField] flowchartScript _flowchartScript;
    [SerializeField] U7And8 _U7And8;

    Next _next;

    List<GameObject> objs = new();
    static readonly bool _canMoveTopToBottom = true;

    private void Awake()
    {
        //演出終了まで描画しない
        _canvas.enabled = false;

        //初期設定を行う
        SetRetryOrSelectColor();
        SetQusetFlowchart();
        CreateBloks();
    }

    async void Start()
    {
        //ふすまの演出
        await _fusumaManager.UniTaskClose();

        //演出終了後に描画
        _canvas.enabled = true;

        //裏で生きていたインゲームを削除
        SceneSystem.Instance.UnLoad(SceneSystem.Instance.CurrentIngameScene, true);

        //ほぼ存在しているがこのシーンを最初に持ってきた場合Instanceがnullの場合があるためStartで行います
        InputSystemManager.Instance.Input.SwitchCurrentActionMap("UI");
        InputSystemManager.Instance.onNavigatePerformed += OnNavigate;
        InputSystemManager.Instance.onChoicePerformed += OnChoice;
    }

    private void OnDestroy()
    {
        InputSystemManager.Instance.onNavigatePerformed -= OnNavigate;
        InputSystemManager.Instance.onChoicePerformed -= OnChoice;
    }

    private void CreateBloks()
    {
        List<QuestData> dangoDatas = ScoreManager.Instance.GetClearQuest();
        List<float> dangoClearTime = ScoreManager.Instance.GetClearTime();

        for (int i = 0; i < dangoDatas.Count; i++)
        {
            GameObject obj = Instantiate(QuestBlokObj, QuestFlowChart.transform.position, Quaternion.identity, QuestFlowChart.transform);
            obj.GetComponent<ClearQuestBlokScript>().SetText(dangoDatas[i], dangoClearTime[i]);
            objs.Add(obj);
            if (i != 0)
                _flowchartScript.posSet(objs[i - 1].transform.localPosition);
        }
        ScoreManager.Instance.ResetClearTime();
        ScoreManager.Instance.ResetClearQuest();
    }

    private void OnNavigate()
    {
        Vector2 axis = InputSystemManager.Instance.NavigateAxis;

        if (!ChangeChoiceUtil.Choice(axis, ref _next, Next.Max, _canMoveTopToBottom, ChangeChoiceUtil.OptionDirection.Horizontal)) return;

        SetRetryOrSelectColor();
    }

    private void OnChoice()
    {
        switch (_next)
        {
            case Next.Retry:
                SceneSystem.Instance.Load(SceneSystem.Instance.CurrentIngameScene);
                SceneSystem.Instance.UnLoad(SceneSystem.Scenes.Success, true);
                break;
            case Next.StageSelect:
                SceneSystem.Instance.Load(SceneSystem.Scenes.StageSelect);
                SceneSystem.Instance.UnLoad(SceneSystem.Scenes.Success, true);
                break;
        }
    }

    private void SetRetryOrSelectColor()
    {
        bool isU7 = _next == Next.Retry;

        _U7And8._u7Image.ImageData.SetSprite(_U7And8._u7Sprites[isU7 ? 0 : 1]);
        _U7And8._u8Image.ImageData.SetSprite(_U7And8._u8Sprites[isU7 ? 1 : 0]);
    }

    private void SetObj(GameObject obj)
    {
        obj.SetActive(true);
    }

    private void SetQusetFlowchart()
    {
        float time = ScoreManager.Instance.GetTime();

        if (time < 180)
            SetObj(Ranks[0]);
        else if (time < 240)
            SetObj(Ranks[1]);
        else
            SetObj(Ranks[2]);

        text.text = time.ToString("f1");
    }

    [Serializable]
    struct U7And8
    {
        public ImageUIData _u7Image;
        public ImageUIData _u8Image;

        public List<Sprite> _u7Sprites;
        public List<Sprite> _u8Sprites;

        public U7And8(ImageUIData u7Image, ImageUIData u8Image, List<Sprite> u7Sprites, List<Sprite> u8Sprites) : this()
        {
            _u7Image = u7Image;
            _u8Image = u8Image;
            _u7Sprites = u7Sprites;
            _u8Sprites = u8Sprites;
        }
    }
}
