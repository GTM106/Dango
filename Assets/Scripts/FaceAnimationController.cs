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

    /// <summary>
    /// �A�j���[�V�������
    /// </summary>
    private struct AnimationInfo
    {
        public Vector2 TextureSize; /// �e�N�X�`���̃T�C�Y. 
        public Rect Atlas;          /// �A�g���X��w. 
        public int Type;            /// ���. 
        public Material Mat;        /// �}�e���A��. 
        public int VNum;            /// �c�����̃A�g���X��
        public int HNum;            /// �������̃A�g���X��
    }

    private AnimationInfo _faceInfo;
    public Material faceMaterial;
    public Vector2 faceAtlasSize;

    void Awake()
    {
        //��̃e�N�X�`�����
        _faceInfo.Mat = faceMaterial;
        Texture texture = _faceInfo.Mat.mainTexture;
        _faceInfo.TextureSize.x = texture.width;
        _faceInfo.TextureSize.y = texture.height;
        _faceInfo.Atlas.width = faceAtlasSize.x;
        _faceInfo.Atlas.height = faceAtlasSize.y;
        _faceInfo.VNum = (int)(_faceInfo.TextureSize.y / _faceInfo.Atlas.height);
        _faceInfo.HNum = (int)(_faceInfo.TextureSize.x / _faceInfo.Atlas.width);
    }

    /// <summary>
    /// ��̎�ނ�ύX.
    /// </summary>
    /// <param name="type">Type.</param>
    public void ChangeFaceType(FaceTypes type)
    {
        _faceInfo.Type = (int)type;

        // ���W�����߂�
        _faceInfo.Atlas.x = ((int)type / _faceInfo.VNum);
        _faceInfo.Atlas.y = ((int)type - (_faceInfo.Atlas.x * _faceInfo.VNum));
        _faceInfo.Atlas.x *= _faceInfo.Atlas.width;
        _faceInfo.Atlas.y *= _faceInfo.Atlas.height;

        // UV���W�v�Z
        Vector2 offset;
        offset = new Vector2(_faceInfo.Atlas.x / _faceInfo.TextureSize.x, 1.0f - (_faceInfo.Atlas.y / _faceInfo.TextureSize.y));

        // �K�p
        _faceInfo.Mat.SetTextureOffset("_MainTex", offset);
    }
}
