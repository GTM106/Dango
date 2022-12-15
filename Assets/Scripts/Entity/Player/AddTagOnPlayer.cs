using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddTagOnPlayer : MonoBehaviour
{
    //---------------------------------------------------------
    //シネマシーンを使用した際に特殊処理としてコライダーを無視するタグを付与するためのスクリプトです。
    //バグの原因がわからず応急処置的なことになっているため、原因がわかり次第削除推奨になります。
    //---------------------------------------------------------

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            gameObject.tag = "StageCollider";
        }

    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            gameObject.tag = "Untagged";
        }
    }
}
