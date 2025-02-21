using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Stage
{
    Stage1,
    Stage2,
    Stage3,

    Tutorial,
}

public enum StageStatus
{
    Lock,
    StandbyForDirection,
    Unlock,
}

public class StageData : MonoBehaviour
{
    [SerializeField] Stage _stage;
    [SerializeField] PortraitScript _portraitScript;
    [SerializeField] TutorialPortraitManager _tutorialPortraitScript;
    [SerializeField] FusumaManager _fusumaManager;
    [SerializeField, Tooltip("制限時間"), Min(0)] float _timeLimit;
    [SerializeField, Tooltip("開始時D5"), Range(3, 7)] int _startD5;

    public bool IsRelease => DataManager.saveData.stagesStatus[(int)_stage] == (int)StageStatus.Unlock;

    public Stage Stage => _stage;

    public List<QuestData> QuestData = new();
    public float TimeLimit => _timeLimit;
    public int StartD5 => _startD5;

    protected virtual void Awake()
    {
        InputSystemManager.Instance.Input.SwitchCurrentActionMap("Player");

        if (_fusumaManager != null) _fusumaManager.Open();
    }

    protected virtual void Start()
    {
        IChangePortrait portrait = _portraitScript;
        if (portrait == null) portrait = _tutorialPortraitScript;

        portrait.ChangePortraitText(StartPortraitText()).Forget();
        AddQuest();
    }

    protected virtual void AddQuest()
    {
        throw new System.NullReferenceException();
    }

    protected virtual PortraitTextData StartPortraitText()
    {
        return null;
    }

    public virtual List<DangoColor> FloorDangoColors()
    {
        return null;
    }

    public virtual void OnStageSucceed()
    {
        throw new System.NullReferenceException();
    }

    protected void Release(Stage stage)
    {
        //既にリリースされているステージには行わない
        if (DataManager.saveData.stagesStatus[(int)stage] != (int)StageStatus.Lock) return;

        DataManager.saveData.stagesStatus[(int)stage] = (int)StageStatus.StandbyForDirection;
    }

    protected void Lock(Stage stage)
    {
        DataManager.saveData.stagesStatus[(int)stage] = (int)StageStatus.Lock;
    }
}
