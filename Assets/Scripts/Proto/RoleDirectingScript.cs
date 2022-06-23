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
    [SerializeField] GameObject[] imageobj;
    private Image[] images;
    void Start()
    {
        //if (_role == null)
        //{
        //    _role = GameObject.Find("Canvas").transform.Find("Role").GetComponent<TextMeshProUGUI>();
        //}
        images = new Image[imageobj.Length];
        for (int i = 0; i < imageobj.Length; i++)
        {
            images[i] = imageobj[i].GetComponent<Image>();
            imageobj[i].SetActive(false);
        }
        instance.imageobj = imageobj;
        instance.images = images;
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
        for (int i = 6; i > -1; i--)
        {
            if (color.Count > i) {
                instance.imageobj[i].SetActive(true);
                switch (color[i])
                {
                    case DangoColor.Red:
                        Logger.Log("��");
                        instance.images[i].color = Color.red;
                        break;
                    case DangoColor.Orange:
                        Logger.Log("��");
                        instance.images[i].color = new Color32(255, 155, 0, 255);
                        break;
                    case DangoColor.Yellow:
                        Logger.Log("���F");
                        instance.images[i].color = Color.yellow;
                        break;
                    case DangoColor.Green:
                        Logger.Log("��");
                        instance.images[i].color = Color.green;
                        break;
                    case DangoColor.Blue:
                        Logger.Log("��");
                        instance.images[i].color = Color.blue;
                        break;
                    case DangoColor.Purple:
                        Logger.Log("��");
                        instance.images[i].color = new Color32(200, 0, 255, 255);
                        break;
                    case DangoColor.Cyan:
                        Logger.Log("���F");
                        instance.images[i].color = Color.cyan;
                        break;
                }
            }
            else
            {
                    Logger.Log("�Ȃ�");
                instance.imageobj[i].SetActive(false);
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
