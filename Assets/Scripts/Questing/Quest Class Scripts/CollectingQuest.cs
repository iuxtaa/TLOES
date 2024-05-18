using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

[System.Serializable]
[CreateAssetMenu(fileName = "New Collecting Quest", menuName = "Quest System/Collecting Quest")]

public class CollectingQuest : Quest  // Inherits from Quest
{
    // INSTANCE VARIABLES
    public CollectableItems requiredItem;
    public int requiredAmount;
    public int currentAmount = 0;  // Tracks the amount of the item collected

    // CONSTRUCTOR
    // Initializes a new instance of CollectionQuest with specified details
    public CollectingQuest(CollectableItems requiredItem, int requiredAmount) : base()
    {
        this.requiredItem = requiredItem;
        this.requiredAmount = requiredAmount;
    }

    // METHODS
    public bool isReached()
    {
        return currentAmount >= requiredAmount;
    }
    
    public override string progress()
    {
        return "1" + "/" + requiredAmount;
    }
}