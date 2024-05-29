using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestObjectiveManager : MonoBehaviour
{
    public QuestObjective[] questObjectives;
    // Start is called before the first frame update
    void Start()
    {
        initializeObjectives();
    }

    // Update is called once per frame
    void Update()
    {
        updateObjectives();
    }

    // at the start of a NEW game all quest objectives will start as incomplete
    public void initializeObjectives()
    {
        foreach(QuestObjective objective in questObjectives)
        {
            objective.completionStatus = false;
        }
    }

    // If an objective is incomplete and can be completed, the objective will complete
    public void updateObjectives()
    {
        foreach (QuestObjective objective in questObjectives)
        {
            if(!objective.completionStatus && objective.checkCanComplete())
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

    // Method for incrementing selling count of a quest, objective must be a SellingQuestObjective
    public void incrementObjectiveSellingCount(QuestObjective objective, int increment)
    {
        if(objective is SellingQuestObjective sellingQuestObjective)
        {
            sellingQuestObjective.incSellingCount(increment);
        }
    }
}
