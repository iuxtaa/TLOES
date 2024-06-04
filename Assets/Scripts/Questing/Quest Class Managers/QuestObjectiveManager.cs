using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
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
    private bool canUpdateCollecting_Patrick = true;
    private bool canUpdateCollecting_Well = true;

    // Static flag to check if objectives have been initialized
    private static bool objectivesInitialized = false;

    private void Start()
    {
        // Only initialize objectives if they haven't been initialized yet
        if (!objectivesInitialized)
        {
            initializeObjectives();
            resetSellingObjectives();
            objectivesInitialized = true; // Set the flag to true after initializing
        }
    }

    // Update is called once per frame
    void Update()
    {
        updateObjectives();
        updateCurrentQuestObjective();
    }

    // At the start of a NEW game all quest objectives will start as incomplete
    public void initializeObjectives()
    {
        foreach (QuestObjective objective in questObjectives)
        {
            objective.completionStatus = false;
        }
    }

    public void resetSellingObjectives()
    {
        foreach (QuestObjective objective in questObjectives)
        {
            if (objective is SellingQuestObjective sellingQuestObjective)
                sellingQuestObjective.sellingCount = 0;
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
                    if (objective is SellingQuestObjective sellingQuestObjective && !objective.completionStatus)
                    {
                        currentObjective = sellingQuestObjective;
                    }
                }
                if (playerClose)
                {
                    if (this.gameObject.name == "Egg_Begger" && canUpdateSelling_Beggar)
                    {
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
            else if (Player.currentQuest.questNumber == (int)QuestIndex.KnightsLetter)
            {
                foreach (QuestObjective objective in questObjectives)
                {
                    if (objective is CollectingQuestObjective collectingQuestObjective && !objective.completionStatus)
                    {
                        currentObjective = collectingQuestObjective;
                    }
                }

                if (playerClose)
                {
                    if (this.gameObject.name == "Paper_Patrick" && canUpdateCollecting_Patrick && InputsHandler.GetInstance().check && Player.money >= CollectableItems.PAPER_COST)
                    {
                        incrementObjectiveCollectingCount(currentObjective, CollectableItems.amountCollectForKnight);
                        canUpdateCollecting_Patrick = false;
                    }
                }
            }
            else if (Player.currentQuest.questNumber == (int)QuestIndex.PriestsHolyWater)
            {
                foreach (QuestObjective objective in questObjectives)
                {
                    if (objective is CollectingQuestObjective && !objective.completionStatus)
                    {
                        currentObjective = objective;
                    }
                }

                if (playerClose)
                {
                    if (this.gameObject.name == "WaterBottle_WishingWell" && canUpdateCollecting_Well && InputsHandler.GetInstance().check)
                    {
                        incrementObjectiveCollectingCount(currentObjective, CollectableItems.amountCollectForPriest);
                        canUpdateCollecting_Well = false;
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

    public void incrementObjectiveCollectingCount(QuestObjective objective, int increment)
    {
        if (objective is CollectingQuestObjective collectingQuestObjective)
        {
            collectingQuestObjective.incCollectingCount(increment);
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
