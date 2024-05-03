using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.InputSystem;

public class NPCindicatorTrigger : MonoBehaviour
{
    [Header("NPC Indicator")]
    [SerializeField] private GameObject floatingIcon;

    [Header("NPC Prompt")]
    [SerializeField] private GameObject promptIcon;

    [Header("Dialogue Files INK")]
    [SerializeField] private TextAsset DialogueFile;
    [SerializeField] private TextAsset DialogueFile2;

    public Quest questCheck;
    private NPCmovement NPClook;

    public Player player;

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
                Debug.Log("Quest check: " + questCheck.isComplete);
                Debug.Log("Quest active " + questCheck.isActive);
                //if (questCheck.isComplete)
                if (player.CanCompleteQuest())
                {
                    DialogueScript.GetInstance().EnterDialogueView(DialogueFile2);//Dialogue 2 will only play if a preceeding quest is done
                }
                else if(!questCheck.isComplete)
                {
                    DialogueScript.GetInstance().EnterDialogueView(DialogueFile);
                }

                /*if(questCheck.isActive)
                {
                    DialogueScript.GetInstance().EnterDialogueView(DialogueFile2);
                }
                else if(questCheck.isActive)
                {
                    DialogueScript.GetInstance().EnterDialogueView(DialogueFile);
                }*/
                
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
