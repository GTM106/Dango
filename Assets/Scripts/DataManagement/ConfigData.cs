/// �ݒ�t�@�C���̎��̃N���X
/// ��{�I�ɂ̓Z�[�u�f�[�^�ƈ����͓���
/// Enum�^�͎g���Ȃ��̂ŃV���A���C�Y�������Ɍォ��ϊ����Ă���܂�
using System;
using UnityEngine;

public enum Language
{
    Unknown,
    JP,
    US
}

[Serializable]
public class ConfigData
{
    //��������� ������ȃt���O�Ȃ̂Őݒ�t�@�C���Ɋ܂߂Ȃ�
    //�f�[�^�����݂����A�t�@�C�����쐬(���Z�b�g)���ꂽ�u���ځv�̂ݐ^�ɂȂ�܂�
    [NonSerialized]
    public bool IsReset = false;

    //����ݒ�@���f�V���A���C�Y�o���Ȃ��̂Őݒ�t�@�C���Ɋ܂߂Ȃ�
    [NonSerialized]
    public Language language = Language.JP;

    //����ݒ�̃f�V���A���C�Y�p
    public string languageString = "JP";

    //�Q�[���p�b�h���g�p���邩
    public bool gamepadInputEnabled = true;

    //�Z�[�u�f�[�^�̃t�@�C���p�X
    public string dataFilePath = "default";

    //�}�X�^�[�{�����[��
    public int masterVolume = 0;

    //�T�E���h�G�t�F�N�g�{�����[��
    public int soundEffectVolume = 0;

    //�o�b�N�O���E���h�~���[�W�b�N�{�����[��
    public int backGroundMusicVolume = 0;

    //�{�C�X�{�����[��
    public int voiceVolume = 0;

    //�J�������x
    public int cameraRotationSpeed = 100;

    //�J�������������]
    public bool cameraHorizontalOrientation = false;

    //�J�����c�������]
    public bool cameraVerticalOrientation = false;
}