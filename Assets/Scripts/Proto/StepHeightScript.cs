using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StepHeightScript : MonoBehaviour
{
    public enum D5
    {
        D3 = 3,
        D4 = 4,
        D5 = 5,
        D6 = 6,
        D7 = 7,
    }

    [SerializeField] D5 objD5;
    [SerializeField] Renderer rend;

    private void Awake()
    {
        rend.enabled = false;
    }

    public void SetColor(int maxDangoCount)
    {
        rend.material.color = (int)objD5 <= maxDangoCount ? Color.green : Color.red;
    }

    public void SetStepEnabled(bool enabled)
    {
        rend.enabled = enabled;
    }
}