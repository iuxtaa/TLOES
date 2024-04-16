using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class TempPLayerController : MonoBehaviour
{
    public float run = 3.0f;
    public float gravity = 20f;

    private BoxCollider2D bc2D;
    private Rigidbody2D rb2D;

    private void Awake()
    {
        bc2D = GetComponent<BoxCollider2D>();
        rb2D = GetComponent<Rigidbody2D>();
        rb2D.gravityScale = gravity;
    }

    private void FixedUpdate()
    {
        if (DialogueScript.GetInstance().currentDialogueIsPlaying)
        {
            return;
        }
        movement();
        
    }
    private void movement()
    {
        Vector2 move = InputsHandler.GetInstance().GetMove();
        rb2D.velocity = new Vector2(move.x * run, rb2D.velocity.y);
    }
}
