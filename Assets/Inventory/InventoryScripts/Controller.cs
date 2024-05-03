using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;





public class InventoryController : MonoBehaviour
{
    public static InventoryController Instance;  // Singleton instance

    public int stacked = 50;  // Maximum stack size for any item
    public InventoryItem[] Item;
    public delegate void ItemChanged();
    public event ItemChanged OnItemChanged;
    public GameObject itemPrefab;

    int pickedItem = -1;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
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
                    Destroy(it.gameObject);
                    Item[index] = null;
                }
                UpdateUI(it);
                OnItemChanged?.Invoke(); // Trigger event when item changes
            }
        }
    }


    public int GetItemCount(Items items)
    {
        int totalItemCount = 0;
        foreach (InventoryItem inventoryItem in Item)
        {
            ItemInside itemInside = inventoryItem.GetComponentInChildren<ItemInside>();
            if (itemInside != null && itemInside.items == items)
            {
                totalItemCount += itemInside.Count;
            }
        }
        return totalItemCount;
    }

    public bool CanAddItem(Items items)
    {
        return GetItemCount(items) < stacked;
    }


    public bool AddItem(Items items)
    {
        foreach (InventoryItem it in Item)
        {
            ItemInside itemInside = it.GetComponentInChildren<ItemInside>();
            if (itemInside != null && itemInside.items == items && itemInside.count < stacked)
            {
                itemInside.count++;
                UpdateUI(it);
                OnItemChanged?.Invoke(); // Trigger event when item changes
                return true;
            }
        }

        foreach (InventoryItem it in Item)
        {
            if (it.GetComponentInChildren<ItemInside>() == null)
            {
                StoreItem(items, it);
                OnItemChanged?.Invoke(); // Trigger event when item changes
                return true;
            }
        }

        return false; // No space to add new item
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
       // it.UpdateUI();
    }
}