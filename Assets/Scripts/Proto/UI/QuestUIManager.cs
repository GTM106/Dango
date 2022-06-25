using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestUIManager : MonoBehaviour
{
    //�N�G�X�g�X�V�܂ł̑ҋ@����
    const int COOLTIME = 150;
    int _coolTime = COOLTIME;

    [SerializeField,Tooltip("�^�C��")] Image[] images;
    [SerializeField,Tooltip("�N�G�X�g�e�L�X�g")] TextMeshProUGUI[] quests;

    //�N�G�X�gUI�̃C���X�^���X
    QuestUI questUI = new();

    QuestManager _questManager = new();

    //�ҋ@������
    bool _isWaitingCoolTime = false;

    private void Start()
    {
        questUI.OnGUIChangeQuest(images, quests);
    }

    private void FixedUpdate()
    {
        if (questUI.OnGUIQuestUpdate(quests)) _isWaitingCoolTime = true;

        NextQuest();
    }

    private void NextQuest()
    {
        //���̃N�G�X�g�\���̑ҋ@���łȂ���Ύ��s���Ȃ�
        if (!_isWaitingCoolTime) return;

        //�ҋ@�I������������B���Ă��Ȃ���Αҋ@���Ԃ��ւ炷
        if (!questUI.IsWaiting(--_coolTime))
        {
            //�N�G�X�g���X�V���A�ݒ��߂��B
            GameManager.Quests.Clear();
            GameManager.Quests.Add(_questManager.CreateQuestCreateRole(DangoRole.POSROLE_LOOP, 2, "���u���[�v�v��2���"));
            GameManager.Quests.Add(_questManager.CreateQuestIncludeColor(DangoColor.Blue, 2, "�F���܂߂Ė���2���I"));

            questUI.OnGUIChangeQuest(images, quests);
            _coolTime = COOLTIME;
            _isWaitingCoolTime = false;
        }
    }

}
