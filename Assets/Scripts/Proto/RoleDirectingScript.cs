using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RoleDirectingScript : MonoBehaviour
{
    // Start is called before the first frame update
    private TextMeshProUGUI _role;
    //public static RoleDirectingScript instance = new RoleDirectingScript();
    [SerializeField] GameObject[] imageobj;
    private Image[] images;
    void Start()
    {
        if (_role == null)
        {
            _role = GameObject.Find("Canvas").transform.Find("Role").GetComponent<TextMeshProUGUI>();
        }
        images = new Image[imageobj.Length];
        for (int i = 0; i < imageobj.Length; i++)
        {
            images[i] = imageobj[i].GetComponent<Image>();
            imageobj[i].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        _role.text = GameManager.NowRoleList;
    }

    public void Dirrecting(List<DangoColor> dangos)
    {
        float i = 0;
        //�F�͑����m��
        ColorDirecting(dangos);

        //�������݂��邩�ǂ��������m�肽������X�R�A�ɂ͓K���Ȋ֐�����Ă܂�
        if (DangoRole.instance.CheckPosRole(dangos, ref i))
        {
            PosRoleDirecting();
        }
        if (DangoRole.instance.CheckSpecialRole(dangos, ref i))
        {
            SpecialRoleDirecting();
        }
        if (DangoRole.instance.CheckColorRole(ref i))
        {
            ColorRoleDirecting();
        }
    }

    private void PosRoleDirecting()
    {
        //�ʒu���̉��o
        switch (GameManager.NowRoleList)
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
    private void ColorDirecting(List<DangoColor> color)
    {
        for (int i = 6; i > -1; i--)
        {
            if (color.Count > i) {
                imageobj[i].SetActive(true);
                switch (color[i])
                {
                    case DangoColor.Red:
                        Logger.Log("��");
                        images[i].color = Color.red;
                        break;
                    case DangoColor.Orange:
                        Logger.Log("��");
                        images[i].color = new Color32(255, 155, 0, 255);
                        break;
                    case DangoColor.Yellow:
                        Logger.Log("���F");
                        images[i].color = Color.yellow;
                        break;
                    case DangoColor.Green:
                        Logger.Log("��");
                        images[i].color = Color.green;
                        break;
                    case DangoColor.Blue:
                        Logger.Log("��");
                        images[i].color = Color.blue;
                        break;
                    case DangoColor.Purple:
                        Logger.Log("��");
                        images[i].color = new Color32(200, 0, 255, 255);
                        break;
                    case DangoColor.Cyan:
                        Logger.Log("���F");
                        images[i].color = Color.cyan;
                        break;
                }
            }
            else
            {
                imageobj[i].SetActive(false);
            }
        }
    }
    private void ColorRoleDirecting()
    {
        switch (GameManager.NowRoleList)
        {
            case "temp":
                break;
            case "test2":
                break;
            case "test3":
                break;
        }
    }

    //�X�y�V�������̉��o
    private void SpecialRoleDirecting()
    {
        switch (GameManager.NowRoleList)
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
