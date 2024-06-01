using System.Collections;
using System.Collections.Generic;
using System.Net.Security;
using Unity.VisualScripting;
using UnityEngine;

public class MarketTrigger : MonoBehaviour
{
    public Player player;
    public bool playerClose = false;

    [Header("NPC Prompt")]
    [SerializeField] private GameObject Instruction;
    [SerializeField] private GameObject promptmessage;

    [Header("Dialogue Files INK")]
    [SerializeField] private TextAsset DialogueFile;

    private NPCmovement NPClook;
    public GameObject invent;
    public GameObject itemToBuyOrSell;


    private void Awake()
    {
        playerClose = false;
        Instruction.SetActive(false);
        promptmessage.SetActive(false);
        NPClook = GetComponent<NPCmovement>();
        itemToBuyOrSell.SetActive(false);
    }

    private void Update()
    {
        if (playerClose)
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

                Instruction.SetActive(false);
                promptmessage.SetActive(false);
            }

            if (!invent.activeInHierarchy && !DialogueScript.GetInstance().currentDialogueIsPlaying && itemToBuyOrSell.activeSelf)
            {
                Instruction.SetActive(true);
            }
            else
            {
                Instruction.SetActive(false);
            }
        }
        else if (!playerClose)
        {
            promptmessage.SetActive(false);
            Instruction.SetActive(false);
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
        yield return new WaitForSeconds(20); // Wait for 20 seconds
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
