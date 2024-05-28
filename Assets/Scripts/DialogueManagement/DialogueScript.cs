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
    [SerializeField] private GameObject dialogueDisplay;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI speakerNameText;
    [SerializeField] private Animator imageAnimator;


    [Header("Dialogue Choice Options UI")]
    [SerializeField] private GameObject[] choices;
    private TextMeshProUGUI[] choicesText;

    [Header("Trigger Zone Objects")]
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


    //private NPCindicatorTrigger npcIndicator;

    //private QuestGiver questGivercurrent;
    private Coroutine typingDialogue;
    private bool canContinueNext;


    private Story currentDialogue;

    public bool currentDialogueIsPlaying { get; private set; }


    private static DialogueScript instance;
    private const string SPEAKER_TAG = "speaker";
    private const string PORTRAIT_TAG = "image";

    private void Awake()
    {
        if (instance != null)
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

        choicesText = new TextMeshProUGUI[choices.Length];
        int index = 0;

        foreach (GameObject choice in choices)
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
        //Quest start Binders
        currentDialogue.BindExternalFunction("fatherQuest", (string questName) =>
        {
            Debug.Log("Selling eggs letter should be running");
            father.acceptQuest();
            Debug.Log(questName);
        });
        currentDialogue.BindExternalFunction("KnightQuest", (string questName) =>
        {
            Knight.acceptQuest();
            Debug.Log(questName);
        });
        currentDialogue.BindExternalFunction("PriestQuest", (string questName) =>
        {
            Priest.acceptQuest();
            Debug.Log(questName);
        });

        //Quest end binders
        currentDialogue.BindExternalFunction("completeFatherQuest", (string compquestName) =>
        {
           father.completeQuest();
           Debug.Log(compquestName + "completion");
        });
        currentDialogue.BindExternalFunction("completePriestQuest", (string compquestName) =>
        {
           Priest.completeQuest();
            Debug.Log(compquestName + "completion");
        });

        currentDialogue.BindExternalFunction("completeKnightQuest", (string compquestName) =>
        {
           Knight.completeQuest();
            Debug.Log(compquestName + "completion");
        });

        //Buying and Selling Binders
        currentDialogue.BindExternalFunction("buyingandsellingApples", (string AppleActivity) =>
          {
              EggSellerTrigger.purchase();  
              Debug.Log("buying apples");
              Debug.Log(AppleActivity); 
         });
        currentDialogue.BindExternalFunction("buyingandsellingHam", (string HamActivity) =>
        {
            HamSellerTrigger.purchase();
            Debug.Log(HamActivity);
        });
        currentDialogue.BindExternalFunction("buyingandsellingWine", (string WineActivity) =>
        {
            WineSellerTrigger.purchase();
            Debug.Log(WineActivity);
        });
        currentDialogue.BindExternalFunction("buyingandsellingEggs", (string EggActivity) =>
        {
            EggSellerTrigger.purchase();
            Debug.Log(EggActivity);
            Debug.Log("buying eggs");
        });
        currentDialogue.BindExternalFunction("buyingandsellingPaper", (string PaperActivity) =>
        {
            PaperSellerTrigger.purchase();
            Debug.Log(PaperActivity);
        });

        //NPC actions
        currentDialogue.BindExternalFunction("BeggerEgg", (string actionName) =>
        {
            BeggerTrigger.purchase();
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
                FreezePlayer(true);
            }
        }
        else if (!currentDialogueIsPlaying)
        {
            if (!dialogueDisplay.activeInHierarchy)
            {
                FreezePlayer(false);
            }
            return;
        }

        if (canContinueNext && InputsHandler.GetInstance().GetContinuePressed())
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
            if (typingDialogue != null)
            {

                StopCoroutine(typingDialogue);

            }
            typingDialogue = StartCoroutine(TypeText(line));

            HandleTags(currentDialogue.currentTags);
        }
        else
        {
            LeaveDialogueView();

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

            // temperary code
            switch (tagKey)
            {
                case SPEAKER_TAG:
                    speakerNameText.text = tagValue;
                    Debug.Log("speaker=" + tagValue);
                    break;
                case PORTRAIT_TAG:
                    imageAnimator.Play(tagValue);
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
        List<Choice> currentChoices = currentDialogue.currentChoices;

        if (currentChoices.Count > choices.Length)
        {
            Debug.Log("there are too many choices" + currentChoices.Count);
        }

        int index = 0;
        foreach (Choice choice in currentChoices)
        {
            choices[index].gameObject.SetActive(true);
            choicesText[index].text = choice.text;
            index++;
        }

        for (int i = index; i < choices.Length; i++)
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
            if (InputsHandler.GetInstance().GetContinuePressed())
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
        if (state)
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
