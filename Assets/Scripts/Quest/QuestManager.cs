using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dango.Quest;
using Dango.Quest.UI;

class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; }

    //クエスト一覧
    List<QuestData> _quests = new();

    [SerializeField] PlayerData _playerData;
    [SerializeField] GameObject _expansionUIObj;
    [SerializeField] QuestExpansionUIManager _questExpansionUIManager;
    [SerializeField] GameManager _gameManager;
    [SerializeField] StageData _stageData;
    [SerializeField] PortraitScript _portraitScript;
    [SerializeField] TutorialPortraitManager _tutorialPortraitScript;
    [SerializeField] TutorialUIManager _tutorialUIManager;
    [SerializeField] QuestSucceedUIManager _questSucceedUIManager;
    [SerializeField] TimelimitsManager[] _timelimitsManager;

    private void Awake()
    {
        IChangePortrait portrait = _portraitScript;
        if (portrait == null) portrait = _tutorialPortraitScript;

        Instance = this;
        SucceedChecker = new(this, portrait, _stageData, _tutorialUIManager, _questSucceedUIManager, _questExpansionUIManager, _timelimitsManager);
    }

#if DEBUG
    private void Start()
    {
        InputSystemManager.Instance.onTutorialSkipPerformed += SucceedChecker.QuestSkip;
    }

    private void OnDestroy()
    {
        InputSystemManager.Instance.onTutorialSkipPerformed -= SucceedChecker.QuestSkip;
    }
#endif

    //クエストの生成・クリア判定のやつ
    public QuestCreater Creater { get; private set; } = new();
    public QuestSucceedChecker SucceedChecker { get; private set; }

    public void ChangeQuest(params QuestData[] items)
    {
        _quests.Clear();

        _quests.AddRange(items);
        _questExpansionUIManager.ChangeQuest();
    }
    public void ChangeQuest(List<QuestData> items)
    {
        ChangeQuest(items.ToArray());
    }

    public QuestData GetQuest(int index)
    {
        if (index < 0 || index >= _quests.Count) return null;

        return _quests[index];
    }

    public void CreateExpansionUIObj()
    {
        Instantiate(_expansionUIObj, _playerData.transform.position, Quaternion.identity);
    }

    public int QuestsCount => _quests.Count;
    public PlayerData Player => _playerData;
    public List<QuestData> GetQuests => _quests;

    public void SetIsComplete()
    {
        _gameManager.SetGameSucceed();
    }
}