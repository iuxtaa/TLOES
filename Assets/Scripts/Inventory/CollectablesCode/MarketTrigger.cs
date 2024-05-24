using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MarketTrigger : MonoBehaviour
{
    public Player player;
    private bool playerClose = false;

    [Header("NPC Prompt")]
    [SerializeField] private GameObject promptIcon;

    private NPCmovement NPClook;
    public GameObject invent;


    private void Awake()
    {
        playerClose = false;
        promptIcon.SetActive(false);
        NPClook = GetComponent<NPCmovement>();
        // invent = GameObject.Find("inventorybg");

    }

    private void Update()
    {
        if (playerClose && !invent.activeInHierarchy)
        {
            NPClook.NPClookAtPlayer();
            promptIcon.SetActive(true);
        }
        else
        {
         
            promptIcon.SetActive(false);
        }
          
     }
    


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerClose = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerClose = false;
        }
    }
}
