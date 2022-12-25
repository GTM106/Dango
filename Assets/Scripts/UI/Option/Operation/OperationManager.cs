using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class OperationManager : MonoBehaviour
{
    enum OperationChoices
    {
        CameraSensitivityYAxis,
        CameraSensitivityXAxis,
        CameraReversalH,
        CameraReversalV,

        Max,
    }

    //表示・非表示切り替え用に管理するもの
    [SerializeField] Canvas _canvas = default!;
    [SerializeField] Image[] _images;

    [SerializeField] ImageUIData _cameraSensitivityXImage;
    [SerializeField] ImageUIData _cameraSensitivityYImage;
    [SerializeField] List<Sprite> _scaleSprites;

    [SerializeField] Image _methodOfOperation = default!;
    [SerializeField] Sprite[] _methodOfOperationSprites;

    OperationChoices _choice = OperationChoices.CameraSensitivityYAxis;

    const int MAX_ROTATIONSPEED = 20;
    const int MIN_ROTATIONSPEED = 1;

    private void Start()
    {
        _cameraSensitivityXImage.ImageData.SetSprite(_scaleSprites[DataManager.configData.cameraRotationSpeedXAxis / 10 - 1]);
        _cameraSensitivityYImage.ImageData.SetSprite(_scaleSprites[DataManager.configData.cameraRotationSpeedYAxis / 10 - 1]);
    }

    public void AfterFusumaOpen()
    {
        InputSystemManager.Instance.onNavigatePerformed += OnNavigate;
        InputSystemManager.Instance.onChoicePerformed += OnChoice;
    }

    public void OnChangeScene()
    {
        InputSystemManager.Instance.onNavigatePerformed -= OnNavigate;
        InputSystemManager.Instance.onChoicePerformed -= OnChoice;
    }

    /// <summary>
    /// Canvasの表示・非表示を設定する関数
    /// </summary>
    public void SetCanvasEnable(bool enable)
    {
        _canvas.enabled = enable;

        if (enable)
        {
            _images[(int)_choice].color = new Color32(176, 176, 176, 255);
            _choice = OperationChoices.CameraSensitivityYAxis;
            _images[(int)_choice].color = Color.red;
        }
    }

    private void OnNavigate()
    {
        if (!_canvas.enabled) return;

        ChangeChoice(InputSystemManager.Instance.NavigateAxis);
        CameraSensitivityXChange(ref DataManager.configData.cameraRotationSpeedXAxis, InputSystemManager.Instance.NavigateAxis.x);
        CameraSensitivityYChange(ref DataManager.configData.cameraRotationSpeedYAxis, InputSystemManager.Instance.NavigateAxis.x);
    }

    private void OnChoice()
    {
        if (!_canvas.enabled) return;

        //boolのみ提示
        switch (_choice)
        {
            case OperationChoices.CameraReversalV:
                CameraReversalV();
                break;
            case OperationChoices.CameraReversalH:
                CameraReversalH();
                break;
        }
    }

    private void ChangeChoice(Vector2 axis)
    {
        if (!ChangeChoiceUtil.Choice(axis, ref _choice, OperationChoices.Max, false, ChangeChoiceUtil.OptionDirection.Vertical)) return;

        SetMethodOfOperation();
        _images[(int)_choice + (int)axis.y].color = new Color32(176, 176, 176, 255);
        _images[(int)_choice].color = Color.red;
        SoundManager.Instance.PlaySE(SoundSource.SE16_UI_SELECTION);
    }

    private void SetMethodOfOperation()
    {
        if (_methodOfOperationSprites.Length != 3) return;

        _methodOfOperation.sprite = _choice switch
        {
            OperationChoices.CameraSensitivityYAxis => _methodOfOperationSprites[2],
            OperationChoices.CameraSensitivityXAxis => _methodOfOperationSprites[2],
            OperationChoices.CameraReversalV => _methodOfOperationSprites[1],
            OperationChoices.CameraReversalH => _methodOfOperationSprites[1],
            _ => throw new System.NotImplementedException(),
        };
    }

    private void CameraReversalV()
    {
        DataManager.configData.cameraInvertYAxis ^= true;
        Logger.Log(DataManager.configData.cameraInvertYAxis);
    }

    private void CameraReversalH()
    {
        DataManager.configData.cameraInvertXAxis ^= true;
        Logger.Log(DataManager.configData.cameraInvertXAxis);
    }

    private void CameraSensitivityXChange(ref int rotationSpeed, float axis)
    {
        if (_choice != OperationChoices.CameraSensitivityXAxis) return;
        if (axis != 1 && axis != -1) return;

        //10を掛けているのは数値が低いと大差がないための補正
        rotationSpeed = Mathf.Clamp(rotationSpeed / 10 + (int)axis, MIN_ROTATIONSPEED, MAX_ROTATIONSPEED) * 10;

        _cameraSensitivityXImage.ImageData.SetSprite(_scaleSprites[(rotationSpeed / 10) - 1]);

        //Logger.Log(rotationSpeed);
    }
    private void CameraSensitivityYChange(ref int rotationSpeed, float axis)
    {
        if (_choice != OperationChoices.CameraSensitivityYAxis) return;
        if (axis != 1 && axis != -1) return;

        //10を掛けているのは数値が低いと大差がないための補正
        rotationSpeed = Mathf.Clamp(rotationSpeed / 10 + (int)axis, MIN_ROTATIONSPEED, MAX_ROTATIONSPEED) * 10;

        _cameraSensitivityYImage.ImageData.SetSprite(_scaleSprites[(rotationSpeed / 10) - 1]);

        //Logger.Log(rotationSpeed);
    }
}