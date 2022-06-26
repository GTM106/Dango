using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �Q�[���̐i�s�Ɋւ����̂̂ݒ�`
/// </summary>
internal class GameManager : MonoBehaviour
{
    public static float GameScore { get; set; } = 0;

    //static private int PlayerNum = 1;
    //static public Player1[] player { get; set; } = new Player1[2];

    QuestManager _questManager = new();

    private void Awake()
    {
        //�}�E�X�J�[�\���̂�B
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        //�ŏ��̃N�G�X�g�����u���B
        _questManager.ChangeQuest(_questManager.Creater.CreateQuestCreateRole(DangoRole.POSROLE_DIVIDED_INTO_TWO, 1, "���u�񕪊��v��1���I"),
                               _questManager.Creater.CreateQuestIncludeColor(DangoColor.Red, 3, "�ԐF���܂߂Ė���3���I"));
    }

    //�����v���C���[�̏����i���p���ꂽ���́j
    //public static void SetPlayer(Player1 obj)
    //{

    //    for (int i = 0; i < player.Length; i++)
    //    {
    //        if (player[i] == null)
    //        {
    //            player[i] = obj;
    //            break;
    //        }
    //    }
    //}

    //public void AddPlayer(GameObject obj)
    //{
    //    //Camera cam = obj.GetComponentInChildren<Camera>();
    //    //if (PlayerNum == 1)
    //    //{
    //    //    cam.rect = new Rect(0, 0, 1f, 0.5f);
    //    //    Logger.Log("cam.rect��ύX");

    //    //}
    //    //else if (PlayerNum == 2)
    //    //{
    //    //    cam.rect = new Rect(0, 0.5f, 1f, 0.5f);
    //    //    Logger.Log("cam.rect��ύX");
    //    //    return;
    //    //}
    //    //    PlayerNum++;
    //}
}
