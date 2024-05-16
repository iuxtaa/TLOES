using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

[System.Serializable]



[CreateAssetMenu(fileName = "New Collecting Quest", menuName = "Quests/CollectingQuest")]
public class CollectingQuest : Quest
{
    public Items requiredItem;
    public int requiredAmount;

    // Constructor
    public CollectingQuest(int questNumber, string title, string desc, int favourabilityReward, Items requiredItem, int requiredAmount)
        : base(questNumber, title, desc, favourabilityReward)
    {
        this.requiredItem = requiredItem;
        this.requiredAmount = requiredAmount;
    }

    public bool CanCompleteQuest()
    {
        return inventory.GetItemCount(requiredItem.name) >= requiredAmount;
    }

    public void CompleteQuest()
    {
        if (inventory != null && CanCompleteQuest())
        {
            inventory.RemoveItem(requiredItem.name, requiredAmount);
            complete();
        }
        else
        {
            Debug.Log("Cannot complete quest. Item count not sufficient or Controller not available.");
        }
    }

    public override void complete()
    {
        base.complete();
        Debug.Log(title + " quest is completed. Congratulations!");
    }
}
