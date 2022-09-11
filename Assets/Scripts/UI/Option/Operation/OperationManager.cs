using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class OperationManager : MonoBehaviour
{
    enum OperationChoices
    {
        None,

        UseController,
        CameraSensitivity,
        CameraReversalV,
        CameraReversalH,

        Max,
    }

    //�\���E��\���؂�ւ��p�ɊǗ��������
    [SerializeField] Canvas _canvas = default!;
    [SerializeField] Image[] _images;

    OperationChoices _choice = OperationChoices.None + 1;

    private void Start()
    {
        InputSystemManager.Instance.onNavigatePerformed += OnNavigate;
    }

    public void OnChangeScene()
    {
        InputSystemManager.Instance.onNavigatePerformed -= OnNavigate;
    }

    /// <summary>
    /// Canvas�̕\���E��\����ݒ肷��֐�
    /// </summary>
    public void SetCanvasEnable(bool enable)
    {
        _canvas.enabled = enable;

        if (enable)
        {
            _images[(int)_choice - 1].color = new Color32(176, 176, 176, 255);
            _choice = OperationChoices.None + 1;
            _images[(int)_choice - 1].color = Color.red;
        }
    }

    private void OnNavigate()
    {
        if (!_canvas.enabled) return;

        Vector2 axis = InputSystemManager.Instance.NavigateAxis;
        if (axis != Vector2.up && axis != Vector2.down) return;

        if (axis == Vector2.up)
        {
            _choice--;
            if (_choice <= OperationChoices.None)
            {
                _choice = OperationChoices.None + 1;
                return;
            }
        }
        else if (axis == Vector2.down)
        {
            _choice++;
            if (_choice >= OperationChoices.Max)
            {
                _choice = OperationChoices.Max - 1;
                return;
            }
        }

        _images[(int)_choice - 1 + (int)axis.y].color = new Color32(176, 176, 176, 255);
        _images[(int)_choice - 1].color = Color.red;
        SoundManager.Instance.PlaySE(SoundSource.SE16_UI_SELECTION);
    }
}
