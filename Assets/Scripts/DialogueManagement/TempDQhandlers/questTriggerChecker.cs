using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class questTriggerChecker : MonoBehaviour
{
    public Player player;
    public GameObject triggerCheckConds;
    public bool droppedItem;// this is a place holder fir inventory

    private bool playerClose;
    // Start is called before the first frame update
    private void Awake()
    {
        playerClose = false;
        triggerCheckConds.SetActive(true);
        droppedItem = false;
    }
    public void Update()
    {
       

    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Player entered trigger zone");
            playerClose = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Player exited trigger zone");
            playerClose = false;
        }
    }
}
