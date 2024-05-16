using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Selling Quest", menuName = "Quest System/Selling Quest")]


public class SellingQuest : Quest
{
    public Items requiredItem;
    public int requiredAmount;

    // Constructor
    public SellingQuest(int questNumber, string title, string desc, int favourabilityReward, Items requiredItem, int requiredAmount)
        : base(questNumber, title, desc, favourabilityReward)
    {
        this.requiredItem = requiredItem;
        this.requiredAmount = requiredAmount;
    }

    public bool CanCompleteQuest()
    {
        return inventory.GetItemCount(requiredItem.name) >= requiredAmount;
    }

    public void CompleteQuest()
    {
        if (inventory != null && CanCompleteQuest())
        {
            // Discard the required number of items
            for (int i = 0; i < requiredAmount; i++)
            {
                int itemIndex = FindItemIndexByType(requiredItem);
                if (itemIndex != -1)
                {
                    inventory.DiscardItem(itemIndex);
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

    private int FindItemIndexByType(Items itemType)
    {
        var itemsList = new List<InventoryItem>(inventory.itemsParent.GetComponentsInChildren<InventoryItem>());
        for (int i = 0; i < itemsList.Count; i++)
        {
            InventoryItem it = itemsList[i];
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
        base.complete();
        Debug.Log(title + " quest is completed. Congratulations!");
    }
}
