using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.InputSystem;

public class NPCindicatorTrigger : MonoBehaviour
{
    [Header("NPC Indicator")] // header
    [SerializeField] private GameObject floatingIcon;

    [Header("NPC Prompt")]
    [SerializeField] private GameObject promptIcon;

    [Header("Dialogue Files INK")]
    [SerializeField] private TextAsset dialogueNoTalk;
    [SerializeField] private TextAsset dialogueStartQuest;
    [SerializeField] private TextAsset dialogueCannotFinishQuest;
    [SerializeField] private TextAsset dialogueCanFinishQuest;
    [SerializeField] private TextAsset dialogueFinishedQuest;

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
                try
                {
                    if (!questCheck.isDependentQuestComplete())
                    {
                        DialogueScript.GetInstance().EnterDialogueView(dialogueNoTalk);
                        Debug.Log("CANNOT START QUEST DIALOGUE");
                    }
                    // Player can start quest
                    else
                    {
                        // First time starting the quest
                        if (!questCheck.isActive && !questCheck.completionStatus)
                        {
                            DialogueScript.GetInstance().EnterDialogueView(dialogueStartQuest);
                            Debug.Log("STARTING QUEST DIALOGUE");
                        }
                        // Has started quest already but CANNOT complete the quest
                        else if (questCheck.isActive && !questCheck.canComplete())
                        {
                            DialogueScript.GetInstance().EnterDialogueView(dialogueCannotFinishQuest);
                            Debug.Log("CANNOT FINISH QUEST DIALOGUE");
                        }
                        // Has started quest already and CAN complete the quest
                        else if (questCheck.isActive && questCheck.canComplete())
                        {
                            DialogueScript.GetInstance().EnterDialogueView(dialogueCanFinishQuest);
                            Debug.Log("CAN FINISH QUEST DIALOGUE");
                        }
                        // Player has finished quest
                        else if (!questCheck.isActive && questCheck.completionStatus)
                        {
                            DialogueScript.GetInstance().EnterDialogueView(dialogueFinishedQuest);
                            Debug.Log("FINISHED QUEST DIALOGUE");
                        }
                        else
                        {
                            Debug.Log("Quest start logic error");
                        }
                    }
                }
                catch (ArgumentNullException)
                {
                    Debug.Log("Null ");
                }
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
