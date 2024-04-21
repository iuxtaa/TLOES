using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Selling Quest", menuName = "Quest System/Selling Quest")]
public class SellingQuest : Quest
{
    // INSTANCE VARIABLES
    public GameObject requiredItem;
    public int requiredAmount;
    //public int currentAmount;

    // CONSTRUCTOR
    public SellingQuest(int requiredAmount, int currentAmount) : base()
    {
        this.requiredAmount = requiredAmount;
        //this.currentAmount = currentAmount;
    }

    //public bool canSell()
    //{
    //return requiredAmount >= currentAmount;
    //}
}
