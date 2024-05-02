using System;
using System.Collections;
using System.Collections.Generic; // Required for Dictionary
using UnityEngine;  

public class Player : Character {

    // INSTANCE VARIABLES 
    public static int favourability;
    public static Dictionary<string, int> inventory = new Dictionary<string, int>();  // Initialize inventory
    [SerializeField] public static Quest currentQuest;

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
        Player.favourability = favourability;
    }

    public int GetFavourability()
    {
        return favourability;
    }

    public void SetQuest(Quest quest)
    {
        Player.currentQuest = quest;
    }

    public Quest GetQuest()
    {
        return currentQuest;
    }

    public void acceptQuest(Quest quest)
    {
        SetQuest(quest);
        currentQuest.isActive = true;
        Debug.Log(Player.currentQuest);
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
            if (currentQuest is CollectingQuest collectingQuest)
            {
                RemoveItem(collectingQuest.requiredItem.name, collectingQuest.requiredAmount);
            }
            else if (currentQuest is SellingQuest sellingQuest)
            {
                RemoveItem(sellingQuest.requiredItem.name, sellingQuest.requiredAmount);
            }
            currentQuest.complete();
            SetQuest(null);
        }
    }

    public void failQuest()
    {
        favourability -= currentQuest.favourabilityReward;
        currentQuest.complete();
        SetQuest(null);
    }
}