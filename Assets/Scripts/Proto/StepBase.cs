using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepBase : MonoBehaviour
{
    [SerializeField] PlayerData _playerData;

    List<StepHeightScript> _childSteps = new();

    void Awake()
    {
        //�V���A���C�Y���Ď擾���Ă������ł����A���������ݒ肪�ʓ|�Ȃ̂Ŏ��͂Ŏ擾���܂�
        for (int i = 0; i < transform.childCount; i++)
        {
            _childSteps.Add(transform.GetChild(i).GetComponent<StepHeightScript>());
        }
    }

    private void Start()
    {
        InputSystemManager.Instance.onExpansionUIPerformed += OnExpansionUI;
        InputSystemManager.Instance.onExpansionUICanceled += OnExpansionUI;
    }

    private void OnDestroy()
    {
        InputSystemManager.Instance.onExpansionUIPerformed -= OnExpansionUI;
        InputSystemManager.Instance.onExpansionUICanceled -= OnExpansionUI;
    }

    //UI�g���\���������ꂽ�Ƃ��Ɨ����ꂽ�Ƃ��̏���
    private void OnExpansionUI()
    {
        SetStepsEnabled(InputSystemManager.Instance.IsPressExpantionUI);
    }

    /// <summary>
    /// �i���\���̈ꊇ�؂�ւ�
    /// </summary>
    /// <param name="enabled">�`��̗L��</param>
    public void SetStepsEnabled(bool enabled)
    {
        foreach (var step in _childSteps)
        {
            step.SetStepEnabled(enabled);
        }
    }

    /// <summary>
    /// ���݂�D5�œo��邩���肵�A�ꊇ�ŐF��ݒ肷��
    /// </summary>
    /// <param name="maxDangoCount">���݂�D5</param>
    public void SetStepsColor(int maxDangoCount)
    {
        foreach (var step in _childSteps)
        {
            step.SetColor(maxDangoCount);
        }
    }
}