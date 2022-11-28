using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using TM.Entity.Player;
using UnityEngine;

public class PlayerSpitColliderManager : MonoBehaviour
{
    PlayerAttackAction _playerAttack;
    [SerializeField] ImageUIData rangeUI;

    //�˂��h���p��Collider�BPlayer�{�̂̃R���C�_�[�̓��f���ɂ��Ă���
    private void OnTriggerEnter(Collider other)
    {
        _playerAttack.OnTriggerEnter(other);
    }

    //�˂��h���p��Collider�BPlayer�{�̂̃R���C�_�[�̓��f���ɂ��Ă���
    private void OnTriggerExit(Collider other)
    {
        _playerAttack.OnTriggerExit(other);
    }

    public void SetRangeUIEnable(bool isGroundOrEvent)
    {
        float alpha = isGroundOrEvent ? 155f / 255f : 0;

        rangeUI.ImageData.SetAlpha(alpha);
    }

    public void SetPlayerAttack(PlayerAttackAction playerAttackAction)
    {
        _playerAttack = playerAttackAction;
    }
}
