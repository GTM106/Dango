using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Dango.Quest.UI
{
    class QuestUIManager : MonoBehaviour
    {
        [SerializeField] Canvas _canvas;

        [SerializeField] QuestIconScript _icon;
        [SerializeField] Sprite[] sprites;

        public void ChangeIcon(QuestData[] questDatas)
        {
            //TODO:�N�G�X�g�̎�ނɉ����ĉ摜�������ւ�
            //if (questData is QuestEatDango questEa)
            _icon.ChangeIcon(questDatas[0]);
        }
    }
}
