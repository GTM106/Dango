using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dango.Quest.UI {
    public class ExpansionUIcircleScript : MonoBehaviour
    {
        ExpansionCanvasScript expansion;
        float time = 0;
        private void Awake()
        {
            expansion = GameObject.Find("ExpansionCanvas").GetComponent<ExpansionCanvasScript>();
        }

        private void Update()
        {
            time += Time.deltaTime;

            if (expansion.set == true)
            {
                if (time > 3f)
                {
                    expansion.set = false;
                    expansion.OffSet();
                    Destroy(gameObject);
                }
            }
        }
        private void OnTriggerStay(Collider col)//�v���C���[�ɓ���������g��UI�\��
        {
            if (col.gameObject.tag =="Player")
            {
                expansion.Onset();
                expansion.set = true;
            }
        }
        private void OnTriggerExit(Collider col)//���ꂽ���\��
        {
            if (col.gameObject.tag == "Player")
            {
                expansion.set = false;
                expansion.OffSet();
                Destroy(gameObject);
            }
        }
    } 
}
