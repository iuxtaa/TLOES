using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CollectionQuest : Quest // Inherits from Quest
{
    // INSTANCE VARIABLES
    public GameObject requiredItem;

    // CONSTRUCTOR
    // Initializes a new instance of CollectionQuest with specified details
    public CollectionQuest(string title, string description, int goldReward, int favourabilityReward, int requiredAmount)
    {
        // Setting the quest details via properties in the base class
        this.title = title;
        this.description = description;
        this.goldReward = goldReward;
        this.favourabilityReward = favourabilityReward;
        this.isActive = true;// Assuming the quest is active upon creation

        // Setting CollectionQuest-specific details
        this.requiredAmount = requiredAmount;
        this.currentAmount = 0; // Starts with 0 items collected
    }

    // Updates the current amount of items collected
    public void collectItem(int amount)
    {
        if (isActive) // Only collect items if the quest is active
        {
            currentAmount += amount;
            Debug.Log($"{title} quest: collected {currentAmount}/{requiredAmount} items.");

            if (isReached())
            {
                complete(); // Complete the quest if the goal is reached
            }
        }
    }

    public void collectItem()
    {
        currentAmount++;
    }

    // Overrides the complete method to log a message specific to collection quests
    public void complete() // removed override (don't know if this should be removed)
    {
        if (isReached())
        {
            isActive = false; // Deactivate the quest
            Debug.Log($"{title} quest is completed. Reward: {goldReward} gold and {favourabilityReward} favourability.");
        }
        else
        {
            Debug.Log($"{title} quest cannot be completed because the item collection is incomplete.");
        }
    }
}