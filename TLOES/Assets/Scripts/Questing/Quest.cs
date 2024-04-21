using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] // Unity will serialize these fields so that they will show up in the Inspector
public class Quest
{
    // PRIVATE INSTANCE VARIABLES

    public bool isActive; // boolean for quest status

    public static int DEFAULT_ACCEPT_QUEST_FAVOURABILITY_REWARD = 1;

    public string title;
    public string description;
    public int favourabilityReward;
    public int goldReward;
    public int currentAmount;
    public int requiredAmount;

    public QuestGoal goal;

    // METHODS

    // Checks if the required amount has been collected
    public bool isReached()
    {
        return currentAmount >= requiredAmount;
    }

    public void complete()
    {
        isActive = false;
        Debug.Log(title + " quest is completed. ");
    }
}
