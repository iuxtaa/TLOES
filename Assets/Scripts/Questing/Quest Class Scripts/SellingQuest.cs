using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Selling Quest", menuName = "Quest System/Selling Quest")]

public class SellingQuest : Quest
{
    public CollectableItems requiredItem; 
    public int requiredAmount;

    // Constructor
    public SellingQuest(CollectableItems requiredItem, int requiredAmount)
    {
        this.requiredItem = requiredItem;
        this.requiredAmount = requiredAmount;
    }

    // METHODS
    public bool canComplete()
    {
        //if Player.inventory.
        return true;
    }

    public override string progress()
    {
        return "1" + "/" + requiredAmount;
    }
}

