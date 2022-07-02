using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackAction
{
    const int ATTACK_FRAME = 25;

    private int _currentTime = ATTACK_FRAME;

    public bool FixedUpdate()
    {
        //�˂��h�����i���Ƃɖ߂�ҋ@���j�Ȃ�
        if (--_currentTime > 0)
        {
            return false;
        }

        return true;
    }

    public void ResetTime()
    {
        _currentTime = ATTACK_FRAME;
    }
}
