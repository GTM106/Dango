using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dango.Quest;

class QuestManager
{
    //�ÓI�ȃN�G�X�g�ꗗ
    static List<QuestData> _quests = new();

    public QuestManager()
    {
        SucceedChecker = new(this);
    }

    //�N�G�X�g�̐����E�N���A����̂��
    public QuestCreater Creater { get; private set; } = new();
    public QuestSucceedChecker SucceedChecker { get; private set; }

    public void ChangeQuest(params QuestData[] items)
    {
        _quests.Clear();

        _quests.AddRange(items);
    }
    public void ChangeQuest(List<QuestData>items)
    {
        _quests.Clear();

        _quests.AddRange(items);
    }

    public QuestData GetQuest(int index)
    {
        if (index < 0 || index >= _quests.Count) return null;

        return _quests[index];
    }

    public int QuestsCount => _quests.Count;
}

