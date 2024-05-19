using System.Collections;
using System.Collections.Generic;
using System.Net.Security;
using Unity.VisualScripting;
using UnityEngine;

public class MarketTrigger : MonoBehaviour
{
    public Player player;
    private bool playerClose = false;

    [Header("NPC Prompt")]
    [SerializeField] private GameObject promptIcon;
    [SerializeField] private GameObject promptmessage;

    [Header("Dialogue Files INK")]
    [SerializeField] private TextAsset DialogueFile;

    private NPCmovement NPClook;
    private GameObject invent;

   
    public GameObject itemToBuyOrSell;


    private void Awake()
    {
        playerClose = false;
        promptIcon.SetActive(false);
        promptmessage.SetActive(false);
        NPClook = GetComponent<NPCmovement>();
        invent = GameObject.Find("inventorybg");
        itemToBuyOrSell.SetActive(false);
    }

    private void Update()
    {
        if(playerClose)
        {
            promptmessage.SetActive(true);
            if (!invent.activeInHierarchy && !DialogueScript.GetInstance().currentDialogueIsPlaying)
            {
                NPClook.NPClookAtPlayer();
                if (InputsHandler.GetInstance().GetInteract())
                {
                    DialogueScript.GetInstance().EnterDialogueView(DialogueFile);
                }
            }
            else
            {

                promptIcon.SetActive(false);
                promptmessage.SetActive(false);
            }

            if (!invent.activeInHierarchy && !DialogueScript.GetInstance().currentDialogueIsPlaying && itemToBuyOrSell.activeSelf)
            {
                promptIcon.SetActive(true);
            }
            else
            {
                promptIcon.SetActive(false);
            }
        }
        else if(!playerClose)
        {
            promptmessage.SetActive(false);
            promptIcon.SetActive(false);
        }
        
      

    }

    public void purchase()
    {
        StartCoroutine(ActivateBuy());
        Debug.Log("Purchase method entered");
    }

    private IEnumerator ActivateBuy()
    {
        itemToBuyOrSell.SetActive(true); // Activate the item
        yield return new WaitForSeconds(20); // Wait for 10 seconds
        itemToBuyOrSell.SetActive(false); // Deactivate the item
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