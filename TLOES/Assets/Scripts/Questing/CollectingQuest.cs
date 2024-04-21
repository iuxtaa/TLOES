using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CollectingQuest : Quest // Inherits from Quest
{
    // INSTANCE VARIABLES
    public GameObject requiredItem;
    public int requiredAmount;

    // CONSTRUCTOR
    // Initializes a new instance of CollectionQuest with specified details
    public CollectingQuest(bool isActive, string title, string description, int favourabilityReward, int requiredAmount, int currentAmount)
    {
        // Setting the quest details via properties in the base class
        this.title = title;
        this.description = description;
        this.favourabilityReward = favourabilityReward;
        this.isActive = false; // Assuming the quest is inactive upon creation

        // Setting CollectionQuest-specific details
        this.requiredAmount = requiredAmount;
        // this.currentAmount = currentAmount; 
    }

    // METHODS
    //public bool isReached()
    //{
        //return currentAmount >= requiredAmount;
    //}
}