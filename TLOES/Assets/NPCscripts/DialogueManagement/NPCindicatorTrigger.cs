using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NPCindicatorTrigger : MonoBehaviour
{
    [Header("NPC Indicator")]
    [SerializeField] private GameObject floatingIcon;
    [Header("Dialogue Files INK")]
    [SerializeField] private TextAsset inkJSON;

    private bool playerClose;

    private void Awake()
    {
        playerClose = false;
        floatingIcon.SetActive(false);
    }

    private void Update()
    {
        if (playerClose && !DialogueScript.GetInstance().currentDialogueIsPlaying)
        {
            floatingIcon.SetActive(true);
            if(InputsHandler.GetInstance().GetInteract())
            {
                DialogueScript.GetInstance().EnterDialogueView(inkJSON);
            }
        }
        else
        {
            floatingIcon.SetActive(false);
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
