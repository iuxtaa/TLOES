using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SearchService;
using static UnityEditor.Experimental.GraphView.GraphView;

public class DialogueScript : MonoBehaviour
{
    [Header("Dialogue Management UI")]
    [SerializeField] private GameObject dialogueDisplay; // UI element for displaying dialogue
    [SerializeField] private TextMeshProUGUI dialogueText;// Text UI element for dialogue text
    [SerializeField] private TextMeshProUGUI speakerNameText;// Text UI element for speaker name
    [SerializeField] private Animator imageAnimator;// Animator for character images


    [Header("Dialogue Choice Options UI")]
    [SerializeField] private GameObject[] choices;// UI elements for dialogue choices
    private TextMeshProUGUI[] choicesText;// Text UI elements for choices

    [Header("Trigger Zone Objects")] //trigger zones gameobject
    public MarketTrigger AppleSellerTrigger;
    public MarketTrigger HamSellerTrigger;
    public MarketTrigger WineSellerTrigger;
    public MarketTrigger EggSellerTrigger;
    public MarketTrigger PaperSellerTrigger;
    public MarketTrigger BeggerTrigger;

    [Header("Quest Giver Objects")]
    public QuestGiver father;
    public QuestGiver Knight;
    public QuestGiver Priest;

    [Header("Player Object")]
    public Player player;


    private Coroutine typingDialogue; // Coroutine for typing effect
    private bool canContinueNext;// Flag to check if the dialogue can continue


    private Story currentDialogue;// Current Ink story

    public bool currentDialogueIsPlaying { get; private set; }// Flag to check if dialogue is playing


    private static DialogueScript instance;// Singleton instance
    private const string SPEAKER_TAG = "speaker";// Tag for speaker name
    private const string PORTRAIT_TAG = "image";// Tag for character image



    private void Awake()
    {// Ensure a single instance
        if (instance != null)
        {
            Debug.LogWarning("There is more than one instance");
        }
        instance = this;
       
    }

    public static DialogueScript GetInstance()
    {
        return instance;// Get singleton instance
    }
    private void Start()
    {
        currentDialogueIsPlaying = false;// Dialogue is not playing initially
        dialogueDisplay.SetActive(false);// Hide dialogue UI initially

        choicesText = new TextMeshProUGUI[choices.Length];// Initialize choices text array
        int index = 0;

        foreach (GameObject choice in choices)
        {
            choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();// Get TextMeshPro component for each choice
            index++;
        }
    }

    public void EnterDialogueView(TextAsset inkJSON)
    {
        currentDialogue = new Story(inkJSON.text);// Load Ink story
        currentDialogueIsPlaying = true;// Set dialogue playing flag
        dialogueDisplay.SetActive(true);// Show dialogue UI
        //Quest start Binders
        currentDialogue.BindExternalFunction("fatherQuest", (string questName) =>
        {
            Debug.Log("Selling eggs letter should be running");
            father.acceptQuest();// Accept father's quest
            Debug.Log(questName);
        });
        currentDialogue.BindExternalFunction("KnightQuest", (string questName) =>
        {
            Knight.acceptQuest();// Accept knight's quest
            Debug.Log(questName);
        });
        currentDialogue.BindExternalFunction("PriestQuest", (string questName) =>
        {
            Priest.acceptQuest();// Accept priest's quest
            Debug.Log(questName);
        });

        //Quest end binders
        currentDialogue.BindExternalFunction("completeFatherQuest", (string compquestName) =>
        {
            turnOffColliderFather();// Complete father's quest
            Debug.Log(compquestName + "completed");
        });
        currentDialogue.BindExternalFunction("completePriestQuest", (string compquestName) =>
        {
            turnOffColliderPriest();// Complete father's quest
            Debug.Log(compquestName + "completed");
        });

        currentDialogue.BindExternalFunction("completeKnightQuest", (string compquestName) =>
        {
            turnOffColliderKnight();// Complete Priest's quest
            Debug.Log(compquestName + "completed");
        });

        //Buying and Selling Binders
        currentDialogue.BindExternalFunction("buyingandsellingApples", (string AppleActivity) =>
          {
              AppleSellerTrigger.purchase();// Handle apple purchase  
              Debug.Log("buying apples");
              Debug.Log(AppleActivity); 
         });
        currentDialogue.BindExternalFunction("buyingandsellingHam", (string HamActivity) =>
        {
            HamSellerTrigger.purchase();// Handle Ham purchase
            Debug.Log(HamActivity);
        });
        currentDialogue.BindExternalFunction("buyingandsellingWine", (string WineActivity) =>
        {
            WineSellerTrigger.purchase();// Handle Wine purchase
            Debug.Log(WineActivity);
        });
        currentDialogue.BindExternalFunction("buyingandsellingEggs", (string EggActivity) =>
        {
            EggSellerTrigger.purchase();// Handle Egg purchase
            Debug.Log(EggActivity);
            Debug.Log("buying eggs");
        });
        currentDialogue.BindExternalFunction("buyingandsellingPaper", (string PaperActivity) =>
        {
            PaperSellerTrigger.purchase();// Handle Paper purchase
            Debug.Log(PaperActivity);
        });

        //NPC actions
        currentDialogue.BindExternalFunction("BeggerEgg", (string actionName) =>
        {
            BeggerTrigger.purchase();// Handle beggar action
            Debug.Log(actionName + "completion");
        });


        NextLine();
    }

    private void LeaveDialogueView()
    {
        //Quests start unbinders
        currentDialogue.UnbindExternalFunction("KnightQuest");
        currentDialogue.UnbindExternalFunction("fatherQuest");
        currentDialogue.UnbindExternalFunction("PriestQuest");
        //Quest End unbinders
        currentDialogue.UnbindExternalFunction("completeFatherQuest");
        currentDialogue.UnbindExternalFunction("completePriestQuest");
        currentDialogue.UnbindExternalFunction("completeKnightQuest");
        //Selling unbunders
        currentDialogue.UnbindExternalFunction("buyingandsellingApples");
        currentDialogue.UnbindExternalFunction("buyingandsellingHam");
        currentDialogue.UnbindExternalFunction("buyingandsellingWine");
        currentDialogue.UnbindExternalFunction("buyingandsellingEggs");
        currentDialogue.UnbindExternalFunction("buyingandsellingPaper");
        //quest actions
        currentDialogue.UnbindExternalFunction("BeggerEgg");

        currentDialogueIsPlaying = false;
        dialogueDisplay.SetActive(false);
        dialogueText.text = "";
    }

    public void Update()
    {
        if (currentDialogueIsPlaying)
        {
            if (dialogueDisplay.activeInHierarchy)
            {
                FreezePlayer(true);// Freeze player during dialogue
            }
        }
        else if (!currentDialogueIsPlaying)
        {
            if (!dialogueDisplay.activeInHierarchy)
            {
                FreezePlayer(false);// Unfreeze player when dialogue is not playing
            }
            return;
        }

        if (canContinueNext && InputsHandler.GetInstance().GetContinuePressed())
        {

            dialogueText.text = currentDialogue.currentText;// Display current dialogue text

            NextLine();// Proceed to the next line of dialogue
        }

    }

    public void turnOffColliderBegger()
    {
        BoxCollider2D boxCollider = BeggerTrigger.GetComponent<BoxCollider2D>();
        if (boxCollider != null)
        {
            boxCollider.enabled = false;// Disable beggar's collider
        }
        else
        {
            Debug.Log("Box collider aint here chief");
        }
    }
    public void turnOffColliderCecil()
    {
        BoxCollider2D boxCollider = EggSellerTrigger.GetComponent<BoxCollider2D>();
        if (boxCollider != null)
        {
            boxCollider.enabled = false;// Disable egg seller's collider
        }
        else
        {
            Debug.Log("Box collider aint here chief");
        }
    }

    public void turnOffColliderFather()
    {
        BoxCollider2D boxCollider = father.GetComponent<BoxCollider2D>();
        if (boxCollider != null)
        {
            father.quest.objectives[1].complete();// Complete father's quest objective
            boxCollider.enabled = false;  // Disable father's collider
        }
        else
        {
            Debug.Log("Box collider aint here chief");
        }
    }
    public void turnOffColliderKnight()
    {
        BoxCollider2D boxCollider = Knight.GetComponent<BoxCollider2D>();
        if (boxCollider != null)
        {
            Knight.quest.objectives[1].complete();// Complete knight's quest objective 1
            Knight.quest.objectives[2].complete();// Complete knight's quest objective 2
            //boxCollider.enabled = false;
        }
        else
        {
            Debug.Log("Box collider aint here chief");
        }
    }
    public void turnOffColliderPriest()
    {
        BoxCollider2D boxCollider = Priest.GetComponent<BoxCollider2D>();
        if (boxCollider != null)
        {
            Priest.quest.objectives[1].complete();
            //boxCollider.enabled = false;
        }
        else
        {
            Debug.Log("Box collider aint here chief");
        }
    }



    private void NextLine()
    {
        if (currentDialogue.canContinue)
        {
            string line = currentDialogue.Continue();// Get the next line of dialogue
            if (typingDialogue != null)
            {

                StopCoroutine(typingDialogue);// Stop the current typing effect

            }
            typingDialogue = StartCoroutine(TypeText(line));// Start typing the new line

            HandleTags(currentDialogue.currentTags);// Handle tags in the dialogue
        }
        else
        {
            LeaveDialogueView();// End the dialogue if no more lines

        }
    }

    private void HandleTags(List<string> currentTags)
    {
        foreach (string tag in currentTags)
        {
            string[] splitTag = tag.Split(':');
            if(splitTag.Length !=2)
            {
                Debug.LogError("Tag could not be appropriately parsed: " +tag);
            }
            string tagKey = splitTag[0].Trim();
            string tagValue = splitTag[1].Trim();

            // Handle specific tags
            switch (tagKey)
            {
                case SPEAKER_TAG:
                    speakerNameText.text = tagValue;// Set speaker name
                    Debug.Log("speaker=" + tagValue);
                    break;
                case PORTRAIT_TAG:
                    imageAnimator.Play(tagValue);// Play character animation
                    Debug.Log("image=" + tagValue);
                    break;
                default:
                    Debug.LogWarning("tag that is being used it not fully handled" + tagValue);
                    break;
            }
        }
    }

    private void OptionDisplay()
    {
        List<Choice> currentChoices = currentDialogue.currentChoices;// Get current choices

        if (currentChoices.Count > choices.Length)
        {
            Debug.Log("there are too many choices" + currentChoices.Count);
        }

        int index = 0;
        foreach (Choice choice in currentChoices)
        {
            choices[index].gameObject.SetActive(true);// Show each choice
            choicesText[index].text = choice.text;
            index++;
        }

        for (int i = index; i < choices.Length; i++)
        {
            choices[i].gameObject.SetActive(false);// Hide unused choice UI elements
        }

        StartCoroutine(SelectedFristChoice());// Select the first choice by default
    }

    private IEnumerator SelectedFristChoice()
    {
        EventSystem.current.SetSelectedGameObject(null);// Deselect current selection

        yield return new WaitForEndOfFrame();

        EventSystem.current.SetSelectedGameObject(choices[0].gameObject);// Select the first choice
    }

    private IEnumerator TypeText(string text)
    {

        dialogueText.text = "";// Clear dialogue text
        canContinueNext = false;// Prevent continuing while typing
        float textDisplaySpeed = 0.03f;// Typing speed
        HideOptions();// Hide options while typing
        canContinueNext = false;// Prevent continuing while typing
        foreach (char c in text)
        {
            if (InputsHandler.GetInstance().GetContinuePressed())
            {
                dialogueText.text = text;// Skip typing if continue is pressed
                break;
            }
            dialogueText.text += c; // Append one character at a time
            yield return new WaitForSecondsRealtime(textDisplaySpeed); // Wait for a specified duration
        }
        OptionDisplay();// Display options after typing
        canContinueNext = true;// Allow continuing after typing
    }


    public void chooseOption(int optionIndex)
    {
        if (optionIndex < 0 || optionIndex >= currentDialogue.currentChoices.Count)
        {
            return;// Return if the option index is out of range
        }
        currentDialogue.ChooseChoiceIndex(optionIndex);// Choose the selected option
    }

    public void FreezePlayer(bool state)
    {
        if (state)
        {
            Time.timeScale = 0;// Freeze the game
        }
        else
        {
            Time.timeScale = 1;// Unfreeze the game
        }
    }

    private void HideOptions()
    {
        foreach (GameObject option in choices)
        {
            option.SetActive(false);// Hide each option
        }
    }

}
