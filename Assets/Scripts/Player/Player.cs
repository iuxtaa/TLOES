using UnityEngine;
using System.Collections.Generic;  // Required for Dictionary

public class Player : MonoBehaviour
{
    // INSTANCE VARIABLES 
    public int favourability;
    public Dictionary<string, int> inventory;  // Inventory using a dictionary NEED to ask enab about how this is stored
    public int currentLocation;
    [SerializeField] public Quest currentQuest;

    // CONSTRUCTOR
    public Player()
    {
        this.favourability = 0;
        this.inventory = null;
        this.currentLocation = 0;
        this.currentQuest = null;
    }

    // METHODS
    public void acceptQuest(Quest quest)
    {
        currentQuest = quest;
        currentQuest.isActive = true;
    }

    public void declineQuest()
    {

    }

    public bool canCompleteQuest()
    {
        if(currentQuest != null)
        {
            if(currentQuest is SellingQuest)
            {
                // return (inventory.item.count >= requiredAmount)
            }
            if (currentQuest is DoingQuest)
            {
                // return true;
            }
            if (currentQuest is CollectingQuest)
            {
                // return (inventory.item.count >= requiredAmount)
            }
        }
        return false;
    }

    public void completeQuest()
    {
        if(canCompleteQuest())
        {
            favourability += currentQuest.favourabilityReward;
            currentQuest.complete();
            currentQuest = null;
        }
    }

    public void failQuest()
    {
        favourability -= currentQuest.favourabilityReward;
        currentQuest.complete();
        currentQuest = null;
    }
}