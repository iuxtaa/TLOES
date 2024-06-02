using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class QuestObjectiveManager : MonoBehaviour
{
    public static QuestObjectiveManager instance { get; private set; }

    public QuestObjective[] questObjectives;
    public Player player;
    private bool playerClose;
    private bool canUpdateSelling_Beggar = true;
    private bool canUpdateSelling_Cecil = true;

    // Start is called before the first frame update
    void Start()
    {
        initializeObjectives();
    }

    // Update is called once per frame
    void Update()
    {
        updateObjectives();
        updateCurrentQuestObjective();
    }

    // at the start of a NEW game all quest objectives will start as incomplete
    public void initializeObjectives()
    {
        foreach (QuestObjective objective in questObjectives)
        {
            objective.completionStatus = false;
        }
    }

    // If an objective is incomplete and can be completed, the objective will complete
    public void updateObjectives()
    {
        foreach (QuestObjective objective in questObjectives)
        {
            if (!objective.completionStatus && objective.checkCanComplete())
            {
                objective.complete();
                Debug.Log($"Objective Completed: {objective.description}");
            }
        }
    }

    // Method for updating the objective's completion status
    public void updateObjectiveCompletion(QuestObjective objective, bool newStatus)
    {
        objective.completionStatus = newStatus;
    }

    public void updateCurrentQuestObjective()
    {
        QuestObjective currentObjective = null;
        if (Player.currentQuest != null)
        {
            if (Player.currentQuest.questNumber == (int)QuestIndex.SellingEggs)
            {
                foreach (QuestObjective objective in questObjectives)
                {
                    if (objective is SellingQuestObjective sellingQuestObjective)
                    {
                        currentObjective = sellingQuestObjective;
                    }
                }
                if (playerClose)
                {
                    if (this.gameObject.name == "Egg_Begger" && canUpdateSelling_Beggar)
                    {
                        Debug.Log("bleh bleh bleh");
                        incrementObjectiveSellingCount(currentObjective, CollectableItems.amountGivenToBeggar);
                        canUpdateSelling_Beggar = false;
                    }
                    if (this.gameObject.name == "Egg_Cecil" && canUpdateSelling_Cecil && InputsHandler.GetInstance().check)
                    {
                        incrementObjectiveSellingCount(currentObjective, CollectableItems.amountGivenToCecil);
                        canUpdateSelling_Cecil = false;
                    }
                }
            }
        }
    }
    // Method for incrementing selling count of a quest, objective must be a SellingQuestObjective
    public void incrementObjectiveSellingCount(QuestObjective objective, int increment)
    {
        if (objective is SellingQuestObjective sellingQuestObjective)
        {
            sellingQuestObjective.incSellingCount(increment);
        }
    }

    // Method for checking if the quest objective is dependent on another quest objective being complete
    public bool isDependent(QuestObjective objective)
    {
        return (objective.isDependent());
    }

    // Method for checking if the quest objective's depend objective is complete.
    public bool isDependentObjectiveComplete(QuestObjective objective)
    {
        return (objective.isDependentObjectiveComplete());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        playerClose = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        playerClose = false;
    }
}
