using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TM.Entity.Player
{
    class PlayerRemoveDango
    {
        List<DangoColor> _dangos;
        DangoUIScript _dangoUIScript;

        public PlayerRemoveDango(List<DangoColor> dangos, DangoUIScript dangoUIScript)
        {
            _dangos = dangos;
            _dangoUIScript = dangoUIScript;
        }

        //�c�q�e(���O��)
        public void Remove()
        {
            //���ɉ����Ȃ���������s���Ȃ��B
            if (_dangos.Count == 0) return;

            //[Debug]�������������킩����
            //���܂ł́Adangos[dangos.Count - 1]�Ƃ��Ȃ���΂Ȃ�܂���ł������A
            //C#8.0�ȍ~�ł͈ȉ��̂悤�ɏȗ��ł���悤�ł��B
            //���́A�����m��Ȃ��l���ǂނƂ킯��������Ȃ��B
            Logger.Log(_dangos[^1]);

            //SE
            SoundManager.Instance.PlaySE(SoundSource.SE9_REMOVE_DANGO);

            //���������B
            _dangos.RemoveAt(_dangos.Count - 1);

            //UI�X�V
            _dangoUIScript.DangoUISet(_dangos);
        }
    }
}