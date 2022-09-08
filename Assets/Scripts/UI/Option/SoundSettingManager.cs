using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSettingManager : MonoBehaviour
{
    //�\���E��\���؂�ւ��p�ɊǗ��������
    [SerializeField] Canvas _canvas = default!;

    /// <summary>
    /// Canvas�̕\���E��\����ݒ肷��֐�
    /// </summary>
    public void SetCanvasEnable(bool enable)
    {
        _canvas.enabled = enable;
    }
}
