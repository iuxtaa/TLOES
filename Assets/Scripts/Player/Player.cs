using System;
using System.Collections;
using System.Collections.Generic; // Required for Dictionary
using UnityEngine;  

public class Player : Character {

    // INSTANCE VARIABLES 
    public int favourability;
    public Dictionary<string, int> inventory;  // Inventory using a dictionary NEED to ask enab about how this is stored
    [SerializeField] public Quest currentQuest;

    public Player(string name) : base(name)
    {
        SetFavourability(0);
        SetQuest(null);
    }

    public Player(string name, int currentLocation, int favourability, Quest currentQuest) : base(name, currentLocation)
    {
        SetFavourability(favourability);
        SetQuest(currentQuest);
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

    public void SetQuest(Quest quest)
    {
        this.currentQuest = quest;
    }

    public Quest GetQuest()
    {
        return this.currentQuest;
    }

    public void acceptQuest(Quest quest)
    {
        SetQuest(quest);
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
            this.favourability += currentQuest.favourabilityReward;
            currentQuest.complete();
            SetQuest(null);
        }
    }

    public void failQuest()
    {
        this.favourability -= currentQuest.favourabilityReward;
        currentQuest.complete();
        SetQuest(null);
    }
}