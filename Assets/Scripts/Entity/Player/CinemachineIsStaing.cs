using Cinemachine;
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemachineIsStaing : MonoBehaviour
{
    [Header("�����x���f�U�C���p�ϐ���")]
    [SerializeField] float _stayTime = 2f;
    [SerializeField] float _recenteringAngle = 45f;

    [Header("���v���O�����p�ϐ���")]
    [SerializeField] CinemachineFreeLook _freeLook;
    [SerializeField] Transform _playerTransform;

    float _currentTime = 0;

    bool _isRecentering;

    private void Update()
    {
        //���͂���������^�C�����Z�b�g
        if (InputSystemManager.Instance.MoveAxis.magnitude > 0.1f || InputSystemManager.Instance.LookAxis.magnitude > 0.1f)
        {
            ResetTime();
        }

        RecenteringUpdate();
    }

    private void RecenteringUpdate()
    {
        //���Ƀ��Z���^�����O���Ȃ�e��
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

        //���Z���^�����O���I������܂őҋ@(+3s�͗]�T���������邽�߂̃}�W�b�N�i���o�[)
        while (currentTime < recenteringTime+3f)
        {
            await UniTask.Yield();

            //�ҋ@���ɉ��炩�̗v���Ń��Z���^�����O���I��������ҋ@�I��
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

        //-180�`180�̊ԂŊǗ�����
        if (localEulerAngleY > 180) localEulerAngleY -= 360f;

        float offset = localEulerAngleY - _freeLook.m_XAxis.Value;

        //�ړ����鎞�Ԃ́A�ړ��l�̐�Βl����1s������̈ړ����Ԃ�����΂悢
        return Mathf.Abs(offset) / _recenteringAngle;
    }

    private float CalcYAxisRecenteringTime()
    {
        float freeLookY = _freeLook.m_YAxis.Value;

        float offset = 0.5f - freeLookY;

        //offset��0.5�����45�x��]���邩�炻�������v�Z������(���90��������)
        offset *= 90f;

        return Mathf.Abs(offset) / _recenteringAngle;
    }
}
