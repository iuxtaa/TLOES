using System;
using System.Collections;
using System.Collections.Generic; // Required for Dictionary
using UnityEngine;



public class Player : Character
{
    // INSTANCE VARIABLES 
    public int favourability;
    public Dictionary<string, int> inventory = new Dictionary<string, int>();  // Initialize inventory
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

    public void AddItem(string item, int quantity)
    {
        if (inventory.ContainsKey(item))
        {
            inventory[item] += quantity;
        }
        else
        {
            inventory.Add(item, quantity);
        }
    }

    public void RemoveItem(string item, int quantity)
    {
        if (inventory.ContainsKey(item))
        {
            inventory[item] -= quantity;
            if (inventory[item] <= 0)
            {
                inventory.Remove(item);
            }
        }
    }

    public int GetItemCount(string item)
    {
        return inventory.ContainsKey(item) ? inventory[item] : 0;
    }

    public bool canCompleteQuest()
    {
        if (currentQuest != null)
        {
            if (currentQuest is SellingQuest sellingQuest)
            {
                return GetItemCount(sellingQuest.requiredItem.name) >= sellingQuest.requiredAmount;
            }
            if (currentQuest is DoingQuest)
            {
                return true;  
            }
            if (currentQuest is CollectingQuest collectingQuest)
            {
                return GetItemCount(collectingQuest.requiredItem.name) >= collectingQuest.requiredAmount;
            }
        }
        return false;
    }

    public void completeQuest()
    {
        if (canCompleteQuest())
        {
            this.favourability += currentQuest.favourabilityReward;
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
        this.favourability -= currentQuest.favourabilityReward;
        currentQuest.complete();
        SetQuest(null);
    }
}
