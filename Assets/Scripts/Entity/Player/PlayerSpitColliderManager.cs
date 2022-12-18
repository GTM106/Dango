using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using TM.Entity.Player;
using UnityEngine;

public class PlayerSpitColliderManager : MonoBehaviour
{
    PlayerAttackAction _playerAttack;
    [SerializeField] ImageUIData _rangeUI;
    [SerializeField] CapsuleCollider _collider;

    private void Awake()
    {
        Logger.Assert(_rangeUI.ImageData.GetWidth() == _rangeUI.ImageData.GetHeight());

        _collider.radius = _rangeUI.ImageData.GetWidth() / 2f;
    }

    //突き刺し用のCollider。Player本体のコライダーはモデルについている
    private void OnTriggerEnter(Collider other)
    {
        _playerAttack.OnTriggerEnter(other);
    }

    //突き刺し用のCollider。Player本体のコライダーはモデルについている
    private void OnTriggerExit(Collider other)
    {
        _playerAttack.OnTriggerExit(other);
    }

    public void SetRangeUIEnable(bool isGroundOrEvent)
    {
        float alpha = isGroundOrEvent ? 155f / 255f : 0;

        _rangeUI.ImageData.SetAlpha(alpha);
    }

    public void SetPlayerAttack(PlayerAttackAction playerAttackAction)
    {
        _playerAttack = playerAttackAction;
    }
}
