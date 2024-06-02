using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Collecting Quest Objective", menuName = "Quest System/Collecting Quest Objective")]
public class CollectingQuestObjective : QuestObjective
{
    public CollectableItems requiredItem;
    public int requiredAmount;
    public int collectingCount;

    // Constructor that initializes all fields
    public CollectingQuestObjective(QuestObjective dependentObjective, string description, CollectableItems requiredItem, int requiredAmount)
        : base(dependentObjective, description)
    {
        this.requiredItem = requiredItem;
        this.requiredAmount = requiredAmount;
    }

    public void incCollectingCount(int amount)
    {
        collectingCount += amount;
    }

    public override bool checkCanComplete()
    {
        return (collectingCount >= requiredAmount);
        //if (Player.inventory.getRequiredItem.getCount ?? >= requiredAmount) ?? NOT SURE -Ann
        // if (0 >= requiredAmount)
        // {
        //     complete();
        //     return true;
        // }
        // return false;
    }

    public override string toString()
    {
        // replace 3 with player's inventory required item count
        return description + " " + collectingCount + "/" + requiredAmount;
    }
}