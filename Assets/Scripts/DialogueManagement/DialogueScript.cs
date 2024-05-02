using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEditor.Experimental.GraphView.GraphView;

public class DialogueScript : MonoBehaviour
{
    [Header("Dialogue Management UI")]
    [SerializeField] private GameObject dialogueDisplay;
    [SerializeField] private TextMeshProUGUI dialogueText;
    

    [Header("Dialogue Choice Options UI")]
    [SerializeField] private GameObject[] choices;
    private TextMeshProUGUI[] choicesText;

    public QuestGiver questGiver;
    private Coroutine typingDialogue;
    private bool canContinueNext;

   
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
        questGiver = FindObjectOfType<QuestGiver>();
    }

    public static DialogueScript GetInstance()
    {
        return instance;
    }
    private void Start()
    {
        currentDialogueIsPlaying = false;
        dialogueDisplay.SetActive(false);

        choicesText = new TextMeshProUGUI[choices.Length];
        int index = 0;

        foreach(GameObject choice in choices)
        {
            choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
            index++;
        }
    }

    public void EnterDialogueView(TextAsset inkJSON)
    {
        currentDialogue = new Story(inkJSON.text);
        currentDialogueIsPlaying = true;
        dialogueDisplay.SetActive(true);
        currentDialogue.BindExternalFunction("startQuest", (string questName) => {
            questGiver.openQuestUI();
        });

        NextLine();
    }

    private void LeaveDialogueView()
    {
        currentDialogue.UnbindExternalFunction("startQuest");
        currentDialogueIsPlaying = false;
        dialogueDisplay.SetActive(false);
        dialogueText.text = "";

        
    }

    public void Update()
    {
        if(currentDialogueIsPlaying)
        {
            if(dialogueDisplay.activeInHierarchy)
            {
                FreezePlayer(true);
            }
        }
        else if (!currentDialogueIsPlaying) 
        {
            if(!dialogueDisplay.activeInHierarchy)
            {
                FreezePlayer(false);
            }
            return;
        }

        if(canContinueNext && InputsHandler.GetInstance().GetContinuePressed())
        {

            dialogueText.text = currentDialogue.currentText;

                NextLine();   
        }
    }

    private void NextLine()
    {
        if (currentDialogue.canContinue)
        {
           string line = currentDialogue.Continue();
            if(typingDialogue != null)
            {
                
               StopCoroutine(typingDialogue);
                
            }
           typingDialogue = StartCoroutine(TypeText(line));

            
        } 
        else
        {
            LeaveDialogueView();

        }
    }

    private void OptionDisplay()
    {
        List<Choice> currentChoices = currentDialogue.currentChoices;

        if(currentChoices.Count > choices.Length) 
        {
            Debug.Log("there are too many choices" + currentChoices.Count);
        }

        int index = 0;  
        foreach(Choice choice in currentChoices) 
        {
            choices[index].gameObject.SetActive(true);
            choicesText[index].text = choice.text;
            index++;
        }

        for(int i = index; i < choices.Length; i++)
        {
            choices[i].gameObject.SetActive(false);
        }

        StartCoroutine(SelectedFristChoice()); 
    }

    private IEnumerator SelectedFristChoice()
    {
        EventSystem.current.SetSelectedGameObject(null);

        yield return new WaitForEndOfFrame();

        EventSystem.current.SetSelectedGameObject(choices[0].gameObject);
    }

    private IEnumerator TypeText(string text)
    {
        
        dialogueText.text = "";
        canContinueNext = false;
        float textDisplaySpeed = 0.03f;
        HideOptions();
        canContinueNext = false;
        foreach (char c in text)
        {
            if(InputsHandler.GetInstance().GetContinuePressed())
            {
                dialogueText.text = text;
                break;
            }
            dialogueText.text += c; // Append one character at a time
            yield return new WaitForSecondsRealtime(textDisplaySpeed); // Wait for a specified duration
        }
        OptionDisplay();
        canContinueNext = true; 
    }


    public void chooseOption(int optionIndex)
    {
        if (optionIndex < 0 || optionIndex >= currentDialogue.currentChoices.Count)
        {
            return;
        }
        currentDialogue.ChooseChoiceIndex(optionIndex);
    }

    public void FreezePlayer(bool state)
    {
        if(state)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    private void HideOptions()
    {
        foreach (GameObject option in choices)
        {
            option.SetActive(false);
        }
    }

}