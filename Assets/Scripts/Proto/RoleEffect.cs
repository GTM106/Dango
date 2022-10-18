using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleEffect : MonoBehaviour
{
    [SerializeField] GameObject[] Effects;
    private GameObject EffectObj;
    private ParticleSystem particle;
    void Start()
    {
        
    }

    void SetEffect()
    {
        switch (DangoRoleUI.CurrentRoleName)
        {
            case "�P�F��":
                SetEffect(Effects[0]);
                break;
            case "���Ώ�":
                SetEffect(Effects[1]);
                break;
            case "���[�v":
                SetEffect(Effects[2]);
                break;
            case "�񕪊�":
                SetEffect(Effects[3]);
                break;
            case "�O����":
                SetEffect(Effects[4]);
                break;
        }
}
    private void Update()
    {
        if(particle!=null)
        if (particle.isStopped) //�p�[�e�B�N�����I������������
        {
            Destroy(EffectObj);//�p�[�e�B�N���p�Q�[���I�u�W�F�N�g���폜
        }
    }
    private void SetEffect(GameObject effect)
    {
        //�G�t�F�N�g�̕\��
        EffectObj = Instantiate(effect, this.transform.position, Quaternion.identity);
        particle =EffectObj.GetComponent<ParticleSystem>();

    }
}
