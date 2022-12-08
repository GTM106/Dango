using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Stage
{
    Stage1,
    Stage2,
    Stage3,
    Stage4,

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
    [SerializeField] FusumaManager _fusumaManager;

    public bool IsRelease => DataManager.saveData.stagesStatus[(int)_stage] == (int)StageStatus.Unlock;

    public Stage Stage => _stage;

    public List<QuestData> QuestData = new();

    protected virtual void Awake()
    {
        InputSystemManager.Instance.Input.SwitchCurrentActionMap("Player");

        if (_fusumaManager != null) _fusumaManager.Open();
    }

    protected virtual void Start()
    {
        _portraitScript.ChangePortraitText(StartPortraitText()).Forget();
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
        DataManager.saveData.stagesStatus[(int)stage] = (int)StageStatus.StandbyForDirection;
    }

    protected void Lock(Stage stage)
    {
        DataManager.saveData.stagesStatus[(int)stage] = (int)StageStatus.Lock;
    }
}
