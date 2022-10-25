using UnityEngine;

// �ICamera�R���|�[�l���g�����Q�[���I�u�W�F�N�g�ɃA�^�b�`���Ă��������I
// ExecuteInEditMode            : �v���C���Ȃ��Ă����삳����
// ImageEffectAllowedInSceneView: �V�[���r���[�Ƀ|�X�g�G�t�F�N�g�𔽉f������
[ExecuteInEditMode, ImageEffectAllowedInSceneView]
public class BasicBlurEffectInserter : MonoBehaviour
{
    [SerializeField]
    private Material _material;

    private int _resolution;
    private Vector2 _displaySize;

    private void Awake()
    {
        _resolution = Shader.PropertyToID("_Resolution"); //�V�F�[�_�[�̃v���p�e�BID������
    }

    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        _displaySize.x = Screen.currentResolution.width; //�f�B�X�v���C�̍������擾
        _displaySize.y = Screen.currentResolution.height; //�f�B�X�v���C�̕����擾
        _material.SetVector(_resolution, _displaySize); //�V�F�[�_�[�̃v���p�e�B�ɗ�������

        //////////�����𔼕��ɂ��������_�[�e�X�N�`�����쐬�i�܂��A�Ȃɂ��`����Ă��Ȃ��j
        var rth = RenderTexture.GetTemporary(src.width / 2, src.height);
        Graphics.Blit(src, rth, _material); //�V�F�[�_�[�����������āA�������̃����_�[�e�N�X�`���ɃR�s�[
                                            /////////

        /////////��̃e�N�X�`���T�C�Y����A����ɏc�����ɂ��������_�[�e�X�N�`�����쐬�i�܂��A�Ȃɂ��`����Ă��Ȃ��j
        var rtv = RenderTexture.GetTemporary(rth.width, rth.height / 2);
        Graphics.Blit(rth, rtv, _material); //�V�F�[�_�[�����������āA�c�����̃����_�[�e�N�X�`���ɃR�s�[
        /////////

        Graphics.Blit(rtv, dest, _material); //���T�C�Y����1/4�ɂȂ��������_�[�e�N�X�`�����A���̃T�C�Y�ɖ߂�

        RenderTexture.ReleaseTemporary(rtv); //�e���|���������_�[�e�X�N�`���̊J��
        RenderTexture.ReleaseTemporary(rth); //�J�����Ȃ��ƃ��������[�N����̂Œ���
    }
}