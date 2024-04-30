using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Collecting Quest", menuName = "Quest System/Collecting Quest")]


public class CollectingQuest : Quest  // Inherits from Quest
{
    // INSTANCE VARIABLES
    public GameObject requiredItem;
    public int requiredAmount;
    public int currentAmount = 0;  // Tracks the amount of the item collected

    // CONSTRUCTOR
    // Initializes a new instance of CollectionQuest with specified details
    public CollectingQuest(GameObject requiredItem, int requiredAmount) : base()
    {
        this.requiredItem = requiredItem;
        this.requiredAmount = requiredAmount;
        this.currentAmount = 0;  
    }

    // METHODS
    public bool isReached()
    {
        return currentAmount >= requiredAmount;
    }

    public void addItem(int amount)
    {
        currentAmount += amount;
    }

    public bool canAddItem(int amount)
    {
        return (currentAmount + amount) <= requiredAmount;
    }
}
