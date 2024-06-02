using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Collecting Quest Objective", menuName = "Quest System/Selling Quest Objective")]
public class SellingQuestObjective : QuestObjective
{
    public CollectableItems requiredItem;
    public int requiredSellingAmount;
    public int sellingCount;

    // Constructor that initializes all fields
    public SellingQuestObjective(QuestObjective dependentObjective, string description, CollectableItems requiredItem, int requiredSellingAmount)
        : base(dependentObjective, description)
    {
        this.requiredItem = requiredItem;
        this.requiredSellingAmount = requiredSellingAmount;
        this.sellingCount = 0;
    }
    public void incSellingCount(int amount)
    {
        sellingCount += amount;
        // checkCanComplete();
    }
    public override bool checkCanComplete()
    {
        return(sellingCount >= requiredSellingAmount);
        //if (sellingCount >= requiredSellingAmount)
        //{
        //    //complete();
        //    return true;
        //}
        //return false;
    }

    public override string toString()
    {
        return description + " " + sellingCount + "/" + requiredSellingAmount;
    }
}