using Dango.Quest;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using static DangoRole;

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

class DangoRole
{
    public enum POSROLE
    {
        MONOCOLOR,
        LINE_SYMMETRY,
        LOOP,
        DIVIDED_INTO_TWO,
        DIVIDED_INTO_THREE,
    }

    //�ÓI�Ȗ�
    //���j���̏������C���X�^���X�����ȉ��ɏ����Ǝ��s���I�ɖ𖼂�null�ɂȂ�܂��B
    public static readonly string POSROLE_MONOCOLOR = "�P�F��";
    public static readonly string POSROLE_LINE_SYMMETRY = "���Ώ�";
    public static readonly string POSROLE_LOOP = "���[�v";
    public static readonly string POSROLE_DIVIDED_INTO_TWO = "�񕪊�";
    public static readonly string POSROLE_DIVIDED_INTO_THREE = "�O����";

    //�C���X�^���X����
    //������������ƁA�X�^�b�N�I�[�o�[�t���[���N���������߃V���O���g���p�^�[���ōs���܂�
    public static readonly DangoRole instance = new();

    private DangoRole()
    {
        posRoles = new()
    {
        new Role<int>(new int[]{0,0,0},POSROLE_MONOCOLOR,3),
        new Role<int>(new int[]{0,0,0,0},POSROLE_MONOCOLOR,4),
        new Role<int>(new int[]{0,0,0,0,0},POSROLE_MONOCOLOR,5),
        new Role<int>(new int[]{0,0,0,0,0,0},POSROLE_MONOCOLOR,6),
        new Role<int>(new int[]{0,0,0,0,0,0,0},POSROLE_MONOCOLOR,7),
        new Role<int>(new int[]{0,1,0},POSROLE_LINE_SYMMETRY,3),
        new Role<int>(new int[]{0,1,1,0},POSROLE_LINE_SYMMETRY,4),
        new Role<int>(new int[]{0,0,1,0,0},POSROLE_LINE_SYMMETRY,5),
        new Role<int>(new int[]{0,1,0,1,0},POSROLE_LINE_SYMMETRY,5),
        new Role<int>(new int[]{0,1,1,1,0},POSROLE_LINE_SYMMETRY,5),
        new Role<int>(new int[]{0,1,2,1,0},POSROLE_LINE_SYMMETRY,5),
        new Role<int>(new int[]{0,0,1,1,0,0},POSROLE_LINE_SYMMETRY,6),
        new Role<int>(new int[]{0,1,0,0,1,0},POSROLE_LINE_SYMMETRY,6),
        new Role<int>(new int[]{0,1,1,1,1,0},POSROLE_LINE_SYMMETRY,6),
        new Role<int>(new int[]{0,1,2,2,1,0},POSROLE_LINE_SYMMETRY,6),
        new Role<int>(new int[]{0,0,0,1,0,0,0},POSROLE_LINE_SYMMETRY,7),
        new Role<int>(new int[]{0,0,1,0,1,0,0},POSROLE_LINE_SYMMETRY,7),
        new Role<int>(new int[]{0,0,1,1,1,0,0},POSROLE_LINE_SYMMETRY,7),
        new Role<int>(new int[]{0,0,1,2,1,0,0},POSROLE_LINE_SYMMETRY,7),
        new Role<int>(new int[]{0,1,0,0,0,1,0},POSROLE_LINE_SYMMETRY,7),
        new Role<int>(new int[]{0,1,0,1,0,1,0},POSROLE_LINE_SYMMETRY,7),
        new Role<int>(new int[]{0,1,0,2,0,1,0},POSROLE_LINE_SYMMETRY,7),
        new Role<int>(new int[]{0,1,1,0,1,1,0},POSROLE_LINE_SYMMETRY,7),
        new Role<int>(new int[]{0,1,1,1,1,1,0},POSROLE_LINE_SYMMETRY,7),
        new Role<int>(new int[]{0,1,1,2,1,1,0},POSROLE_LINE_SYMMETRY,7),
        new Role<int>(new int[]{0,1,2,0,2,1,0},POSROLE_LINE_SYMMETRY,7),
        new Role<int>(new int[]{0,1,2,1,2,1,0},POSROLE_LINE_SYMMETRY,7),
        new Role<int>(new int[]{0,1,2,2,2,1,0},POSROLE_LINE_SYMMETRY,7),
        new Role<int>(new int[]{0,1,2,3,2,1,0},POSROLE_LINE_SYMMETRY,7),
        new Role<int>(new int[]{0,1,0,1},POSROLE_LOOP,4),
        new Role<int>(new int[]{0,0,1,0,0,1},POSROLE_LOOP,6),
        new Role<int>(new int[]{0,1,1,0,1,1},POSROLE_LOOP,6),
        new Role<int>(new int[]{0,1,2,0,1,2},POSROLE_LOOP,6),
        new Role<int>(new int[]{0,1,0,1,0,1},POSROLE_LOOP,6),
        new Role<int>(new int[]{0,0,1,1},POSROLE_DIVIDED_INTO_TWO,4),
        new Role<int>(new int[]{0,0,0,1,1,1},POSROLE_DIVIDED_INTO_TWO,6),
        new Role<int>(new int[]{0,0,1,1,2,2},POSROLE_DIVIDED_INTO_THREE,6),
    };
    }

    List<DangoColor> _color = new();
    Random rand = new();

    private List<Role<DangoColor>> specialRoles = new()
    {
    };

    private List<Role<DangoColor>> colorRoles = new()
    {
    };

    private List<Role<int>> posRoles;

    /// <summary>
    /// �H�ׂ��c�q�ɖ������邩���肵�ē_����Ԃ��֐�
    /// </summary>
    /// <param name="dangos">�H�ׂ��c�q</param>
    /// <returns>float:�_��</returns>
    public float CheckRole(List<DangoColor> dangos, int currentMaxDango)
    {
        //�J���[�̏�����
        _color.Clear();
        DangoRoleUI.OnGUIReset();

        //�Ԃ�l�̓��_
        float score = 0;

        //������̔���Atrue�Ȃ炱���Ŕ���I���B
        //if (CheckSpecialRole(dangos, ref score)) return score;

        //��������Ȃ�������E�E�E
        foreach (DangoColor c in dangos)
        {
            //�d����h���ŁE�E�E
            if (!_color.Contains(c))
            {
                //�����J���[�̃��X�g�ɒǉ�
                _color.Add(c);
            }
        }

        //�������݂��邩�`�F�b�N
        bool enableRole = CheckPosRole(dangos, ref score);

        //EstablishRole�n�N�G�X�g�̃`�F�b�N
        QuestManager.Instance.SucceedChecker.CheckQuestCreateRoleSucceedEs(dangos, enableRole, currentMaxDango);

        //���̑����̔���
        if (enableRole)
        {
            SoundManager.Instance.PlaySE(rand.Next((int)SoundSource.VOISE_PRINCE_CREATEROLE01, (int)SoundSource.VOISE_PRINCE_CREATEROLE02 + 1));

            //IncludeColor�n�N�G�X�g�̃`�F�b�N
            QuestManager.Instance.SucceedChecker.CheckQuestCreateRoleSucceedIr(dangos);

            //��t���Œc�q�����H�ׂ��n�N�G�X�g�E���̑��N�G�X�g�̃`�F�b�N
            QuestManager.Instance.SucceedChecker.CheckQuestEatDangoSucceed(QuestManager.Instance, dangos, true);
        }
        else
        {
            SoundManager.Instance.PlaySE(rand.Next((int)SoundSource.VOISE_PRINCE_NOROLE01, (int)SoundSource.VOISE_PRINCE_NOROLE02 + 1));

            //EatDango�ɕ��ނ����唼�̃N�G�X�g�̃`�F�b�N�B���Ȃ��𐔂��鐔���Ȃ����̂��߂ɏ�L�̃`�F�b�J�[�ƕ����Ă��܂�
            QuestManager.Instance.SucceedChecker.CheckQuestEatDangoSucceed(QuestManager.Instance, dangos, false);
        }

        //�ړI�n�Œc�q��H�ׂ�n�N�G�X�g�̃`�F�b�N
        QuestManager.Instance.SucceedChecker.CheckQuestDestinationSucceed();

        //CheckColorRole(ref score);//���������Ƀ\�[�g���܂ނ��߁A�ʒu����艺�ɔz�u�B

        //�S�̓I�ȓ_���v�Z�i���̏����͖��̗L���Ɋւ�炸���s�����j
        //score += (8 - _color.Count) * dangos.Count();

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
    private bool CheckSpecialRole(List<DangoColor> dangos, ref float score)
    {
        foreach (var specialRole in specialRoles)
        {
            //�z������X�g�ɕϊ�
            List<DangoColor> specialRoleList = specialRole.GetData().ToList();

            //�F�ƈʒu�����[���ƈ�v���Ă�����
            if (dangos.SequenceEqual(specialRoleList))
            {
                // �\��
                DangoRoleUI.OnGUIRoleName(specialRole.GetRolename());

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
    private bool CheckColorRole(ref float score)
    {
        //�����\�[�g
        _color.Sort();

        foreach (var colorRole in colorRoles)
        {
            //�z������X�g�ɕϊ�
            List<DangoColor> colorRoleList = colorRole.GetData().ToList();

            //�O�̂��߂�������\�[�g
            colorRoleList.Sort();

            //�F�����[���ƈ�v���Ă�����
            if (_color.SequenceEqual(colorRoleList))
            {
                // �\��
                DangoRoleUI.OnGUIRoleName(colorRole.GetRolename());

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
    private bool CheckPosRole(List<DangoColor> dangos, ref float score)
    {
        //�F�ɉ������C���f�b�N�X������U�����z����쐬
        var normalizeDangoList = new List<int>();

        //�c�q�̐F�f�[�^�𐳋K������
        foreach (DangoColor d in dangos)
        {
            normalizeDangoList.Add(_color.IndexOf(d));
        }

        foreach (var posRole in posRoles)
        {
            //�z������X�g�ɕϊ�
            List<int> posRoleList = posRole.GetData().ToList();

            //�z�u�����[���ƈ�v���Ă��Ȃ������玟�Ɉڍs
            if (!normalizeDangoList.SequenceEqual(posRoleList)) continue;

            //�\��
            DangoRoleUI.OnGUIRoleName(posRole.GetRolename());

            //������񐔂𑝂₵�E�E�E
            posRole.AddMadeCount();

            //����ɃX�R�A�����Z��������
            score += posRole.GetScore();

            //��A�����n�̃N�G�X�g�̃`�F�b�N
            QuestManager.Instance.SucceedChecker.CheckQuestCreateRoleSucceedSr(posRole);

            //SameRole�n�N�G�X�g�̃`�F�b�N
            QuestManager.Instance.SucceedChecker.CheckQuestCreateRoleSucceedSm(posRole);

            //[Debug]�𖼂̕\��
            //Logger.Log(posRole.GetRolename());
            return true;
        }

        //���������Ȃ�������false��Ԃ��A������
        return false;
    }

    public List<Role<int>> GetPosRoles() => posRoles;
}
