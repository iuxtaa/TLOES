using System;
using System.Collections;
using System.Collections.Generic; // Required for Dictionary
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : Character
{
    public int favourability;
    public Dictionary<string, int> inventory = new Dictionary<string, int>();
    public Items[] Items;
    [SerializeField] public Quest currentQuest;

    public KeyCode discardKey = KeyCode.Q;  // Key to discard an item set to Q

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

    void Update()
    {
        if (Input.GetKeyDown(discardKey))
        {
            // Discard the first item in the inventory when the key is pressed
            RemoveFirstItem(); // You can change this to target a specific item
        }
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
        if (quest != null)
        {
            quest.Initialize(Controller.Instance);
        }
    }

    public Quest GetQuest()
    {
        return this.currentQuest;
    }

    public void AcceptQuest(Quest quest)
    {
        SetQuest(quest);
        if (currentQuest != null)
            currentQuest.isActive = true;
    }

    public void AddItem(string itemName, int quantity)
    {
        Items itemType = Array.Find(Items, item => item.name == itemName);
        if (itemType != null)
        {
            for (int i = 0; i < quantity; i++)
            {
                if (Controller.Instance != null)
                {
                    Controller.Instance.AddItem(itemType, 1);
                }
            }

            if (inventory.ContainsKey(itemName))
            {
                inventory[itemName] += quantity;
            }
            else
            {
                inventory[itemName] = quantity;
            }
        }
        else
        {
            Debug.LogWarning($"Item {itemName} not found in Items array.");
        }
    }

    public void RemoveItem(string itemName, int quantity)
    {
        Items itemType = Array.Find(Items, item => item.name == itemName);
        if (itemType != null)
        {
            var itemsList = new List<InventoryItem>(Controller.Instance.itemsParent.GetComponentsInChildren<InventoryItem>());
            int removedCount = 0;

            for (int i = 0; i < itemsList.Count; i++)
            {
                InventoryItem it = itemsList[i];
                if (it != null && it.itemInside.items == itemType)
                {
                    Controller.Instance.DiscardItem(i);
                    removedCount++;
                    if (removedCount >= quantity) break;
                }
            }

            if (inventory.ContainsKey(itemName))
            {
                inventory[itemName] -= quantity;
                if (inventory[itemName] <= 0)
                {
                    inventory.Remove(itemName);
                }
            }
        }
        else
        {
            Debug.LogWarning($"Item {itemName} not found in Items array.");
        }
    }

    public void RemoveFirstItem()
    {
        var itemsList = new List<InventoryItem>(Controller.Instance.itemsParent.GetComponentsInChildren<InventoryItem>());
        if (itemsList.Count > 0)
        {
            InventoryItem firstItem = itemsList[0];
            if (firstItem != null)
            {
                Controller.Instance.DiscardItem(0);
                string itemName = firstItem.itemInside.items.name;
                if (inventory.ContainsKey(itemName))
                {
                    inventory[itemName]--;
                    if (inventory[itemName] <= 0)
                    {
                        inventory.Remove(itemName);
                    }
                }
            }
        }
    }

    public int GetItemCount(string itemName)
    {
        return inventory.ContainsKey(itemName) ? inventory[itemName] : 0;
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