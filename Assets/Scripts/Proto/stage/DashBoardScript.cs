using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashBoardScript : MonoBehaviour
{
    [SerializeField] float speed;
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            col.GetComponent<Rigidbody>().AddForce(col.GetComponent<Player1>().move * speed);
        }
    }
}
