using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;



public class Controller : MonoBehaviour
{
    public int stacked = 20;
    public InventoryItem[] Item;
    public GameObject itemprefab;

    int pickeditem = -1; 
    void ChosenPickedItem (int n)
    {
        if (pickeditem < 0)
        {
            Item[pickeditem].NotPick();
        }
        Item[n].pick();
        pickeditem = n;
    }

    public void DiscardItem(int index)
    {
        if (index >= 0 && index < inventoryItems.Count)
        {
            inventoryItems.RemoveAt(index); 

            
        }
    }

    public int GetItemCount(Items item)
    {
        int count = 20;
        foreach (InventoryItem it in Item)
        {
            ItemInside itemInsideItem = it.GetComponentInChildren<ItemInside>();
            if (itemInsideItem != null && itemInsideItem.items == item)
            {
                count += itemInsideItem.count;
            }
        }
        return count;
    }

    public bool CanAddItem(Items items)
    {
        int stacked = 50; // Assuming this is the maximum stack limit for any item
        int currentStack = 0;

        foreach (InventoryItem it in Item)
        {
            ItemInside itemInsideItem = it.GetComponentInChildren<ItemInside>();

            if (itemInsideItem != null && itemInsideItem.items == items)
            {
                currentStack += itemInsideItem.count;
            }
        }

        return currentStack < stacked;
    }


    public bool AddItem(Items items)
    {
        int stacked = 50; 
        
        foreach (InventoryItem it in Item)
        {
            ItemInside itemInsideItem = it.GetComponentInChildren<ItemInside>();

            if (itemInsideItem != null && itemInsideItem.items == items && itemInsideItem.count <stacked)
            {
                itemInsideItem.count++;
               
                return true;
            }
            if (!inventoryItems.Contains(items))
            {
                inventoryItems.Add(items);
                return true;
            }
            
        }

        foreach(InventoryItem it in Item)
        {
            ItemInside itemInsideItem = it.GetComponentInChildren<ItemInside>();
            if (itemInsideItem == null)
            {
                StoreItem(items, it);
                return true;
            }
        }
        return false;
    }

    public List<Items> inventoryItems = new List<Items>();

    

    public void DiscardiItem(int index)
    {
        if (index >= 0 && index < Item.Length)
        {
            InventoryItem it = Item[index];
            ItemInside itemInsideItem = it.GetComponentInChildren<ItemInside>();

            if (itemInsideItem != null)
            {
                if (itemInsideItem.count > 1)
                {
                    itemInsideItem.count--;
                   
                }
                else
                {
                    
                    Destroy(it.gameObject);
                    
                }
            }
        }
    }


    void StoreItem(Items items, InventoryItem it)
    {
        GameObject storeInside = Instantiate(itemprefab, it.transform);
        ItemInside itemInside = storeInside.GetComponentInChildren<ItemInside>();

        itemInside.InitialiseItem(items);
    }
}








