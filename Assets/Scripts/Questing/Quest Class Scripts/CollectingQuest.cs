using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Collecting Quest", menuName = "Quest System/Collecting Quest")]
public class CollectingQuest : Quest // Inherits from Quest
{
    // INSTANCE VARIABLES
    public GameObject requiredItem;
    public int requiredAmount;

    // CONSTRUCTOR
    // Initializes a new instance of CollectionQuest with specified details
    public CollectingQuest(int requiredAmount) : base()
    {
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