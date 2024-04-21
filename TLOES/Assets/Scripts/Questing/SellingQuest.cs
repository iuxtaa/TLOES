using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellingQuest : Quest
{
    // INSTANCE VARIABLES
    public GameObject requiredItem;
    public int requiredAmount;
    //public int currentAmount;

    // CONSTRUCTOR
    public SellingQuest(int requiredAmount, int currentAmount)
    {
        this.questNumber = 0;

        this.title = string.Empty;
        this.description = string.Empty;
        this.favourabilityReward = 0;
        this.isActive = false;// Assuming the quest is active upon creation

        this.requiredAmount = requiredAmount;
        //this.currentAmount = currentAmount;
    }

    //public bool canSell()
    //{
        //return requiredAmount >= currentAmount;
    //}
}
