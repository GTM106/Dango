using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class OptionSystem : MonoBehaviour
{
    private const float _volumeDefaultScale = 2.0f;
    private float _volumeScale = default;

    private SoundManager _soundManager = default;

    private void Awake()
    {
        _volumeScale = _volumeDefaultScale;
    }

    private void OnEnable()
    {
        if (_soundManager == null) _soundManager = GameManager.SoundManager;

        //FixedUpdate�̎��s�����S�ɒ�~������
        Time.timeScale = 0;

        if (_soundManager != null)
        {
            _soundManager.BGM.volume /= volumeScale;
        }
    }

    private void OnDisable()
    {
        Time.timeScale = 1f;

        if (_soundManager != null)
        {
            _soundManager.BGM.volume *= volumeScale;
        }
    }

    private void Update()
    {
        ExitOption();
    }

    private void ExitOption()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// �I�v�V�����ɓ������ۂ�BGM�̉��ʔ{��(�����l�F2)
    /// </summary>
    public float volumeScale
    {
        get => _volumeScale;
        set
        {
            if (value <= 0)
            {
                Logger.Warn("�{�����[���̕ύX�{����0�ȉ��ɂ͂ł��܂���B�����l��" + _volumeDefaultScale + "�ɕύX���܂��B");
                _volumeScale = _volumeDefaultScale;
            }
            else
            {
                _volumeScale = value;
            }
        }
    }

}
