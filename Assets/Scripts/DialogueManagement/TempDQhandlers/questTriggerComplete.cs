using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class questTriggerComplete : MonoBehaviour
{
    public Player player;
    public GameObject triggerCompQuest;
    public Quest questCheck;
    public questTriggerChecker checkItem; //this is a place holder for now

    private bool playerClose;
    // Start is called before the first frame update
    private void Awake()
    {
        playerClose = false;
        triggerCompQuest.SetActive(true);
        
    }
    public void Update()
    {
        if (playerClose && !questCheck.isComplete && questCheck.isActive && checkItem.droppedItem) //if player has item then dont complete the quest
        { 
            questCheck.isComplete = true;
            questCheck.isActive = false;    
        }
        else if (!playerClose && questCheck.isComplete && !questCheck.isActive)
        {
            triggerCompQuest.SetActive(false);
        }
       
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
