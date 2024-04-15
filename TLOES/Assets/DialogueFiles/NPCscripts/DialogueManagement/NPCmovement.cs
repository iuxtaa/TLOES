using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCmovement : MonoBehaviour
{
    public Transform NPCturn;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            LookAtPlayer();
        }
    }

    private void LookAtPlayer()
    {
        Vector3 turn = transform.localScale;
        turn.x = (NPCturn.position.x < transform.position.x) ? -Mathf.Abs(turn.x) : Mathf.Abs(turn.x);
        transform.localScale = turn;
    }
}
