using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepBase : MonoBehaviour
{
    [SerializeField] PlayerData _playerData;

    List<StepHeightScript> _childSteps = new();

    void Awake()
    {
        //シリアライズして取得してもいいですが、数が多く設定が面倒なので自力で取得します
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

    //UI拡張表示が押されたときと離されたときの処理
    private void OnExpansionUI()
    {
        SetStepsEnabled(InputSystemManager.Instance.IsPressExpantionUI);
    }

    /// <summary>
    /// 段差表示の一括切り替え
    /// </summary>
    /// <param name="enabled">描画の有無</param>
    public void SetStepsEnabled(bool enabled)
    {
        foreach (var step in _childSteps)
        {
            step.SetStepEnabled(enabled);
        }
    }

    /// <summary>
    /// 現在のD5で登れるか判定し、一括で色を設定する
    /// </summary>
    /// <param name="maxDangoCount">現在のD5</param>
    public void SetStepsColor(int maxDangoCount)
    {
        foreach (var step in _childSteps)
        {
            step.SetColor(maxDangoCount);
        }
    }
}