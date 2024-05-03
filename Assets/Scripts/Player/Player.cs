using System;
using System.Collections;
using System.Collections.Generic; 
using UnityEngine;




public class Player : Character
{
    public int favourability;
    public Dictionary<string, int> inventory = new Dictionary<string, int>();
    public Items[] Items;
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
        if (currentQuest != null)
            currentQuest.isActive = true;
    }

    public void AddItem(string item, int quantity)
    {
        Items itemType = (Items)Enum.Parse(typeof(Items), item);
        for (int i = 0; i < quantity; i++)
        {
            if (Controller.Instance.CanAddItem(itemType))
            {
                Controller.Instance.AddItem(itemType);
            }
        }
    }

    public void RemoveItem(string item, int quantity)
    {
        Items itemType = (Items)Enum.Parse(typeof(Items), item);
        for (int i = 0; i < Controller.Instance.Item.Length; i++)
        {
            InventoryItem it = Controller.Instance.Item[i];
            if (it != null && it.GetComponentInChildren<ItemInside>().items == itemType)
            {
                Controller.Instance.DiscardItem(i);
                if (--quantity <= 0) break;
            }
        }
    }

    public int GetItemCount(string item)
    {
        return inventory.ContainsKey(item) ? inventory[item] : 0;
    }

    public bool CanCompleteQuest()
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

    public void CompleteQuest()
    {
        if (CanCompleteQuest())
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

    public void FailQuest()
    {
        if (currentQuest != null)
        {
            this.favourability -= currentQuest.favourabilityReward;
            currentQuest.complete();
            SetQuest(null);
        }
    }
}
