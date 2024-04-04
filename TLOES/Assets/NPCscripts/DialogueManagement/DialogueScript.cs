using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueScript : MonoBehaviour
{
    [Header("Dialogue Management UI")]
    [SerializeField] private GameObject dialogueDisplay;
    [SerializeField] private TextMeshProUGUI dialogueText;
    private Story currentDialogue;
    public bool currentDialogueIsPlaying { get; private set; }


   private static DialogueScript instance;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogWarning("There is more than one instance");
        }
        instance = this;
    }

    public static DialogueScript GetInstance()
    {
        return instance;
    }
    private void Start()
    {
        currentDialogueIsPlaying = false;
        dialogueDisplay.SetActive(false);
    }

    public void EnterDialogueView(TextAsset inkJSON)
    {
        currentDialogue = new Story(inkJSON.text);
        currentDialogueIsPlaying = true;
        dialogueDisplay.SetActive(true);

        NextLine();
    }

    private void LeaveDialogueView()
    {
        currentDialogueIsPlaying = false;
        dialogueDisplay.SetActive(false);
        dialogueText = null;

    }

    public void Update()
    {
        if (!currentDialogueIsPlaying) 
        {
            return;
        }

        if(InputsHandler.GetInstance().GetContinuePressed())
        {
            NextLine();
        }
    }

    private void NextLine()
    {
        if (currentDialogue.canContinue)
        {
            dialogueText.text = currentDialogue.Continue();
        }
        else
        {
            LeaveDialogueView();
        }
    }
}
