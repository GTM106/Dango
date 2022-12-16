using Cinemachine;
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemachineIsStaing : MonoBehaviour
{
    [Header("↓レベルデザイン用変数↓")]
    [SerializeField] float _stayTime = 2f;
    [SerializeField] float _recenteringAngle = 45f;

    [Header("↓プログラム用変数↓")]
    [SerializeField] CinemachineFreeLook _freeLook;
    [SerializeField] Transform _playerTransform;

    float _currentTime = 0;

    bool _isRecentering;

    private void Update()
    {
        //入力があったらタイムリセット
        if (InputSystemManager.Instance.MoveAxis.magnitude > 0.1f || InputSystemManager.Instance.LookAxis.magnitude > 0.1f)
        {
            ResetTime();
        }

        RecenteringUpdate();
    }

    private void RecenteringUpdate()
    {
        //既にリセンタリング中なら弾く
        if (_isRecentering) return;

        _currentTime += Time.deltaTime;

        if (_currentTime >= _stayTime)
        {
            StartRecentering();
        }
    }

    private void StartRecentering()
    {
        float recenteringTime = Mathf.Max(CalcXAxisRecenteringTime(), CalcYAxisRecenteringTime());

        _isRecentering = true;

        _currentTime = 0;

        _freeLook.m_YAxisRecentering.m_enabled = true;
        _freeLook.m_YAxisRecentering.m_RecenteringTime = recenteringTime;

        _freeLook.m_RecenterToTargetHeading.m_enabled = true;
        _freeLook.m_RecenterToTargetHeading.m_RecenteringTime = recenteringTime;

        WaitRecentering(recenteringTime);
    }

    private async void WaitRecentering(float recenteringTime)
    {
        float currentTime = 0;

        //リセンタリングが終了するまで待機(+3sは余裕を持たせるためのマジックナンバー)
        while (currentTime < recenteringTime+3f)
        {
            await UniTask.Yield();

            //待機中に何らかの要因でリセンタリングが終了したら待機終了
            if (!_isRecentering) break;

            currentTime += Time.deltaTime;
        }

        ResetTime();
    }

    private void ResetTime()
    {
        _isRecentering = false;

        _currentTime = 0;

        _freeLook.m_YAxisRecentering.m_enabled = false;
        _freeLook.m_RecenterToTargetHeading.m_enabled = false;
    }

    private float CalcXAxisRecenteringTime()
    {
        float localEulerAngleY = _playerTransform.localEulerAngles.y;

        //-180～180の間で管理する
        if (localEulerAngleY > 180) localEulerAngleY -= 360f;

        float offset = localEulerAngleY - _freeLook.m_XAxis.Value;

        //移動する時間は、移動値の絶対値から1sあたりの移動時間を割ればよい
        return Mathf.Abs(offset) / _recenteringAngle;
    }

    private float CalcYAxisRecenteringTime()
    {
        float freeLookY = _freeLook.m_YAxis.Value;

        float offset = 0.5f - freeLookY;

        //offsetの0.5が大体45度回転するからそういう計算をする(比で90をかける)
        offset *= 90f;

        return Mathf.Abs(offset) / _recenteringAngle;
    }
}
