using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Selling Quest", menuName = "Quest System/Selling Quest")]




public class SellingQuest : Quest
{
    public Items requiredItem; 
    public int requiredAmount;

    // Constructor
    public SellingQuest(Items requiredItem, int requiredAmount)
    {
        this.requiredItem = requiredItem;
        this.requiredAmount = requiredAmount;
    }

    
    public bool CanCompleteQuest(Controller inventory)
    {
        return inventory.GetItemCount(requiredItem) >= requiredAmount;
    }

    
    public void CompleteQuest()
    {
        if (Controller.Instance != null && CanCompleteQuest(Controller.Instance))
        {
            // Discard the required number of items
            for (int i = 0; i < requiredAmount; i++)
            {
                int itemIndex = FindItemIndexByType(requiredItem);
                if (itemIndex != -1)
                {
                    Controller.Instance.DiscardItem(itemIndex);
                }
                else
                {
                    Debug.Log("Error: Not enough items left to discard.");
                    break;
                }
            }
            complete();
        }
        else
        {
            Debug.Log("Cannot complete quest. Item count not sufficient or Controller not available.");
        }
    }

    // just for Finding the number of the item in inventory
    private int FindItemIndexByType(Items itemType)
    {
        for (int i = 0; i < Controller.Instance.Item.Length; i++)
        {
            InventoryItem it = Controller.Instance.Item[i];
            ItemInside itemInside = it.GetComponentInChildren<ItemInside>();
            if (itemInside != null && itemInside.items == itemType)
            {
                return i; // Found the item 
            }
        }
        return -1; // Item not found
    }

   
    public override void complete()
    {
        if (!isComplete)
        {
            base.complete();
            Debug.Log(title + " quest is completed. Congratulations!");
           
        }
    }
}

