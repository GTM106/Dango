using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dango.Quest;

class QuestManager
{
    //�ÓI�ȃN�G�X�g�ꗗ
    static List<QuestData> _quests = new();

    //�N�G�X�g�̐����E�N���A����̂��
    public QuestCreater Creater { get; private set; } = new();
    public QuestSucceedChecker SucceedChecker { get; private set; } = new();

    public void ChangeQuest(params QuestData[] items)
    {
        _quests.Clear();
        
        foreach (QuestData item in items)
        {
            _quests.Add(item);
        }
    }

    public QuestData GetQuest(int index)
    {
        if (index < 0 || index >= _quests.Count) return null;

        return _quests[index];
    }

    public int QuestsCount => _quests.Count;
}

