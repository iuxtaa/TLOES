using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Collecting Quest Objective", menuName = "Quest System/Collecting Quest Objective")]
public class CollectingQuestObjective : QuestObjective
{
    public CollectableItems requiredItem;
    public int requiredAmount;

    // Constructor that initializes all fields
    public CollectingQuestObjective(string description, CollectableItems requiredItem, int requiredAmount)
        : base(description)
    {
        this.requiredItem = requiredItem;
        this.requiredAmount = requiredAmount;
    }

    public override bool checkCanComplete()
    {
        //if (Player.inventory.getRequiredItem.getCount ?? >= requiredAmount)
        if (3 >= requiredAmount)
        {
            complete();
            return true;
        }
        return false;
    }

    public override string toString()
    {
        // replace 3 with player's inventory required item count
        return description + " " + 3 + "/" + requiredAmount;
    }
}