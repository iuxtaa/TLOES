using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    // INSTANCE VARIABLE
    private int favourability;
    [SerializeField] public Quest currentQuest;

    // CONSTRUCTOR
    public Player(string name) : base(name)
    {
    }

    public Player(string name, int favourability, int currentLocation, int currentQuest) : base(name, currentLocation, currentQuest)
    {
        SetFavourability(favourability);
    }

    // METHODS
    public void SetFavourability(int favourability)
    {
        this.favourability = favourability;
    }

    public int GetFavourability()
    {
        return this.favourability;
    }

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
        if (currentQuest != null)
        {
            if (currentQuest is SellingQuest)
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
        if (canCompleteQuest())
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
