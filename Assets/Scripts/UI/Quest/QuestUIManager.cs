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

        QuestManager _questManager = new();

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
                //_questManager.ChangeQuest(_questManager.Creater.CreateQuestCreateRole(DangoRole.POSROLE_LOOP, 2, "���u���[�v�v��2���I"),
                //                          _questManager.Creater.CreateQuestIncludeColor(DangoColor.Blue, 2, "�F���܂߂Ė���2���I"));

                QuestUI.Instance.OnGUIChangeQuest(images, quests);

                _coolTime = COOLTIME;
                _isWaitingCoolTime = false;
            }
        }
    }
}
