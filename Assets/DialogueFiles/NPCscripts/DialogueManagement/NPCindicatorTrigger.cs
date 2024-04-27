using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NPCindicatorTrigger : MonoBehaviour
{
    [Header("NPC Indicator")]
    [SerializeField] private GameObject floatingIcon;

    [Header("NPC Prompt")]
    [SerializeField] private GameObject promptIcon;

    [Header("Dialogue Files INK")]
    [SerializeField] private TextAsset inkJSON;

    private NPCmovement NPClook;


    private bool playerClose;

    private void Awake()
    {
        playerClose = false;
        floatingIcon.SetActive(false);
        promptIcon.SetActive(false);
        NPClook = GetComponent<NPCmovement>();
    }

    private void Update()
    {
        if (playerClose && !DialogueScript.GetInstance().currentDialogueIsPlaying)
        {
            floatingIcon.SetActive(true);
            promptIcon.SetActive(true);
            if(InputsHandler.GetInstance().GetInteract())
            {
                NPClook.NPClookAtPlayer();
                DialogueScript.GetInstance().EnterDialogueView(inkJSON);  
            }
            
        }
        else
        {
            floatingIcon.SetActive(false);
            promptIcon.SetActive(false);

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            playerClose=true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            playerClose=false;
        }
    }

}
