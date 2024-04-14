using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionQuest : Quest // Inherits from Quest
{
    // INSTANCE VARIABLES
    private int requiredAmount; // The amount needed to complete the quest
    private int currentAmount; // The current amount collected

    // CONSTRUCTOR
    // Initializes a new instance of CollectionQuest with specified details
    public CollectionQuest(string title, string description, int goldReward, int favourabilityReward, int requiredAmount)
    {
        // Setting the quest details via properties in the base class
        setTitle(title);
        setDescription(description);
        setGoldReward(goldReward);
        setFavourabilityReward(favourabilityReward);
        setIsActive(false); // Assuming the quest is active upon creation

        // Setting CollectionQuest-specific details
        this.requiredAmount = requiredAmount;
        this.currentAmount = 0; // Starts with 0 items collected
    }

    // Checks if the required amount has been collected
    public bool isReached()
    {
        return currentAmount >= requiredAmount;
    }

    // Updates the current amount of items collected
    public void collectItem(int amount)
    {
        if (getIsActive()) // Only collect items if the quest is active
        {
            currentAmount += amount;
            Debug.Log($"{getTitle()} quest: collected {currentAmount}/{requiredAmount} items.");

            if (isReached())
            {
                complete(); // Complete the quest if the goal is reached
            }
        }
    }

    // Overrides the complete method to log a message specific to collection quests
    public override void complete()
    {
        if (isReached())
        {
            setIsActive(false); // Deactivate the quest
            Debug.Log($"{getTitle()} quest is completed. Reward: {getGoldReward()} gold and {getFavourabilityReward()} favourability.");
        }
        else
        {
            Debug.Log($"{getTitle()} quest cannot be completed because the item collection is incomplete.");
        }
    }
}