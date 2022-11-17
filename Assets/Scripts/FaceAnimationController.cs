using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �t�F�C�X�A�j���[�V�����Ǘ��N���X.
/// </summary>
public class FaceAnimationController : MonoBehaviour
{
    public enum FaceTypes
    {
        Default,    /// �f�t�H���g. 
        OpenMouth,  /// ���J��
        Chewing,    /// ��
        Smile,      /// �j�R�b

        ForTitle,   /// �^�C�g���p

        NoFace,     /// �̂��؂�ڂ�
    }

    [SerializeField] Material faceMaterial;

    static readonly List<Vector2> _faceOffsetTable = new() { Vector2.zero, new(0.3282f, 0), new(0.6563f, 0), new(0, 0.4846f), new(0.3282f, 0.4846f), new(0.6563f, 0.4846f) };

    /// <summary>
    /// ��̎�ނ�ύX.
    /// </summary>
    /// <param name="type">FaceTypes.</param>
    public void ChangeFaceType(FaceTypes type)
    {
        faceMaterial.SetTextureOffset("_MainTex", _faceOffsetTable[(int)type]);
    }
}
