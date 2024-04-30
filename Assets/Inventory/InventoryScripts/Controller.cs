using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;





public class Controller : MonoBehaviour
{
    public int stacked = 50;  // Maximum stack size for any item
    public InventoryItem[] Item;
    public GameObject itemPrefab;

    int pickedItem = -1;

    void ChosenPickedItem(int n)
    {
        if (pickedItem >= 0 && pickedItem < Item.Length)
        {
            Item[pickedItem].NotPick();
        }
        if (n >= 0 && n < Item.Length)
        {
            Item[n].pick();
            pickedItem = n;
        }
    }

    public void DiscardItem(int index)
    {
        if (index >= 0 && index < Item.Length)
        {
            InventoryItem it = Item[index];
            ItemInside itemInside = it.GetComponentInChildren<ItemInside>();

            if (itemInside != null)
            {
                if (itemInside.count > 1)
                {
                    itemInside.count--;
                }
                else
                {
                    Destroy(it.gameObject);  // This destroys the item
                    Item[index] = null;     // Set the array element to null after destroying
                }
            }
        }
    }

    public int GetItemCount(Items item)
    {
        int count = 0;
        foreach (InventoryItem it in Item)
        {
            ItemInside itemInside = it.GetComponentInChildren<ItemInside>();
            if (itemInside != null && itemInside.items == item)
            {
                count += itemInside.count;
            }
        }
        return count;
    }

    public bool CanAddItem(Items items)
    {
        return GetItemCount(items) < stacked;
    }


        public bool AddItem(Items items)
        {
            // Checking if the item can be added to an existing slot
            foreach (InventoryItem it in Item)
            {
                ItemInside itemInside = it.GetComponentInChildren<ItemInside>();
                if (itemInside != null && itemInside.items == items && itemInside.count < stacked)
                {
                    itemInside.count++;
                    UpdateUI(it);
                    return true;
                }
            }

            // Try to store in a new slot if existing slots are full
            foreach (InventoryItem it in Item)
            {
                if (it.GetComponentInChildren<ItemInside>() == null)
                {
                    StoreItem(items, it);
                    return true;
                }
            }

            return false;
        }

        void StoreItem(Items items, InventoryItem it)
        {
            GameObject storeInside = Instantiate(itemPrefab, it.transform);
            ItemInside itemInside = storeInside.GetComponent<ItemInside>();
            itemInside.InitialiseItem(items);
            itemInside.count = 1;  // Initialize with a count of one
            UpdateUI(it);
        }

        void UpdateUI(InventoryItem it)
        {
            
            it.UpdateUI();
        }
    }
