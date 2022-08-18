using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Volume
{
    /// <summary>
    /// Volume��ݒ肵�܂��B0-1�͈̔͂œ��͂��Ă��������B
    /// </summary>
    public float volume
    {
        get => volume;
        set => volume = Mathf.Clamp01(value);
    }

    /// <summary>
    /// Volume��ݒ肵�܂��B0-1�͈̔͂œ��͂��Ă��������B
    /// </summary>
    public Volume(float v)
    {
        volume = v;
    }
};
public struct Pan
{
    /// <summary>
    /// ���E�ǂ��炩�畷�����邩�ݒ�B-1�`1�͈̔͂œ��͂��Ă��������B
    /// </summary>
    public float pan
    {
        get => pan;
        set => pan = Mathf.Clamp(value, -1, 1);
    }

    /// <summary>
    /// ���E�ǂ��炩�畷�����邩�ݒ�B-1�`1�͈̔͂œ��͂��Ă��������B
    /// </summary>
    public Pan(float v)
    {
        pan = v;
    }

};
public struct Pitch
{
    /// <summary>
    /// ���̑�����ݒ�B-3�`3�͈̔͂œ��͂��Ă��������B
    /// </summary>
    public float pitch
    {
        get => pitch;
        set => pitch = Mathf.Clamp(value, -3, 3);
    }

    /// <summary>
    /// ���̑�����ݒ�B-3�`3�͈̔͂œ��͂��Ă��������B
    /// </summary>
    public Pitch(float v)
    {
        pitch = v;
    }
};

public class SESystem : MonoBehaviour
{
    private AudioSource _audioSource;

    private bool isPlaying;

    public delegate void StopSEEventHandler();
    public static event StopSEEventHandler OnStopSE;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (_audioSource.isPlaying)
        {
            isPlaying = true;
        }
        else if (isPlaying)
        {
            isPlaying = false;
            StopCallBack();
        }

    }

    protected virtual void StopCallBack()
    {
        OnStopSE?.Invoke();
    }

    #region public void Set()
    public void Set(Volume v)
    {
        _audioSource.volume = v.volume;
    }
    public void Set(Pan p)
    {
        _audioSource.panStereo = p.pan;
    }
    public void Set(Pitch p)
    {
        _audioSource.panStereo = p.pitch;
    }
    public void Set(Volume v, Pan p)
    {
        _audioSource.volume = v.volume;
        _audioSource.panStereo = p.pan;
    }
    public void Set(Volume v, Pitch p)
    {
        _audioSource.volume = v.volume;
        _audioSource.pitch = p.pitch;
    }
    public void Set(Pan pa, Pitch pi)
    {
        _audioSource.panStereo = pa.pan;
        _audioSource.pitch = pi.pitch;
    }
    public void Set(Volume v, Pan pa, Pitch pi)
    {
        _audioSource.volume = v.volume;
        _audioSource.panStereo = pa.pan;
        _audioSource.pitch = pi.pitch;
    }

    #endregion

}