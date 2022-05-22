using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Role<T>
{
    private T[] m_role;
    private string m_rolename;
    private float m_score;
    private int m_madeCount = 0;

    /// <param name="t">���̔z��</param>
    /// <param name="n">��</param>
    /// <param name="s">�X�R�A</param>
    public Role(T[] t, string n, float s)
    {
        m_role = t;
        m_rolename = n;
        m_score = s;
    }

    public T[] GetData() => m_role;
    public string GetRolename() => m_rolename;
    public float GetScore() => m_score;
    public int GetMadeCount() => m_madeCount;
    public void AddMadeCount() => m_madeCount++;
}

public class DangoRole
{
    static List<DangoColor> color = new();

    private static List<Role<DangoColor>> specialRoles = new()
    {
        new Role<DangoColor>(new DangoColor[]{DangoColor.Red,DangoColor.Yellow,DangoColor.Blue},"temp",30),

    };

    private static List<Role<DangoColor>> colorRoles = new()
    {
        new Role<DangoColor>(new DangoColor[]{DangoColor.Red,DangoColor.Orange},"temp",3),

    };

    private static List<Role<int>> posRoles = new()
    {
        new Role<int>(new int[]{0,0,0},"0,0,0",3),
        new Role<int>(new int[]{0,0,0,0},"0,0,0,0",4),
        new Role<int>(new int[]{0,0,1,1},"0,0,1,1",4),
        new Role<int>(new int[]{0,1,0,1},"0,1,0,1",4),
        new Role<int>(new int[]{0,0,0,0,0},"0,0,0,0,0",5),
        new Role<int>(new int[]{0,1,0,1,0},"0,1,0,1,0",5),
        new Role<int>(new int[]{0,0,1,0,0},"0,0,1,0,0",5),
        new Role<int>(new int[]{0,1,2,3,2,1,0},"0,1,2,3,2,1,0",7),
        new Role<int>(new int[]{0,1,0,1,0,1,0},"0,1,0,1,0,1,0",7),
        new Role<int>(new int[]{0,0,0,0,0,0,0},"0,0,0,0,0,0,0",7),
        new Role<int>(new int[]{0,1,2,3,4,5,6},"0,1,2,3,4,5,6",7),
    };

    /// <summary>
    /// �H�ׂ��c�q�ɖ������邩���肵�ē_����Ԃ��֐�
    /// </summary>
    /// <param name="dangos">�H�ׂ��c�q</param>
    /// <returns>float:�_��</returns>
    public static float CheckRole(List<DangoColor> dangos)
    {

        GameManager.NowRoleList = "";

        //�Ԃ�l�̓��_
        float score = 0;

        //������̔���Atrue�Ȃ炱���Ŕ���I���B
        if (CheckSpecialRole(dangos, ref score)) return score;

        //��������Ȃ�������E�E�E
        foreach (DangoColor c in dangos)
        {
            //�d����h���ŁE�E�E
            if (!color.Contains(c))
            {
                //�����J���[�̃��X�g�ɒǉ�
                color.Add(c);
            }
        }

        //���̑����̔���
        CheckPosRole(dangos, ref score);
        CheckColorRole(ref score);//���������Ƀ\�[�g���܂ނ��߁A�ʒu����艺�ɔz�u�B
        
        //�S�̓I�ȓ_���v�Z�i���̏����͖��̗L���Ɋւ�炸���s�����j
        score += (8 - color.Count) * dangos.Count();
        
        return score;
    }

    /// <summary>
    /// ������̔���
    /// </summary>
    /// <param name="value">�_���̏o��</param>
    /// <returns>
    /// <para>true : ����</para>
    /// <para>false : �Ȃ�</para>
    /// </returns>
    private static bool CheckSpecialRole(List<DangoColor> dangos, ref float score)
    {
        foreach (var specialRole in specialRoles)
        {
            //�z������X�g�ɕϊ�
            List<DangoColor> specialRoleList = specialRole.GetData().ToList();

            //�F�ƈʒu�����[���ƈ�v���Ă�����
            if (dangos.SequenceEqual(specialRoleList))
            {

                //�\��
                GameManager.NowRoleList = "�u" + specialRole.GetRolename() + "�v�I" + specialRole.GetScore() + "�_�I";

                //������񐔂𑝂₵
                specialRole.AddMadeCount();

                //�X�R�A�����Z��������
                score += specialRole.GetScore();
                return true;
            }
        }

        //���������Ȃ�������false��Ԃ��A������
        return false;
    }

    /// <summary>
    /// �F���̔���
    /// </summary>
    /// <returns>
    /// <para>true : ����</para>
    /// <para>false : �Ȃ�</para>
    /// </returns>
    private static bool CheckColorRole(ref float score)
    {
        //�����\�[�g
        color.Sort();

        foreach (var colorRole in colorRoles)
        {
            //�z������X�g�ɕϊ�
            List<DangoColor> colorRoleList = colorRole.GetData().ToList();

            //�O�̂��߂�������\�[�g
            colorRoleList.Sort();

            //�F�����[���ƈ�v���Ă�����
            if (color.SequenceEqual(colorRoleList))
            {

                //�\��
                GameManager.NowRoleList = GameManager.NowRoleList + "�u" + colorRole.GetRolename() + "�v�I" + colorRole.GetScore() + "�_�I";

                //������񐔂𑝂₵
                colorRole.AddMadeCount();

                //�X�R�A�����Z��������
                score += colorRole.GetScore();
                return true;
            }
        }

        //���������Ȃ�������false��Ԃ��A������
        return false;
    }

    /// <summary>
    /// �ʒu���̔���
    /// </summary>
    /// <returns>
    /// <para>true : ����</para>
    /// <para>false : �Ȃ�</para>
    /// </returns>
    private static bool CheckPosRole(List<DangoColor> dangos, ref float score)
    {
        //�F�ɉ������C���f�b�N�X������U�����z����쐬
        var normalizeDangoList = new List<int>();

        //�c�q�̐F�f�[�^�𐳋K������
        foreach (DangoColor d in dangos)
        {
            normalizeDangoList.Add(color.IndexOf(d));
        }

        foreach (var posRole in posRoles)
        {
            //�z������X�g�ɕϊ�
            List<int> posRoleList = posRole.GetData().ToList();

            //�z�u�����[���ƈ�v���Ă�����
            if (normalizeDangoList.SequenceEqual(posRoleList))
            {

                //�\��
                GameManager.NowRoleList = GameManager.NowRoleList + "�u" + posRole.GetRolename() + "�v�I" + posRole.GetScore() + "�_�I";

                //������񐔂𑝂₵�E�E�E
                posRole.AddMadeCount();

                //����ɃX�R�A�����Z��������
                score += posRole.GetScore();

                //[Debug]�𖼂̕\��
                Logger.Log(posRole.GetRolename());
                return true;
            }
        }

        //���������Ȃ�������false��Ԃ��A������
        return false;
    }

    public static List<Role<int>> GetPosRoles() => posRoles;
}
