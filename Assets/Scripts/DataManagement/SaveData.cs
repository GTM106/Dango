/// �Z�[�u�f�[�^�̎��̃N���X
/// float�^��Unity�Ǝ��̌^�͎g����
/// int�Adouble�Astring�Abool�����g����Ɗo���Ă����Ă�������
/// ����float���g���Ȃ��͖̂Y�ꂪ���Ȃ̂Œ���
/// �l�X�g�����I�u�W�F�N�g�^���g���邩�͖�����
///
using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class SaveData
{
    //�f�[�^�Ɋ܂ޕ���������Ȃ��̂ŉ��ł����Ȃ��Ă܂�
    //��ŐF�X�t�������Ƃ��Ă�������

    //�Q�[���J�n���ɂ��̃t���O��false�Ȃ�^�C�g����Atutorial�ɑJ�ڂ���
    public bool completedTutorial;

    //�e�X�e�[�W�̃A�����b�N���o�̗L��
    public int[] stagesStatus = new int[(int)Stage.Tutorial];

    public int a = 0;
    public string b = "string";
    public double c = 3.14d;
    public bool d = false;
    public bool Stage1Frag = false;
}
