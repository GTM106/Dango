using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public enum SoundSource
{
    //BGM
    BGM_Stage,
    

    //SE
    SE_PLAYER_STAY_EATDANGO,//���̍��ڂ�SE�̐擪�Œ�ł��肢���܂��B
    SE_PLAYER_EATDANGO,
    SE_STAB_DANGO,
    SE_REMOVE_DANGO,
    SE_CREATE_ROLE,

}

#if UNITY_EDITOR
[CustomEditor(typeof(SoundManager))]
public class SoundManagerOnGUI : Editor
{
    private SoundManager soundManager;

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        soundManager = target as SoundManager;

        EditorGUILayout.HelpBox("BGM�ASE��ǉ�����ۂ͏��Ԃɒ��ӂ��Ă��������B", MessageType.Info);

        EditorGUILayout.Separator();

        EditorGUILayout.LabelField(soundManager._debugSoundSource >= SoundSource.SE_PLAYER_STAY_EATDANGO ? "SE Clip : Element " + (int)(soundManager._debugSoundSource - SoundSource.SE_PLAYER_STAY_EATDANGO) : "BGM Clip : Element " + (int)soundManager._debugSoundSource);
        EditorGUILayout.Separator();

        base.OnInspectorGUI();

    }
}
#endif

public class SoundManager : MonoBehaviour
{
    [Tooltip("BGM�ASE�̒ǉ��ꏊ���擾�ł��܂��B")]
    public SoundSource _debugSoundSource;

    private AudioSource[] SEs = new AudioSource[SENum];
    private const int SENum = 10;

    [SerializeField] private AudioSource _BGM;
    [SerializeField] private GameObject _SEPrefab;

    [SerializeField] private AudioClip[] BGMClip;
    [SerializeField] private AudioClip[] SEClip;

    public AudioSource BGM => _BGM;

    private void Awake()
    {
        CreateSEs();
    }

    private void CreateSEs()
    {
        for (int i = 0; i < SENum; i++)
        {
            SEs[i] = Instantiate(_SEPrefab).GetComponent<AudioSource>();
            SEs[i].name = ("SE" + (i + 1));
        }

    }

    private void ChangeBGM(SoundSource sound)
    {
        int temp = 0;
        foreach (var bgm in BGMClip)
        {
            if ((int)sound == temp)
            {
                _BGM.clip = bgm;
                temp = 0;
                break;
            }
            else temp++;
        }
    }

    /// <summary>
    /// BGM�������܂��B
    /// </summary>
    /// <param name="sound">��������BGM</param>
    public void PlayBGM(SoundSource sound)
    {
        ChangeBGM(sound);
        _BGM.Play();
    }

    public void StopBGM()
    {
        _BGM.Stop();
    }

    private void ChangeSE(AudioSource _as, SoundSource sound)
    {
        int temp = BGMClip.Length;
        foreach (var se in SEClip)
        {
            if ((int)sound == temp)
            {
                _as.clip = se;
                temp = BGMClip.Length;
                break;
            }
            else temp++;
        }

    }

    /// <summary>
    /// SE���Đ����܂��B10�`�����l�����ׂė��p����Ă����ꍇ����܂���
    /// </summary>
    /// <param name="sound">�Đ�������SE</param>
    public void PlaySE(SoundSource sound)
    {
        foreach (var se in SEs)
        {
            if (se.isPlaying) continue;

            ChangeSE(se, sound);
            se.Play();
            return;
        }
        //���ׂẴ`�����l�����g�p���Ȃ炱���ɂ���
        Logger.Warn("�SSE�`�����l�����g�p����" + sound + "���Đ��ł��܂���ł���");
    }

    /// <summary>
    /// ���ׂĂ�SE���~�����܂��B
    /// </summary>
    public void StopSE()
    {
        foreach (AudioSource se in SEs)
        {
            if (se.isPlaying == false) continue;

            se.Stop();
        }

    }
}
