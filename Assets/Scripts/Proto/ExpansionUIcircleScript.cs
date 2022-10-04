using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dango.Quest.UI {
    public class ExpansionUIcircleScript : MonoBehaviour
    {
        public ExpansionCanvasScript expansion;

        private void Awake()
        {
            expansion = GameObject.Find("ExpansionCanvas").GetComponent<ExpansionCanvasScript>();
        }
        private void OnTriggerStay(Collider col)//�v���C���[�ɓ���������g��UI�\��
        {
            if (col.gameObject.CompareTag("Player"))
            {
                expansion.Onset();
                expansion.set = true;
            }
        }
        private void OnTriggerExit(Collider col)//���ꂽ���\��
        {
            if (col.gameObject.CompareTag("Player"))
            {
                expansion.set = false;
                expansion.OffSet();
                Destroy(gameObject);
            }
        }
    } 
}
