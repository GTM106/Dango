using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RoleDirectingScript : MonoBehaviour
{
    // Start is called before the first frame update
    //private TextMeshProUGUI _role;
    public static RoleDirectingScript instance = new RoleDirectingScript();
    void Start()
    {
        //if (_role == null)
        //{
        //    _role = GameObject.Find("Canvas").transform.Find("Role").GetComponent<TextMeshProUGUI>();
        //}
    }

    // Update is called once per frame
    void Update()
    {
        //_role.text = GameManager.NowRoleList;
    }

    public void PosDirecting(Role<int> dangos)
    {
        //�ʒu���̉��o
        switch (dangos.GetRolename())
        {
            case "�P�F��":
                Logger.Log("�P�F��");
                break;
            case "���Ώ�":
                Logger.Log("���Ώ�");
                break;
            case "���[�v":
                Logger.Log("���[�v");
                break;
            case "�񕪊�":
                Logger.Log("�񕪊�");
                break;
            default:
                break;
        }
    }

    //�F�̉��o
    public void ColorDirecting(List<DangoColor> color)
    {
        for (int i = color.Count-1; i > -1; i--)
        {
            switch (color[i])
            {
                case DangoColor.Red:
                    Logger.Log("��");
                    break;
                case DangoColor.Orange:
                    Logger.Log("��");
                    break;
                case DangoColor.Yellow:
                    Logger.Log("���F");
                    break;
                case DangoColor.Green:
                    Logger.Log("��");
                    break;
                case DangoColor.Blue:
                    Logger.Log("��");
                    break;
                case DangoColor.Purple:
                    Logger.Log("��");
                    break;
                case DangoColor.Cyan:
                    Logger.Log("���F");
                    break;
            }
        }
    }

    //�X�y�V�������̉��o
    private void SpecialDirecting(Role<DangoColor> dangos)
    {
        //�X�y�V���������ǂ̊�Ŕ��f���邩�킩��Ȃ����牼��Rolename�Ŏ���Ă܂�
        switch (dangos.GetRolename())
        {
            case "temp":
                break;
            case "test2":
                break;
            case "test3":
                break;
        }
    }
}
