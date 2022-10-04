using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Dango.Quest.UI
{
    class QuestUIManager : MonoBehaviour
    {
        //�N�G�X�g�X�V�܂ł̑ҋ@����
        const int COOLTIME = 150;
        int _coolTime = COOLTIME;

        [SerializeField, Tooltip("�^�C��")] Image[] images;
        [SerializeField, Tooltip("�N�G�X�g�e�L�X�g")] TextMeshProUGUI[] quests;

        //�ҋ@������
        bool _isWaitingCoolTime = false;

        private void Start()
        {
            QuestUI.Instance.OnGUIChangeQuest(images, quests);
        }

        private void FixedUpdate()
        {
            if (QuestUI.Instance.OnGUIQuestUpdate(quests)) _isWaitingCoolTime = true;

            NextQuest();
        }

        private void NextQuest()
        {
            //���̃N�G�X�g�\���̑ҋ@���łȂ���Ύ��s���Ȃ�
            if (!_isWaitingCoolTime) return;

            //�ҋ@�I������������B���Ă��Ȃ���Αҋ@���Ԃ��ւ炷
            if (!QuestUI.Instance.IsWaiting(--_coolTime))
            {
                QuestUI.Instance.OnGUIChangeQuest(images, quests);

                _coolTime = COOLTIME;
                _isWaitingCoolTime = false;
            }
        }
    }
}
